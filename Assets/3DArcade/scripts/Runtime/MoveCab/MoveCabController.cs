/* MIT License

 * Copyright (c) 2020 Skurdt
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:

 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE. */

using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcade
{
    public static class MoveCabController
    {
        public static event Action<ModelConfigurationComponent> OnCurrentModelChanged;

        public static void FindModelSetup(in MoveCabData data, in Ray ray, in float maxDistance, in LayerMask layerMask)
        {
            Assert.IsNotNull(data);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, layerMask))
            {
                ModelConfigurationComponent targetModel = hitInfo.transform.GetComponent<ModelConfigurationComponent>();
                if (targetModel != null && targetModel != data.ModelSetup)
                {
                    int arcadeModelsLayer = LayerMask.NameToLayer("Arcade/ArcadeModels");

                    if (data.ModelSetup != null && data.ModelSetup.gameObject.layer != arcadeModelsLayer)
                    {
                        data.ModelSetup.transform.SetLayersRecursively(data.SavedLayer);
                    }

                    data.Set(targetModel, hitInfo.collider, hitInfo.rigidbody);

                    if (data.ModelSetup.gameObject.layer != arcadeModelsLayer)
                    {
                        data.SavedLayer = data.ModelSetup.gameObject.layer;
                        data.ModelSetup.transform.SetLayersRecursively(LayerMask.NameToLayer("Selection"));

                        OnCurrentModelChanged?.Invoke(data.ModelSetup);
                    }
                    else
                    {
                        OnCurrentModelChanged?.Invoke(null);
                    }
                }
            }
            else
            {
                data.Reset();

                OnCurrentModelChanged?.Invoke(null);
            }
        }

        public static void ManualMoveAndRotate(in Transform transform, in Rigidbody rigidbody, in Vector2 positionInput, in float rotationInput)
        {
            Assert.IsNotNull(transform);
            Assert.IsNotNull(rigidbody);

            rigidbody.constraints = RigidbodyConstraints.None;

            // Position
            if (positionInput.sqrMagnitude > 0.001f)
            {
                rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

                rigidbody.AddForce(-transform.forward * positionInput.y, ForceMode.VelocityChange);
                rigidbody.AddForce(-transform.right * positionInput.x, ForceMode.VelocityChange);
            }
            rigidbody.AddForce(Vector3.right * -rigidbody.velocity.x, ForceMode.VelocityChange);
            rigidbody.AddForce(Vector3.forward * -rigidbody.velocity.z, ForceMode.VelocityChange);

            // Rotation
            if (rotationInput < -0.5f || rotationInput > 0.5f)
            {
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;

                float angle           = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
                float targetAngle     = angle + rotationInput;
                float angleDifference = (targetAngle - angle);

                if (Mathf.Abs(angleDifference) > 180f)
                {
                    if (angleDifference < 0f)
                    {
                        angleDifference = (360f + angleDifference);
                    }
                    else if (angleDifference > 0f)
                    {
                        angleDifference = (360f - angleDifference) * -1f;
                    }
                }

                rigidbody.AddTorque(Vector3.up * angleDifference, ForceMode.VelocityChange);
                rigidbody.AddTorque(Vector3.up * -rigidbody.angularVelocity.y, ForceMode.VelocityChange);
            }
        }

        public static void AutoMoveAndRotate(in MoveCabData data, in Ray ray, in Vector3 forward, in float maxDistance, in LayerMask layerMask)
        {
            Assert.IsNotNull(data);
            Assert.IsNotNull(data.ModelSetup);
            Assert.IsNotNull(data.Collider);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, layerMask))
            {
                Transform transform = data.ModelSetup.transform;
                Collider collider   = data.Collider;
                Vector3 position    = hitInfo.point;
                Vector3 normal      = hitInfo.normal;
                float dot           = Vector3.Dot(Vector3.up, normal);
                if (dot > 0.05f)
                {
                    Vector3 newPosition     = position + new Vector3(0f, 0.1f, 0f);
                    transform.position      = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 12f);
                    transform.localRotation = Quaternion.FromToRotation(Vector3.up, normal) * Quaternion.LookRotation(-forward);
                }
                else if (dot < -0.05f)
                {
                    Vector3 newPosition     = new Vector3(position.x, transform.position.y, position.z);
                    transform.position      = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 12f);
                    transform.localRotation = Quaternion.FromToRotation(Vector3.up, -normal) * Quaternion.LookRotation(-forward);
                }
                else // Vertical surface
                {
                    Vector3 positionOffset  = normal * Mathf.Max(collider.bounds.extents.x + 0.1f, collider.bounds.extents.z + 0.1f);
                    Vector3 newPosition     = new Vector3(position.x, transform.position.y, position.z) + positionOffset;
                    transform.position      = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 12f);
                    transform.localRotation = Quaternion.LookRotation(normal);
                }
            }
        }

        public static MoveCabGrabSavedData InitGrabMode(in MoveCabData data, in Camera camera)
        {
            Assert.IsNotNull(data);
            Assert.IsNotNull(data.ModelSetup);
            Assert.IsNotNull(data.Collider);
            Assert.IsNotNull(data.Rigidbody);
            Assert.IsNotNull(camera);

            data.ScreenPoint = camera.WorldToScreenPoint(data.ModelSetup.transform.position);

            MoveCabGrabSavedData result = new MoveCabGrabSavedData
            {
                Layer                  = data.ModelSetup.gameObject.layer,
                ColliderIsTrigger      = data.Collider.isTrigger,
                RigidbodyInterpolation = data.Rigidbody.interpolation,
                CollisionDetectionMode = data.Rigidbody.collisionDetectionMode,
                RigidbodyIsKinematic   = data.Rigidbody.isKinematic
            };

            data.ModelSetup.gameObject.layer      = 0;
            data.Collider.isTrigger               = true;
            data.Rigidbody.interpolation          = RigidbodyInterpolation.Interpolate;
            data.Rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            data.Rigidbody.isKinematic            = true;

            return result;
        }

        public static void RestoreSavedValues(in MoveCabData data, MoveCabGrabSavedData savedValues)
        {
            Assert.IsNotNull(data);
            Assert.IsNotNull(data.Collider);
            Assert.IsNotNull(data.Rigidbody);
            Assert.IsNotNull(savedValues);

            data.ModelSetup.gameObject.layer      = savedValues.Layer;
            data.Collider.isTrigger               = savedValues.ColliderIsTrigger;
            data.Rigidbody.interpolation          = savedValues.RigidbodyInterpolation;
            data.Rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            data.Rigidbody.isKinematic            = savedValues.RigidbodyIsKinematic;
            data.Rigidbody.collisionDetectionMode = savedValues.CollisionDetectionMode;
        }

        //public static void Throw(in Data data, in Camera camera)
        //{
        //    Assert.IsNotNull(data);
        //    Assert.IsNotNull(data.Transform);
        //    Assert.IsNotNull(data.Collider);
        //    Assert.IsNotNull(data.Rigidbody);
        //    Assert.IsNotNull(camera);

        //    Ray ray = camera.ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
        //    bool didRayCast = Physics.Raycast(ray, out RaycastHit hitInfo, 5f, LayerMask.GetMask("Arcade/ArcadeModels"));
        //    if (!didRayCast || !data.Collider.bounds.Intersects(hitInfo.collider.bounds))
        //    {
        //        data.Transform.position = camera.transform.position + (camera.transform.forward * 1.2f) - new Vector3(0f, data.Collider.bounds.extents.y, 0f);
        //        data.Transform.localRotation = Quaternion.LookRotation(-camera.transform.forward);

        //        data.Collider.isTrigger = false;
        //        data.Rigidbody.isKinematic = false;
        //        data.Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        //        data.Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

        //        data.Rigidbody.AddForce(camera.transform.forward * 1000f, ForceMode.Impulse);
        //    }
        //}
    }
}

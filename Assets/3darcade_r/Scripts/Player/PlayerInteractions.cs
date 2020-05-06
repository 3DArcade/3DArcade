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

using UnityEngine;
using UnityEngine.InputSystem;

namespace Arcade_r.Player
{
    [RequireComponent(typeof(PlayerControls))]
    public class PlayerInteractions : MonoBehaviour
    {
        public GlobalData globalData;

        [SerializeField] private LayerMask _interactableRaycastLayers;
        [SerializeField] private Vector3 _interactableRaycastOffset    = Vector3.zero;
        [SerializeField] private float _interactableRaycastMaxDistance = 2.5f;
        [SerializeField] private bool _useMouseWheelRotation  = true;
        [SerializeField] private float _mouseWheelSensitivity = 2.0f;

        [SerializeField] private LayerMask _worldRaycastLayers;
        [SerializeField] private Vector3 _worldRaycastOffset    = new Vector3(0f, -200f, 0f);
        [SerializeField] private float _worldRaycastMaxDistance = 12f;

        private PlayerControls _playerControls;
        private Camera _camera;
        private StateContext _stateContext;

        private void Awake()
        {
            _playerControls  = GetComponent<PlayerControls>();
            _camera          = Camera.main;
            _stateContext    = new StateContext(_playerControls, this);
        }

        private void Start()
        {
            Utils.HideMouseCursor();
            _stateContext.TransitionTo<NormalState>();
        }

        private void Update()
        {
            _stateContext.Update(Time.deltaTime);

            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                Utils.ExitApp();
            }
        }

        public bool FindGrabbableEntity()
        {
            if (PhysicsUtils.RaycastFromScreen(out RaycastHit hitInfo, _camera, _interactableRaycastOffset, _interactableRaycastMaxDistance, _interactableRaycastLayers))
            {
                globalData.UpdateCurrentGame(hitInfo);
                return true;
            }

            globalData.UpdateCurrentGame(null);
            return false;
        }

        public void TryMoveEntityToWorldPoint()
        {
            if (PhysicsUtils.RaycastFromScreen(out RaycastHit hitInfo, _camera, _worldRaycastOffset, _worldRaycastMaxDistance, _worldRaycastLayers))
            {
                Vector3 hitPosition = hitInfo.point;
                float distanceFromPlayer = (hitPosition - transform.position).sqrMagnitude - 1f;
                if (distanceFromPlayer > globalData.CurrentEntityCollider.bounds.size.z)
                {
                    Vector3 hitNormal = hitInfo.normal;
                    if (Vector3.Dot(Vector3.up, hitNormal) > 0f)
                    {
                        globalData.CurrentEntityTransform.position = hitPosition;
                        if (_useMouseWheelRotation)
                        {
                            globalData.CurrentEntityTransform.RotateAround(hitPosition,
                                                                           globalData.CurrentEntityTransform.up,
                                                                           Mouse.current.scroll.ReadValue().y * _mouseWheelSensitivity * Time.deltaTime);
}
                        else
                        {
                            globalData.CurrentEntityTransform.rotation = Quaternion.FromToRotation(Vector3.up, hitNormal) * Quaternion.LookRotation(-transform.forward);
                        }
                    }
                    else
                    {
                        Vector3 positionOffset = new Vector3(hitNormal.x, 0f, hitNormal.z) * ((globalData.CurrentEntityCollider as BoxCollider).bounds.size.z * 0.7f);
                        globalData.CurrentEntityTransform.position = new Vector3(hitPosition.x, globalData.CurrentEntityTransform.position.y, hitPosition.z) + positionOffset;
                        globalData.CurrentEntityTransform.rotation = Quaternion.LookRotation(hitNormal);
                    }
                }
            }
        }
    }
}

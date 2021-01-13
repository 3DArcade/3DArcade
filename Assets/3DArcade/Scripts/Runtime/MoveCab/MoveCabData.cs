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

namespace Arcade
{
    public sealed class MoveCabData
    {
        public ModelConfigurationComponent ModelSetup;
        public Collider Collider;
        public Rigidbody Rigidbody;
        public Vector2 ScreenPoint;

        public Vector2 AimPosition;
        public float AimRotation;

        public int SavedLayer;

        public void Set(ModelConfigurationComponent targetModel, Collider collider, Rigidbody rigidbody)
        {
            ModelSetup = targetModel;
            Collider   = collider;
            if (Rigidbody != null)
                Rigidbody.angularVelocity = Vector3.zero;
            Rigidbody = rigidbody;
        }

        public void Reset()
        {
            ModelSetup  = null;
            Collider    = null;
            if (Rigidbody != null)
                Rigidbody.angularVelocity = Vector3.zero;
            Rigidbody   = null;
            ScreenPoint = Vector2.zero;
            AimPosition = Vector2.zero;
            AimRotation = 0f;
            SavedLayer  = 0;
        }
    }

    public sealed class MoveCabGrabSavedData
    {
        public int Layer;
        public bool ColliderIsTrigger;
        public CollisionDetectionMode CollisionDetectionMode;
        public RigidbodyInterpolation RigidbodyInterpolation;
        public bool RigidbodyIsKinematic;
    }
}

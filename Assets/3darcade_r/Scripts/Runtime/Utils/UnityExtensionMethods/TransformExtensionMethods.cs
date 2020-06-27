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

namespace Arcade_r
{
    public static class TransformExtensionMethods
    {
        public static void SetLayersRecursively(this Transform transform, int layer)
        {
            if (transform == null)
            {
                return;
            }

            transform.gameObject.layer = layer;
            for (int i = 0; i < transform.childCount; ++i)
            {
                Transform child = transform.GetChild(i);
                child.gameObject.layer = layer;
                SetLayersRecursively(child, layer);
            }
        }

        public static float GetWidth(this Transform transform)
        {
            if (transform == null)
            {
                return 0f;
            }

            return transform.TryGetComponent(out BoxCollider collider) ? collider.size.x : 0f;
        }

        public static float GetHeight(this Transform transform)
        {
            if (transform == null)
            {
                return 0f;
            }

            return transform.TryGetComponent(out BoxCollider collider) ? collider.size.y : 0f;
        }

        public static float GetDepth(this Transform transform)
        {
            if (transform == null)
            {
                return 0f;
            }

            return transform.TryGetComponent(out BoxCollider collider) ? collider.size.z : 0f;
        }

        public static float GetHalfWidth(this Transform transform)
        {
            if (transform == null)
            {
                return 0f;
            }

            return transform.GetWidth() * 0.5f;
        }

        public static float GetHalfHeight(this Transform transform)
        {
            if (transform == null)
            {
                return 0f;
            }

            return transform.GetHeight() * 0.5f;
        }

        public static float GetHalfDepth(this Transform transform)
        {
            if (transform == null)
            {
                return 0f;
            }

            return transform.GetDepth() * 0.5f;
        }
    }
}

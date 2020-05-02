using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcade
{
    public static class UnityExtensionMethods
    {
        public static void RunOnChildrenRecursive(this GameObject gameObject, Action<GameObject> action)
        {
            if (gameObject != null)
            {
                foreach (Transform transform in gameObject.GetComponentsInChildren<Transform>(true))
                {
                    action(transform.gameObject);
                }
            }
        }

        public static List<GameObject> GetChildren(this GameObject gameObject)
        {
            if (gameObject != null)
            {
                List<GameObject> children = new List<GameObject>();
                foreach (Transform childTransform in gameObject.transform)
                {
                    if (childTransform.gameObject != null)
                    {
                        children.Add(childTransform.gameObject);
                    }
                }
                return children;
            }

            return default;
        }
    }
}

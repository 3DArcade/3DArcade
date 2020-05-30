using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcade
{
    public static class UnityExtensionMethods
    {
        public static void RunOnChildrenRecursive(this GameObject gameObject, in Action<GameObject> action)
        {
            if (gameObject == null)
            {
                return;
            }

            Transform[] childTransforms = gameObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform childTransform in childTransforms)
            {
                action(childTransform.gameObject);
            }
        }

        public static GameObject[] GetChildren(this GameObject gameObject)
        {
            if (gameObject == null)
            {
                return null;
            }

            GameObject[] result = new GameObject[gameObject.transform.childCount];
            for (int i = 0; i < gameObject.transform.childCount; ++i)
            {
                result[i] = gameObject.transform.GetChild(i).gameObject;
            }
            return result;
        }
    }
}

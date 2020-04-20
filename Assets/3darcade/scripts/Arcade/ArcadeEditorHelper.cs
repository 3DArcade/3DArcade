#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Arcade
{
    public class ArcadeEditorHelper : MonoBehaviour
    {
        ArcadeEditorHelper()
        {
            EditorApplication.hierarchyChanged -= MyHierarchyChangedCallback;
            EditorApplication.hierarchyChanged += MyHierarchyChangedCallback;
        }

        private static void MyHierarchyChangedCallback()
        {
            GameObject activeObj = Selection.activeGameObject;
            string name;
            if (activeObj != null)
            {
                name = activeObj.name;
            }
            else
            {
                return;
            }

            Transform parentTransform = activeObj.transform.parent;
            if (parentTransform != null)
            {
                if (parentTransform.gameObject.name == "ArcadeModels" || parentTransform.gameObject.name == "GameModels" || parentTransform.gameObject.name == "PropModels")
                {
                    if (activeObj.GetComponent<ModelSetup>() == null)
                    {
                        activeObj.AddComponent<ModelSetup>();
                        Debug.Log(name + " was added to the scene in " + parentTransform.gameObject.name);
                    }
                }
            }
        }
    }
}
#endif

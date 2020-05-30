using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arcade
{
    [InitializeOnLoad]
    public static class HierarchyMonitor
    {
        static HierarchyMonitor()
        {
            if (SceneManager.GetActiveScene().name != "3darcade")
            {
                return;
            }
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }

        private static void OnHierarchyChanged()
        {
            GameObject activeObj = Selection.activeGameObject;
            if (activeObj == null)
            {
                return;
            }

            Transform parentTransform = activeObj.transform.parent;
            if (parentTransform != null)
            {
                string parentName = parentTransform.gameObject.name;
                if (parentName == "ArcadeModels" || parentName == "GameModels" || parentName == "PropModels")
                {
                    if (activeObj.GetComponent<ModelSetup>() == null)
                    {
                        _ = activeObj.AddComponent<ModelSetup>();
                        Debug.Log($"{activeObj.name} was added to the scene in {parentName}");
                    }
                }
            }
        }
    }
}

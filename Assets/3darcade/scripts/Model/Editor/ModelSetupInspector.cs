using UnityEditor;
using UnityEngine;

namespace Arcade
{
    [CustomEditor(typeof(ModelSetup))]
    public class ModelSetupInspector : Editor
    {
        public ModelSetup ModelSetupScript { get; private set; }

        private void OnEnable()
        {
            ModelSetupScript = target as ModelSetup;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(8f);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Update from MasterGamelist", GUILayout.Width(EditorGUIUtility.labelWidth), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                // Do something
            }
            GUILayout.FlexibleSpace();
            //  ModelSetupScript.id = EditorGUILayout.TextField(ModelSetupScript.id);
            EditorGUILayout.EndHorizontal();

        }
    }
}


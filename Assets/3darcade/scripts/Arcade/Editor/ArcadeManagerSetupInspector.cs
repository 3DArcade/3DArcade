using System.IO;
using UnityEditor;
using UnityEngine;

namespace Arcade
{
    [CustomEditor(typeof(ArcadeManager))]
    public class ArcadeManagerSetupInspector : Editor
    {
        public ArcadeManager ArcadeManagerSetupScript { get; private set; }

        private void OnEnable()
        {
            ArcadeManagerSetupScript = target as ArcadeManager;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(8f);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Save General Configuration", GUILayout.Width(EditorGUIUtility.labelWidth), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                FileManager.SaveJSONData<GeneralConfiguration>(ArcadeManagerSetupScript.generalConfiguration, Path.Combine(ArcadeManager.applicationPath + "/3darcade/Configuration/"), "GeneralConfiguration.json");
            }
            GUILayout.FlexibleSpace();
            //  ModelSetupScript.id = EditorGUILayout.TextField(ModelSetupScript.id);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(8f);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Save Arcade Configuration", GUILayout.Width(EditorGUIUtility.labelWidth), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                ArcadeManager.loadSaveArcadeConfiguration.SaveArcade();
            }
            if (GUILayout.Button("Delete Arcade Configuration", GUILayout.Width(EditorGUIUtility.labelWidth), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                if (ArcadeManager.arcadesConfigurationList.Count > 1)
                {
                    if (ArcadeManagerSetupScript.id != ArcadeManagerSetupScript.generalConfiguration.mainMenuArcadeConfiguration)
                    {
                        ArcadeManager.loadSaveArcadeConfiguration.DeleteArcadeConfiguration(ArcadeManager.arcadeConfiguration);
                        ArcadeManager.loadSaveArcadeConfiguration.LoadArcadesConfigurationList();
                        ArcadeManager.arcadeConfiguration = ArcadeManager.arcadesConfigurationList[0];
                        ArcadeManager.loadSaveArcadeConfiguration.ResetArcade();
                        ArcadeManager.loadSaveArcadeConfiguration.LoadArcade(ArcadeManager.arcadeConfiguration);
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Can't delete this Arcade Configuration", "Can't delete this Arcade Configuration. This Arcade Configuration is used as the startup Arcade Configuration. First choose a different one in General Configuration!", "Ok");
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("Can't delete this Arcade Configuration", "Can't delete this Arcade Configuration. You need at least one Arcade Configuration", "Ok");
                }
            }
            GUILayout.FlexibleSpace();
            //  ModelSetupScript.id = EditorGUILayout.TextField(ModelSetupScript.id);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(8f);
            EditorGUILayout.BeginHorizontal();
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.wordWrap = true;
            GUILayout.Label("Add a new Arcade Configuration by changing the Id and Descriptive Name of this Arcade Configuration and then Save it.", guiStyle);
            EditorGUILayout.EndHorizontal();
        }
    }
}
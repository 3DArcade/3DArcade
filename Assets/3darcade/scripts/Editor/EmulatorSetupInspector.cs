using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Arcade
{
    [CustomEditor(typeof(EmulatorSetup))]
    public class EmulatorSetupInspector : Editor
    {
        public EmulatorSetup EmulatorSetupScript { get; private set; }

        private void OnEnable()
        {
            EmulatorSetupScript = target as EmulatorSetup;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Space(8f);
            _ = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("EMULATOR PROPERTIES", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(8f);

            _ = DrawDefaultInspector();

            GUILayout.Space(8f);
            _ = EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Delete Emulator Configuration", GUILayout.Width(200), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                DestroyImmediate(EmulatorSetupScript);
                ArcadeManager.loadSaveEmulatorConfiguration.SaveEmulators();
                ArcadeManager.loadSaveEmulatorConfiguration.LoadEmulators();
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(8f);
            _ = EditorGUILayout.BeginHorizontal();
            GUIStyle guiStyle = new GUIStyle
            {
                wordWrap = true
            };
            GUILayout.Label("Load, Save and Add Emulator Configurations from the 3DArcade Menu.", guiStyle);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(16f);
            _ = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("MASTER GAMELIST", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(8f);
            _ = EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Select", GUILayout.Width(80), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                EditorEmulatorSetupSelectMasterGamelist.ShowWindow(EmulatorSetupScript);
            }
            if (GUILayout.Button("Update", GUILayout.Width(80), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                EmulatorConfiguration emulatorConfiguration = ArcadeManager.loadSaveEmulatorConfiguration.GetEmulatorConfiguration(EmulatorSetupScript.id);
                if (emulatorConfiguration != null)
                {
                    emulatorConfiguration = UpdateMasterGamelists.UpdateMasterGamelistFromEmulatorConfiguration(emulatorConfiguration);
                    ArcadeManager.loadSaveEmulatorConfiguration.SaveEmulatorConfiguration(emulatorConfiguration);
                }
            }
            if (GUILayout.Button("Show", GUILayout.Width(80), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                EmulatorConfiguration emulatorConfiguration = ArcadeManager.loadSaveEmulatorConfiguration.GetEmulatorConfiguration(EmulatorSetupScript.id);
                if (emulatorConfiguration != null)
                {
                    List<ModelProperties> gamelist = emulatorConfiguration.masterGamelist;
                    EditorModelSetupGame.ShowWindow(EmulatorSetupScript.transform.gameObject, gamelist);
                }
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }
}

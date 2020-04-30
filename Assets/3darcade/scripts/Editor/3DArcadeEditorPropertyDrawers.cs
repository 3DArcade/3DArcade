using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Arcade
{
    [CustomPropertyDrawer(typeof(ArcadePopUp))]
    public class ArcadePopUpDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ArcadePopUp stringInList = attribute as ArcadePopUp;
            string[] list = stringInList.List;
            if (property.propertyType == SerializedPropertyType.String)
            {
                int index = Mathf.Max(0, Array.IndexOf(list, property.stringValue));
                index = EditorGUI.Popup(position, property.displayName, index, list);
                property.stringValue = list[index];
            }
            else if (property.propertyType == SerializedPropertyType.Integer)
            {
                property.intValue = EditorGUI.Popup(position, property.displayName, property.intValue, list);
            }
            else
            {
                base.OnGUI(position, property, label);
            }
        }
    }

    [CustomPropertyDrawer(typeof(ArcadeAttribute))]
    public class UnitDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ArcadeAttribute arcadeAttribute = attribute as ArcadeAttribute;
            position.width = position.width - arcadeAttribute.width - 16;
            _ = EditorGUI.PropertyField(position, property, label);
            position.x += position.width;
            position.width = arcadeAttribute.width + 16;

            if (GUI.Button(position, arcadeAttribute.label))
            {
                ModelSetup obj = property.serializedObject.targetObject as ModelSetup;
                //  if (obj == null) { return; }
                //Debug.Log("I belong to " + obj.descriptiveName);
                if (arcadeAttribute.label == "Model")
                {
                    EditorModelSetupSelectModel.ShowWindow(obj.transform.gameObject);
                }
                if (arcadeAttribute.label == "Emulator")
                {
                    if (ArcadeManager.emulatorsConfigurationList.Count < 1)
                    {
                        if (!ArcadeManager.loadSaveEmulatorConfiguration.LoadEmulatorsConfigurationList())
                        {
                            return;
                        }
                    }
                    EditorModelSetupEmulator.ShowWindow(obj.transform.gameObject);
                }
                if (arcadeAttribute.label == "Game")
                {
                    EmulatorConfiguration emulatorConfiguration = ArcadeManager.loadSaveEmulatorConfiguration.GetEmulatorConfiguration(obj.emulator);
                    if (emulatorConfiguration != null)
                    {
                        List<ModelProperties> gamelist = emulatorConfiguration.masterGamelist;
                        EditorModelSetupGame.ShowWindow(obj.transform.gameObject, gamelist);
                    }
                }
                if (arcadeAttribute.label == "Folder")
                {

                    string folder = FileManager.DialogGetFolderPath(true);
                    if (folder != null)
                    {
                        property.stringValue = folder;
                    }
                }
                if (arcadeAttribute.label == "Exe")
                {
                    string exe = FileManager.DialogGetFilePart("Select LibretroCore Executable", null, FileManager.FilePart.Name_Extension);
                    if (exe != null)
                    {
                        property.stringValue = exe;
                    }
                }
            }
        }
    }
}

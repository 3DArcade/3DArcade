﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Arcade
{
    public class BuildAssetContextMenuWindow : EditorWindow
    {
        private static string assetPath;
        private static string assetName;

        [MenuItem("Assets/Build Prefab")]
        private static void MenuOptionGetAssetPath(MenuCommand menuCommand)
        {
            var obj = Selection.activeObject;
            //Debug.Log("path is " + AssetDatabase.GetAssetPath(obj));
            //Debug.Log("name is " + obj.name);
            assetPath = AssetDatabase.GetAssetPath(obj);
            assetName = obj.name;
            EditorWindow window = GetWindow(typeof(BuildAssetContextMenuWindow)) as BuildAssetContextMenuWindow;
            window.Show();
        }
        [MenuItem("Assets/Build Prefab", true)]
        private static bool MenuOptionGetAssetPathValidation(MenuCommand menuCommand)
        {
            return AssetDatabase.GetAssetPath(Selection.activeObject).Contains("Assets/Resources") ? true : false;
        }

        void OnGUI()
        {
            GUILayout.Label("Build Asset Bundle from Model Prefab");
            GUILayout.BeginVertical();
            if (GUILayout.Button("Build for MacOS"))
            {
                Debug.Log("Building for MacOs");
                if (BuildAssetBundles.BuildForTargetOS(BuildAssetBundles.OS.MacOS, BuildTarget.StandaloneOSX, assetPath, assetName))
                {
                    Debug.Log("Success");
                }
                else
                {
                    Debug.Log("Failed");
                }
                EditorWindow window = GetWindow(typeof(BuildAssetContextMenuWindow)) as BuildAssetContextMenuWindow;
                window.Close();
            }
            if (GUILayout.Button("Build for iOS"))
            {
                Debug.Log("Building for iOs");
                if (BuildAssetBundles.BuildForTargetOS(BuildAssetBundles.OS.iOS, BuildTarget.iOS, assetPath, assetName))
                {
                    Debug.Log("Success");
                }
                else
                {
                    Debug.Log("Failed");
                }
                EditorWindow window = GetWindow(typeof(BuildAssetContextMenuWindow)) as BuildAssetContextMenuWindow;
                window.Close();
            }
            if (GUILayout.Button("Build for Windows"))
            {
                Debug.Log("Building for Windows");
                if (BuildAssetBundles.BuildForTargetOS(BuildAssetBundles.OS.Windows, BuildTarget.StandaloneWindows, assetPath, assetName))
                {
                    Debug.Log("Success");
                }
                else
                {
                    Debug.Log("Failed");
                }
                EditorWindow window = GetWindow(typeof(BuildAssetContextMenuWindow)) as BuildAssetContextMenuWindow;
                window.Close();
            }
            GUILayout.EndVertical();
        }
    }

    public class BuildAssetBundlesWindow : EditorWindow
    {
        [MenuItem("3DArcade/Build Asset Bundles from Model Prefabs", false, 10001)]
        static void Init()
        {
            EditorWindow window = GetWindow(typeof(BuildAssetBundlesWindow)) as BuildAssetBundlesWindow;
            window.Show();
        }

        void OnGUI()
        {
            GUILayout.Label("Build Asset Bundles from Model Prefabs");
            GUILayout.BeginVertical();
            if (GUILayout.Button("Build for MacOS"))
            {
                BuildAssetBundles.BuildAllForTargetOS(BuildAssetBundles.OS.MacOS);
            }
            if (GUILayout.Button("Build for iOS"))
            {
                BuildAssetBundles.BuildAllForTargetOS(BuildAssetBundles.OS.iOS);
            }
            if (GUILayout.Button("Build for Windows"))
            {
                BuildAssetBundles.BuildAllForTargetOS(BuildAssetBundles.OS.Windows);
            }
            GUILayout.EndVertical();
        }

    }

    public class BuildAssetBundleWindow : EditorWindow
    {
        private static ModelSetup modelSetup;
        private static string assetPath;
        private static string assetName;

        [InitializeOnLoadMethod]
        static void Init()
        {
            ModelSetup.ShowBuildAssetBundleWindow = ShowWindow;
        }

        public static void ShowWindow(GameObject gameObject, string assetPath, string assetName)
        {
            Debug.Log("show me now");
            BuildAssetBundleWindow.assetPath = assetPath;
            BuildAssetBundleWindow.assetName = assetName;
            modelSetup = gameObject.GetComponent<ModelSetup>();
            EditorWindow window = GetWindow(typeof(BuildAssetBundleWindow)) as BuildAssetBundleWindow;
            window.Show();
        }

        void OnGUI()
        {
            GUILayout.Label("Build Asset Bundle from Model Prefab");
            GUILayout.BeginVertical();
            if (GUILayout.Button("Build for MacOS"))
            {
                Debug.Log("Building");
                if (BuildAssetBundles.BuildForTargetOS(BuildAssetBundles.OS.MacOS, BuildTarget.StandaloneOSX, assetPath, assetName))
                {
                    Debug.Log("Success");
                }
                else
                {
                    Debug.Log("Failed");
                }
                EditorWindow window = GetWindow(typeof(BuildAssetBundleWindow)) as BuildAssetBundleWindow;
                window.Close();
            }
            if (GUILayout.Button("Build for iOS"))
            {
                BuildAssetBundles.BuildAllForTargetOS(BuildAssetBundles.OS.iOS);
            }
            if (GUILayout.Button("Build for Windows"))
            {
                BuildAssetBundles.BuildAllForTargetOS(BuildAssetBundles.OS.Windows);
            }
            GUILayout.EndVertical();
        }
    }

    public class BuildAssetBundles : MonoBehaviour
    {
        public enum OS
        {
            MacOS, iOS, tvOS, Windows, Linux, Android
        }

        private static Tuple<string, string> GetTargetPathForOS(OS selectedOS, string assetPath)
        {
            string targetPath = Application.streamingAssetsPath + "/3darcade/Configuration/Assets/" + selectedOS.ToString() + "/Misc/";
            string assetType = "Games";
            if (assetPath.Contains("/Resources/Arcades"))
            {
                targetPath = Application.streamingAssetsPath + "/3darcade/Configuration/Assets/" + selectedOS.ToString() + "/Arcades/";
                assetType = "Arcades";
            }
            if (assetPath.Contains("/Resources/Games"))
            {
                targetPath = Application.streamingAssetsPath + "/3darcade/Configuration/Assets/" + selectedOS.ToString() + "/Games/";
            }
            if (assetPath.Contains("/Resources/Props"))
            {
                targetPath = Application.streamingAssetsPath + "/3darcade/Configuration/Assets/" + selectedOS.ToString() + "/Props/";
                assetType = "Props";
            }
            return new Tuple<string, string>(targetPath, assetType);
        }

        public static bool BuildForTargetOS(OS selectedOS, BuildTarget buildTarget, string assetPath, string assetName)
        {
            Debug.Log("assetpath " + assetPath);
            Debug.Log("namepath " + assetName);

            string tempPath = Application.streamingAssetsPath + "/3darcade/Configuration/Assets/" + selectedOS.ToString() + "/Temp/";
            Tuple<string, string> tuple = GetTargetPathForOS(selectedOS, assetPath);
            string targetPath = tuple.Item1;
            string assetType = tuple.Item2;

            if (FileManager.DirectoryExists(tempPath) != null)
            {
                Directory.Delete(tempPath, true);
            }
            Directory.CreateDirectory(tempPath);

            AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
            buildMap[0].assetBundleName = assetName;
            buildMap[0].assetNames = new string[1] { "Assets/Resources/" + assetType + "/" + assetName + ".prefab" };
            Debug.Log("Build now...");
            BuildPipeline.BuildAssetBundles(tempPath, buildMap, BuildAssetBundleOptions.None, buildTarget);
            Debug.Log("Done...");

            List<FileInfo> myAssets = FileManager.FilesFromDirectory(tempPath, null);
            if (myAssets == null) { return false; }
            print("Build Asset number " + myAssets.Count());
            foreach (FileInfo file in myAssets)
            {
                Debug.Log(file.FullName);
                if (file.FullName.Contains("/Temp/Temp")) { continue; }
                string name = Path.GetFileNameWithoutExtension(file.Name);
                var existingFile = (FileManager.FileExists(targetPath, name + ".unity3d"));
                if (existingFile != null)
                {
                    File.Delete(existingFile);
                }
                Debug.Log("target " + targetPath + name + ".unity3d");
                File.Move(file.FullName, targetPath + name + ".unity3d");
            }
            Directory.Delete(tempPath, true);
            return true;
        }

        public static void BuildAllForTargetOS(OS selectedOS)
        {
            List<string> myArcadesAssetNames = FileManager.GetListOfAssetNames(ModelType.Arcade.ToString(), false);
            print("arcades nr is " + myArcadesAssetNames.Count);
            List<string> myGamesAssetNames = FileManager.GetListOfAssetNames(ModelType.Game.ToString(), false);
            print("games nr is " + myGamesAssetNames.Count);
            List<string> myPropsAssetNames = FileManager.GetListOfAssetNames(ModelType.Prop.ToString(), false);
            print("props nr is " + myGamesAssetNames.Count);
            if (selectedOS == OS.MacOS)
            {
                string assetPath = Application.streamingAssetsPath + "/3darcade/Configuration/Assets/" + OS.MacOS.ToString() + "/Temp/";
                BuildAllAssetsForTarget(BuildTarget.StandaloneOSX, assetPath, OS.MacOS);
                print("MacOS update finished");
            }
            if (selectedOS == OS.iOS)
            {
                string assetPath = Application.streamingAssetsPath + "/3darcade/Configuration/Assets/" + OS.iOS + "/Temp/";
                BuildAllAssetsForTarget(BuildTarget.iOS, assetPath, OS.iOS);
                print("iOS update finished");
            }
            if (selectedOS == OS.Windows)
            {
                string assetPath = Application.streamingAssetsPath + "/3darcade/Configuration/Assets/" + OS.Windows + "/Temp/";
                BuildAllAssetsForTarget(BuildTarget.StandaloneWindows, assetPath, OS.Windows);
                print("Windows update finished");
            }

            void BuildAllAssetsForTarget(BuildTarget buildTarget, string targetPath, OS targetOS)
            {
                if (FileManager.DirectoryExists(targetPath) != null)
                {
                    Directory.Delete(targetPath, true);
                }
                Directory.CreateDirectory(targetPath);

                BuildPipeline.BuildAssetBundles(targetPath, BuildAssetBundleOptions.None, buildTarget);

                string arcadesPath = Application.streamingAssetsPath + "/3darcade/Configuration/Assets/" + targetOS.ToString() + "/Arcades/";
                if (FileManager.DirectoryExists(arcadesPath) != null)
                {
                    Directory.Delete(arcadesPath, true);
                }
                string gamesPath = Application.streamingAssetsPath + "/3darcade/Configuration/Assets/" + targetOS.ToString() + "/Games/";
                if (FileManager.DirectoryExists(gamesPath) != null)
                {
                    Directory.Delete(gamesPath, true);
                }
                string propsPath = Application.streamingAssetsPath + "/3darcade/Configuration/Assets/" + targetOS.ToString() + "/Props/";
                if (FileManager.DirectoryExists(propsPath) != null)
                {
                    Directory.Delete(propsPath, true);
                }

                List<FileInfo> myAssets = FileManager.FilesFromDirectory(targetPath, null);
                if (myAssets == null) { return; }

                foreach (FileInfo file in myAssets)
                {
                    print(file.Name);
                    string name = Path.GetFileNameWithoutExtension(file.Name);
                    string path = Application.streamingAssetsPath + "/3darcade/Configuration/Assets/" + targetOS.ToString() + "/Misc/";

                    if (myGamesAssetNames.Contains(name))
                    {
                        path = gamesPath;
                    }
                    if (myArcadesAssetNames.Contains(name))
                    {
                        path = arcadesPath;
                    }
                    if (myPropsAssetNames.Contains(name))
                    {
                        path = propsPath;
                    }
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var existingFile = (FileManager.FileExists(path, name + ".unity3d"));
                    if (existingFile != null)
                    {
                        File.Delete(existingFile);
                    }
                    File.Move(file.FullName, path + name + ".unity3d");
                }
                Directory.Delete(targetPath, true);
            }
        }
    }
}
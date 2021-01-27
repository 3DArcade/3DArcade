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

using SK.Utilities.Unity;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

[assembly: SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "UnityEditor", Scope = "namespaceanddescendants", Target = "~N:Arcade.UnityEditor")]

namespace Arcade.UnityEditor
{
    internal enum ModelType
    {
        None,
        Arcade,
        Game,
        Prop
    }

    internal static class GlobalPaths
    {
        internal const string MODELS_FOLDER        = "Assets/3Darcade/Models";
        internal const string ARCADEMODELS_FOLDER  = MODELS_FOLDER + "/Arcades";
        internal const string GAMEMODELS_FOLDER    = MODELS_FOLDER + "/Games";
        internal const string PROPMODELS_FOLDER    = MODELS_FOLDER + "/Props";
        internal const string RESOURCES_FOLDER     = "Assets/3Darcade/Resources";
        internal const string ARCADEPREFABS_FOLDER = RESOURCES_FOLDER + "/Arcades";
        internal const string GAMEPREFABS_FOLDER   = RESOURCES_FOLDER + "/Games";
        internal const string PROPPREFABS_FOLDER   = RESOURCES_FOLDER + "/Props";
    }

    internal sealed class ModelAssetPresetImportPerFolderPreprocessor : AssetPostprocessor
    {
        private void OnPreprocessModel()
        {
            if (assetImporter.importSettingsMissing)
            {
                string assetDirectory = Path.GetDirectoryName(assetPath);
                while (!string.IsNullOrEmpty(assetDirectory))
                {
                    string[] presetGuids = AssetDatabase.FindAssets("t:Preset", new[] { assetDirectory });
                    foreach (string presetGuid in presetGuids)
                    {
                        string presetPath = AssetDatabase.GUIDToAssetPath(presetGuid);
                        if (Path.GetDirectoryName(presetPath) == assetDirectory)
                        {
                            Preset preset = AssetDatabase.LoadAssetAtPath<Preset>(presetPath);
                            if (preset.ApplyTo(assetImporter))
                                return;
                        }
                    }
                    assetDirectory = Path.GetDirectoryName(assetDirectory);
                }
            }
        }
    }

    internal sealed class ImportAssistantWindow : EditorWindow
    {
        private static string _savedBrowseDir = string.Empty;
        private static string _externalPath   = string.Empty;
        private static ModelType _modelType   = ModelType.Game;

        private static bool _closeAfterImport = true;

        [MenuItem("3DArcade/Import a new Model...", false, 10000)]
        private static void ShowWindow() => GetWindow<ImportAssistantWindow>("Import Assistant").minSize = new Vector2(290f, 120f);

        private void OnGUI()
        {
            GUILayout.Space(8f);
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Close after import:", GUILayout.Width(110f));
                _closeAfterImport = GUILayout.Toggle(_closeAfterImport, string.Empty, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            }

            GUILayout.Space(8f);
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Model file:", GUILayout.Width(80f));
                if (GUILayout.Button("Browse", GUILayout.Width(60f), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                    _externalPath = ShowSelectModelFileDialog();
                _externalPath = GUILayout.TextField(_externalPath);
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Model type:", GUILayout.Width(80f));
                _modelType = (ModelType)EditorGUILayout.EnumPopup(_modelType);
            }

            GUILayout.Space(8f);
            if (string.IsNullOrEmpty(_externalPath) || !Path.GetExtension(_externalPath).Equals(".fbx", System.StringComparison.OrdinalIgnoreCase))
                EditorGUILayout.HelpBox("No valid model selected!", MessageType.Error);
            else if (_modelType == ModelType.None)
                EditorGUILayout.HelpBox("Select a model type other than 'None'!", MessageType.Error);
            else if (_externalPath.Contains(Application.dataPath))
                EditorGUILayout.HelpBox("The model must be located outside of the project!", MessageType.Error);
            else
            {
                if (GUILayout.Button("Import", GUILayout.Height(32f)))
                {
                    string assetName      = Path.GetFileName(_externalPath);
                    string assetNameNoExt = Path.GetFileNameWithoutExtension(assetName);
                    string destinationFolder;
                    switch (_modelType)
                    {
                        case ModelType.Arcade:
                            destinationFolder = $"{GlobalPaths.ARCADEMODELS_FOLDER}/{assetNameNoExt}";
                            break;
                        case ModelType.Game:
                            destinationFolder = $"{GlobalPaths.GAMEMODELS_FOLDER}/{assetNameNoExt}";
                            break;
                        case ModelType.Prop:
                            destinationFolder = $"{GlobalPaths.PROPMODELS_FOLDER}/{assetNameNoExt}";
                            break;
                        default:
                            return;
                    }

                    if (!Directory.Exists(destinationFolder))
                        _ = Directory.CreateDirectory(destinationFolder);

                    string destinationPath = Path.Combine(destinationFolder, assetName);
                    File.Copy(_externalPath, destinationPath, true);
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

                    GameObject asset            = AssetDatabase.LoadAssetAtPath<GameObject>(destinationPath);
                    string assetPath            = AssetDatabase.GetAssetPath(asset);
                    ModelImporter modelImporter = AssetImporter.GetAtPath(assetPath) as ModelImporter;
                    if (modelImporter != null)
                    {
                        if (Utils.ExtractTextures(assetPath, modelImporter, "textures"))
                            Utils.ExtractMaterials(assetPath, "materials");
                    }
                    else
                        Debug.LogError("modelImporter is null");

                    Utils.SaveAsPrefab(asset, _modelType);

                    if (_closeAfterImport)
                        GetWindow<ImportAssistantWindow>().Close();
                }
            }
        }

        private string ShowSelectModelFileDialog()
        {
            string filePath = EditorUtility.OpenFilePanel("Select FBX", _savedBrowseDir, "fbx");
            if (!string.IsNullOrEmpty(filePath))
                _savedBrowseDir = Path.GetDirectoryName(filePath);
            return filePath;
        }
    }

    internal sealed class GameObjectContextMenus
    {
        [MenuItem("GameObject/ArcadeEditorExtensions/Node/Add BoxCollider (Replace existing)", false, 11)]
        private static void GameObjectAddBoxCollider()
        {
            GameObject selectedObj = Selection.activeObject as GameObject;

            Undo.SetCurrentGroupName("Add/Replace BoxCollider");

            if (selectedObj.TryGetComponent(out Collider collider))
                Undo.DestroyObjectImmediate(collider);
            Utils.AddBoxCollider(selectedObj);

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        [MenuItem("GameObject/ArcadeEditorExtensions/Node/Add Non-Kinematic RigidBody (Replace existing)", false, 12)]
        private static void GameObjectAddNonKinematicRigidBody()
        {
            GameObject selectedObj = Selection.activeObject as GameObject;

            Undo.SetCurrentGroupName("Add/Replace Non-Kinematic Rigidbody");

            if (selectedObj.TryGetComponent(out Rigidbody rigidbody))
                Undo.DestroyObjectImmediate(rigidbody);
            Utils.AddRigidBody(selectedObj, false, 80f);

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        [MenuItem("GameObject/ArcadeEditorExtensions/Node/Add Kinematic RigidBody (Replace existing)", false, 13)]
        private static void GameObjectAddKinematicRigidBody()
        {
            GameObject selectedObj = Selection.activeObject as GameObject;

            Undo.SetCurrentGroupName("Add/Replace Kinematic Rigidbody");

            if (selectedObj.TryGetComponent(out Rigidbody rigidbody))
                Undo.DestroyObjectImmediate(rigidbody);
            Utils.AddRigidBody(selectedObj, true, 1f);

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        [MenuItem("GameObject/ArcadeEditorExtensions/Node/Set as marquee", false, 14)]
        private static void GameObjectSetAsMarquee() => Utils.SetGameObjectAsMarquee(Selection.activeObject as GameObject);

        [MenuItem("GameObject/ArcadeEditorExtensions/Node/Set as monitor", false, 15)]
        private static void GameObjectSetAsMonitor() => Utils.SetGameObjectAsMonitor(Selection.activeObject as GameObject);

        [MenuItem("GameObject/ArcadeEditorExtensions/Node/Set as generic", false, 16)]
        private static void GameObjectSetAsGeneric()
        {
            GameObject selectedObj = Selection.activeObject as GameObject;

            Undo.SetCurrentGroupName("Set as generic");

            Transform parent = selectedObj.transform.parent;
            if (parent != null)
            {
                if (selectedObj.TryGetComponent(out NodeTag nodeTag))
                    Undo.DestroyObjectImmediate(nodeTag);
                _ = Undo.AddComponent<GenericNodeTag>(selectedObj);
            }

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        [MenuItem("GameObject/ArcadeEditorExtensions/Material/Set as transparent", false, 21)]
        private static void GameObjectSetAsTransparent()
        {
            GameObject selectedObj = Selection.activeObject as GameObject;

            Renderer renderer = selectedObj.GetComponent<Renderer>();
            Undo.RecordObject(renderer.sharedMaterial, "Set as transparent");
            Utils.SetupTransparentMaterial(renderer.sharedMaterial);
        }

        //[MenuItem("GameObject/ArcadeEditorExtensions/Material/Set as emissive", false, 22)]
        //private static void GameObjectSetAsEmissive()
        //{
        //    GameObject selectedObj = Selection.activeObject as GameObject;

        //    Renderer renderer = selectedObj.GetComponent<MeshRenderer>();
        //    Undo.RecordObject(renderer.sharedMaterial, "Set as emissive");
        //    Utils.SetupEmissiveMaterial(renderer.sharedMaterial/*, Color.white * 1.4f*/);
        //}

        // ***************
        // Validation
        // ***************
        [MenuItem("GameObject/ArcadeEditorExtensions/Node/Add BoxCollider (Replace existing)", true)]
        private static bool GameObjectAddBoxColliderValidation() => Selection.activeObject as GameObject != null;

        [MenuItem("GameObject/ArcadeEditorExtensions/Node/Add Non-Kinematic RigidBody (Replace existing)", true)]
        private static bool GameObjectAddNonKinematicRigidBodyValidation() => GameObjectAddBoxColliderValidation();

        [MenuItem("GameObject/ArcadeEditorExtensions/Node/Add Kinematic RigidBody (Replace existing)", true)]
        private static bool GameObjectAddKinematicRigidBodyValidation() => GameObjectAddBoxColliderValidation();

        [MenuItem("GameObject/ArcadeEditorExtensions/Node/Set as marquee", true)]
        private static bool GameObjectSetAsMarqueeValidation()
        {
            GameObject selectedObj = Selection.activeObject as GameObject;
            return selectedObj != null
                && selectedObj.transform.parent != null
                && selectedObj.TryGetComponent(out Renderer renderer)
                && !renderer.sharedMaterial.name.Equals("Default-Material");
        }

        [MenuItem("GameObject/ArcadeEditorExtensions/Node/Set as monitor", true)]
        private static bool GameObjectSetAsMonitorValidation() => GameObjectSetAsMarqueeValidation();

        [MenuItem("GameObject/ArcadeEditorExtensions/Node/Set as generic", true)]
        private static bool GameObjectSetAsGenericValidation() => GameObjectSetAsMarqueeValidation();

        [MenuItem("GameObject/ArcadeEditorExtensions/Material/Set as transparent", true)]
        private static bool GameObjectSetAsTransparentValidation() => GameObjectSetAsMarqueeValidation();

        [MenuItem("GameObject/ArcadeEditorExtensions/Material/Set as emissive", true)]
        private static bool GameObjectSetAsEmissiveValidation() => GameObjectSetAsMarqueeValidation();
    }

    internal static class Utils
    {
        //private static readonly Color _marqueeEmissiveColor = Color.white * 1.4f;
        //private static readonly Color _monitorEmissiveColor = Color.white * 1.3f;

        internal static ModelType GetModelType(string assetPath)
        {
            ModelType result = ModelType.None;
            if (assetPath.StartsWith(GlobalPaths.ARCADEMODELS_FOLDER, System.StringComparison.OrdinalIgnoreCase))
                result = ModelType.Arcade;
            else if (assetPath.StartsWith(GlobalPaths.GAMEMODELS_FOLDER, System.StringComparison.OrdinalIgnoreCase))
                result = ModelType.Game;
            else if (assetPath.StartsWith(GlobalPaths.PROPMODELS_FOLDER, System.StringComparison.OrdinalIgnoreCase))
                result = ModelType.Prop;
            return result;
        }

        internal static bool ExtractTextures(string assetPath, ModelImporter modelImporter, string subFolder = null)
        {
            string modelPath         = Path.GetDirectoryName(assetPath);
            string texturesDirectory = !string.IsNullOrEmpty(subFolder) ? Path.Combine(modelPath, subFolder) : modelPath;

            if (!Directory.Exists(texturesDirectory))
                _ = Directory.CreateDirectory(texturesDirectory);

            if (!modelImporter.ExtractTextures(texturesDirectory))
            {
                Debug.LogError("Failed to extract textures");
                return false;
            }

            AssetDatabase.Refresh(ImportAssetOptions.Default);
            Debug.Log("Extracted textures");
            return true;
        }

        internal static void ExtractMaterials(string assetPath, string subFolder = null)
        {
            string modelPath          = Path.GetDirectoryName(assetPath);
            string materialsDirectory = !string.IsNullOrEmpty(subFolder) ? Path.Combine(modelPath, subFolder) : modelPath;

            if (!Directory.Exists(materialsDirectory))
                _ = Directory.CreateDirectory(materialsDirectory);

            HashSet<string> assetsToReload = new HashSet<string>();
            IEnumerable<Object> materials  = AssetDatabase.LoadAllAssetsAtPath(assetPath).Where(x => x.GetType() == typeof(Material));
            foreach (Object obj in materials)
            {
                Material material = obj as Material;
                if (material != null && material.mainTexture != null)
                    material.color = Color.white;
                string newAssetPath = Path.Combine(materialsDirectory, $"{obj.name}.mat");
                newAssetPath        = AssetDatabase.GenerateUniqueAssetPath(newAssetPath);
                string error        = AssetDatabase.ExtractAsset(obj, newAssetPath);
                if (string.IsNullOrEmpty(error))
                    _ = assetsToReload.Add(assetPath);
            }

            if (assetsToReload.Count > 0)
            {
                foreach (string path in assetsToReload)
                {
                    _ = AssetDatabase.WriteImportSettingsIfDirty(path);
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                }
                Debug.Log("Extracted materials");
            }
            else
                Debug.LogWarning("Failed to extract materials (Already extracted?)");
        }

        internal static void SaveAsPrefab(GameObject obj, ModelType modelType)
        {
            GameObject tempObj   = Object.Instantiate(obj);
            string prefabsFolder;
            switch (modelType)
            {
                case ModelType.Arcade:
                {
                    prefabsFolder = GlobalPaths.ARCADEPREFABS_FOLDER;
                    AddRigidBody(tempObj, true, 1f);
                }
                break;
                case ModelType.Game:
                {
                    prefabsFolder = GlobalPaths.GAMEPREFABS_FOLDER;
                    AddBoxCollider(tempObj);
                    AddRigidBody(tempObj, false, 80f);
                }
                break;
                case ModelType.Prop:
                {
                    prefabsFolder = GlobalPaths.PROPPREFABS_FOLDER;
                    AddBoxCollider(tempObj);
                    AddRigidBody(tempObj, false, 80f);
                }
                break;
                case ModelType.None:
                default:
                    return;
            }

            if (!Directory.Exists(prefabsFolder))
                _ = Directory.CreateDirectory(prefabsFolder);

            RenameNodes(tempObj.transform, modelType);

            string destinationPath = Path.Combine(prefabsFolder, $"{obj.name}.prefab");
            if (File.Exists(destinationPath))
            {
                File.Delete(destinationPath);
                File.Delete($"{destinationPath}.meta");
            }

            GameObject newObj = PrefabUtility.SaveAsPrefabAsset(tempObj, destinationPath, out bool success);
            if (success)
            {
                Debug.Log($"{modelType} prefab '{obj.name}' created");
                Selection.activeGameObject = newObj;
                EditorGUIUtility.PingObject(newObj);
                _ = AssetDatabase.OpenAsset(newObj);
            }
            else
                Debug.LogError($"{modelType} prefab '{obj.name}' creation failed");

            Object.DestroyImmediate(tempObj);
        }

        internal static void AddBoxCollider(GameObject obj)
        {
            if (obj.GetComponent<Collider>() != null || obj.GetComponentInChildren<Collider>() != null)
                return;

            BoxCollider boxCollider = Undo.AddComponent<BoxCollider>(obj);

            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            if (renderers == null || renderers.Length == 0)
                return;

            Bounds bounds = new Bounds(renderers[0].bounds.center, renderers[0].bounds.size);
            foreach (Renderer renderer in renderers)
                bounds.Encapsulate(renderer.bounds);
            boxCollider.center = boxCollider.transform.InverseTransformPoint(bounds.center);
            Vector3 size       = boxCollider.transform.InverseTransformVector(bounds.size);
            boxCollider.size   = new Vector3(math.abs(size.x), math.abs(size.y), math.abs(size.z));
        }

        internal static void AddRigidBody(GameObject obj, bool kinematic, float mass)
        {
            if (obj.GetComponent<Rigidbody>() != null)
                return;

            Rigidbody rigidbody   = Undo.AddComponent<Rigidbody>(obj);
            rigidbody.useGravity  = !kinematic;
            rigidbody.isKinematic = kinematic;
            rigidbody.mass        = mass;
        }

        internal static void SetGameObjectAsMarquee(GameObject obj)
        {
            Undo.SetCurrentGroupName("Set as marquee");

            Transform parent = obj.transform.parent;
            if (parent != null && parent.childCount >= 1)
            {
                Undo.RegisterFullObjectHierarchyUndo(parent, "Set as marquee");
                obj.transform.SetAsFirstSibling();
            }

            if (obj.TryGetComponent(out NodeTag nodeTag))
                Undo.DestroyObjectImmediate(nodeTag);
            _ = Undo.AddComponent<MarqueeNodeTag>(obj);

            //MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
            //Undo.RecordObject(meshRenderer.sharedMaterial, "Set as marquee");
            //SetupEmissiveMaterial(meshRenderer.sharedMaterial, _marqueeEmissiveColor);

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        internal static void SetGameObjectAsMonitor(GameObject obj)
        {
            Undo.SetCurrentGroupName("Set as monitor");

            Transform parent = obj.transform.parent;
            if (parent != null && parent.childCount >= 2)
            {
                Undo.RegisterFullObjectHierarchyUndo(parent, "Set as monitor");
                obj.transform.SetSiblingIndex(1);
            }

            if (obj.TryGetComponent(out NodeTag nodeTag))
                Undo.DestroyObjectImmediate(nodeTag);
            _ = Undo.AddComponent<ScreenNodeTag>(obj);

            //MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
            //Undo.RecordObject(meshRenderer.sharedMaterial, "Set as monitor");
            //SetupEmissiveMaterial(meshRenderer.sharedMaterial, _monitorEmissiveColor);

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        //internal static void SetupEmissiveMaterial(Material material, Color color)
        //{
        //    material.SetEmissive(true);
        //    material.SetEmissiveColor(color);
        //    Texture mainTex = material.GetBaseTexture();
        //    if (mainTex != null)
        //    {
        //        material.SetEmissiveTexture(mainTex);
        //    }
        //}

        internal static void SetupTransparentMaterial(Material material)
        {
            material.EnableKeyword("_BLENDMODE_ALPHA");
            material.EnableKeyword("_BLENDMODE_PRESERVE_SPECULAR_LIGHTING");
            material.EnableKeyword("_ENABLE_FOG_ON_TRANSPARENT");
            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.renderQueue = 3000;
            material.SetInt("_AlphaDstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_SurfaceType", 1);
            material.SetInt("_ZTestDepthEqualForOpaque", 4);
            material.SetInt("_ZWrite", 0);
        }

        private static void RenameNodes(Transform transform, ModelType modelType)
        {
            Transform[] children = GetAllChildren(transform, modelType == ModelType.Arcade);
            if (children.Length == 0)
                return;

            string meshNumberFormat = children.Length > 100 ? "{0:000}" : "{0:00}";
            for (int i = 0; i < children.Length; ++i)
            {
                Transform child = children[i];
                if (!child.name.StartsWith(transform.name))
                {
                    child.gameObject.StripCloneFromName();
                    child.name += $"_Mesh{string.Format(meshNumberFormat, i)}";
                }
            }
        }

        private static Transform[] GetAllChildren(Transform root, bool getNestedNodes)
        {
            List<Transform> result = new List<Transform>();
            foreach (Transform childTransform in root)
            {
                result.Add(childTransform);
                if (getNestedNodes)
                    result.AddRange(GetAllChildren(childTransform, getNestedNodes));
            }
            return result.ToArray();
        }
    }
}

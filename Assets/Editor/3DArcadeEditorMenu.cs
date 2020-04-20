using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Arcade
{

#if UNITY_EDITOR
public class EditorLoadArcadeConfiguration : EditorWindow
{
    Vector2 scrollPos;

    [MenuItem("3DArcade/Load Arcade Configuration")]
    static void Init()
    {
        ArcadeManager.arcadesConfigurationList.Clear();
        if (!ArcadeManager.loadSaveArcadeConfiguration.LoadArcadesConfigurationList())
        {
            return;
        }
        EditorWindow window = GetWindow(typeof(EditorLoadArcadeConfiguration)) as EditorLoadArcadeConfiguration;
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Load arcade configuration from file");
       
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);
        GUILayout.BeginVertical();
        foreach (ArcadeConfiguration arcadeConfiguration in ArcadeManager.arcadesConfigurationList)
        {
            if (GUILayout.Button(arcadeConfiguration.descriptiveName))
            {
                ArcadeManager.arcadeConfiguration = arcadeConfiguration;
                ArcadeManager.loadSaveArcadeConfiguration.ResetArcade();
                ArcadeManager.loadSaveArcadeConfiguration.LoadArcade(arcadeConfiguration);
                EditorWindow window = GetWindow(typeof(EditorLoadArcadeConfiguration)) as EditorLoadArcadeConfiguration;
                window.Close();
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }
}

public class EditorLoadArcadeConfigurationPopUP : EditorWindow
{
    Vector2 scrollPos;

    [InitializeOnLoadMethod]
    static void Init()
    {
        ArcadeManager.ShowSelectArcadeConfigurationWindow = ShowWindow;
    }

    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(EditorLoadArcadeConfigurationPopUP)) as EditorLoadArcadeConfigurationPopUP;
        window.Show();
    }
        
    void OnGUI()
    {
        GUILayout.Label("Load arcade configuration from file");
        ArcadeManager.arcadesConfigurationList.Clear();
        if (ArcadeManager.arcadesConfigurationList.Count < 1)
        {
            if (!ArcadeManager.loadSaveArcadeConfiguration.LoadArcadesConfigurationList())
            {
                return;
            }
        }
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);
        GUILayout.BeginVertical();
        foreach (ArcadeConfiguration arcadeConfiguration in ArcadeManager.arcadesConfigurationList)
        {
            if (GUILayout.Button(arcadeConfiguration.descriptiveName))
            {
                ArcadeManager.arcadeConfiguration = arcadeConfiguration;
                ArcadeManager.loadSaveArcadeConfiguration.ResetArcade();
                ArcadeManager.loadSaveArcadeConfiguration.LoadArcade(arcadeConfiguration);
                EditorWindow window = GetWindow(typeof(EditorLoadArcadeConfigurationPopUP)) as EditorLoadArcadeConfigurationPopUP;
                window.Close();
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }
}

public class EditorSaveArcadeConfiguration : EditorWindow
{
    [MenuItem("3DArcade/Save Arcade Configuration")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(EditorSaveArcadeConfiguration)) as EditorSaveArcadeConfiguration;
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Save arcade configuration to file");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            ArcadeManager.loadSaveArcadeConfiguration.SaveArcade();
            EditorWindow window = GetWindow(typeof(EditorSaveArcadeConfiguration)) as EditorSaveArcadeConfiguration;
            window.Close();
        }
        GUILayout.EndHorizontal();
    }
}

public class EditorLoadEmulatorsConfiguration : EditorWindow
{
    [MenuItem("3DArcade/Load Emulators Configuration")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(EditorLoadEmulatorsConfiguration)) as EditorLoadEmulatorsConfiguration;
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Load emulators configuration from file");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Load"))
        {
            ArcadeManager.loadSaveEmulatorConfiguration.LoadEmulators();
            EditorWindow window = GetWindow(typeof(EditorLoadEmulatorsConfiguration)) as EditorLoadEmulatorsConfiguration;
            window.Close();
        }
        GUILayout.EndHorizontal();
    }
}

public class EditorSaveEmulatorsConfiguration : EditorWindow
{
    [MenuItem("3DArcade/Save Emulators Configuration")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(EditorSaveEmulatorsConfiguration)) as EditorSaveEmulatorsConfiguration;
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Save emulators configuration to file");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            ArcadeManager.loadSaveEmulatorConfiguration.SaveEmulators();
            EditorWindow window = GetWindow(typeof(EditorSaveEmulatorsConfiguration)) as EditorSaveEmulatorsConfiguration;
            window.Close();
        }
        GUILayout.EndHorizontal();
    }
}

public class EditorAddEmulatorConfiguration : EditorWindow
{
    [MenuItem("3DArcade/Add Emulator Configuration")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(EditorAddEmulatorConfiguration)) as EditorAddEmulatorConfiguration;
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Add emulator configuration into the editor");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
        {
            GameObject obj = GameObject.Find("Emulators");
            if (obj != null)
            {
                obj.AddComponent<EmulatorSetup>();

            }
        }
        GUILayout.EndHorizontal();
    }
}

public class EditorUpdateMasterGamelists : EditorWindow
{
    [MenuItem("3DArcade/Update MasterGamelists")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(EditorUpdateMasterGamelists)) as EditorUpdateMasterGamelists;
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Updating masterGamelist for each emulator configuration");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Update"))
        {
            UpdateMasterGamelists.UpdateAll();
        }
        GUILayout.EndHorizontal();
    }
}

public class EditorLoadAssetbundles : EditorWindow
{
    private static GameObject editorModelcache;

    [MenuItem("3DArcade/Load All External Models into the Editor")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(EditorLoadAssetbundles)) as EditorLoadAssetbundles;
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Load all External models into the editor");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Load"))
        {
            if (editorModelcache != null)
            {
                foreach (Transform child in editorModelcache.transform)
                {
                    DestroyImmediate(child.gameObject);
                }
            }
            AssetBundle.UnloadAllAssetBundles(true);
            editorModelcache = new GameObject();
            editorModelcache.name = "EditorModelCache";
            GameObject arcadeType = new GameObject();
            arcadeType.name = "ArcadeModelsCache";
            arcadeType.transform.parent = editorModelcache.transform;
            GameObject gameType = new GameObject();
            gameType.name = "GameModelsCache";
            gameType.transform.parent = editorModelcache.transform;
            GameObject propType = new GameObject();
            propType.name = "PropModelsCache";
            propType.transform.parent = editorModelcache.transform;
            ArcadeManager.modelAssets = new List<AssetBundle>();
            var files = FileManager.FilesFromDirectory(ArcadeManager.applicationPath + "/3darcade/Configuration/Assets/" + ArcadeManager.currentOS.ToString() + "/Arcades/", "*.unity3d");
            if (files != null) { AddObject("Arcades", arcadeType); }
            files = FileManager.FilesFromDirectory(ArcadeManager.applicationPath + "/3darcade/Configuration/Assets/" + ArcadeManager.currentOS.ToString() + "/Games/", "*.unity3d");
            if (files != null) { AddObject("Games", gameType); }
            files = FileManager.FilesFromDirectory(ArcadeManager.applicationPath + "/3darcade/Configuration/Assets/" + ArcadeManager.currentOS.ToString() + "/Props/", "*.unity3d");
            if (files != null) { AddObject("Props", propType); }
            arcadeType.SetActive(false);
            gameType.SetActive(false);
            propType.SetActive(false);
            EditorWindow window = GetWindow(typeof(EditorLoadAssetbundles)) as EditorLoadAssetbundles;
            window.Close();

            void AddObject(string type, GameObject gameObject)
            {
                foreach (System.IO.FileInfo file in files)
                {
                    var assetName = System.IO.Path.GetFileNameWithoutExtension(file.Name);
                    List<AssetBundle> prefab = ArcadeManager.modelAssets.Where(x => x.name == assetName).ToList();
                    if (prefab.Count < 1)
                    {
                        var path = FileManager.FileExists(ArcadeManager.applicationPath + "/3darcade/Configuration/Assets/" + ArcadeManager.currentOS.ToString() + "/" + type + "/", file.Name);
                        if (path != null)
                        {
                            var asset = AssetBundle.LoadFromFile(path);
                            if (asset != null)
                            {
                                ArcadeManager.modelAssets.Add(asset);
                                prefab.Add(asset);
                            }
                        }
                    }
                    if (prefab.Count > 0 && prefab[0].name != null && prefab[0].name != "")
                    {
                        Debug.Log("instanceme " + prefab[0].name);
                        GameObject me = prefab[0].LoadAsset(prefab[0].name) as GameObject;
                        GameObject child = Instantiate(me);
                        child.transform.parent = gameObject.transform;
                    }
                }
            }
        }
        GUILayout.EndHorizontal();
    }
}

public class EditorModelSetupGame : EditorWindow
{
    private static ModelSetup modelSetup;
    private static List<ModelProperties> gameList;
    private string searchString = "";
    private bool startsWith = true;
    private bool contains = false;
    Vector2 scrollPos;

    [InitializeOnLoadMethod]
    static void Init()
    {
        ModelSetup.ShowSelectGameWindow = ShowWindow;
    }

    public static void ShowWindow(GameObject gameObject, List<ModelProperties> gamelist)
    {
        modelSetup = gameObject.GetComponent<ModelSetup>();
        gameList = gamelist.OrderBy(x => x.descriptiveName).ToList();
        EditorWindow window = GetWindow(typeof(EditorModelSetupGame)) as EditorModelSetupGame;
        window.Show();
    }

    void OnGUI()
    {
        if (gameList == null || gameList.Count < 1)
        {
            return;
        }

        GUILayout.Label("Select a game");
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.Label("Search: ");

        startsWith = EditorGUILayout.Toggle(" starts ", startsWith);
        //Debug.Log("Starts is " + startsWith);
        if (startsWith)
        {
            contains = !startsWith;
        }
        contains = EditorGUILayout.Toggle(" contains ", contains);
        //Debug.Log("Contains is " + contains);
        if (contains)
        {
            startsWith = !contains;
        }
        searchString = GUILayout.TextField(searchString, GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);

        GUILayout.BeginVertical();
        var filtered = gameList;
        if (startsWith)
        {
            filtered = gameList.Where(x => x.descriptiveName.ToLower().StartsWith(searchString.ToLower(), System.StringComparison.CurrentCulture)).ToList();
        }
        if (contains)
        {
            filtered = gameList.Where(x => x.descriptiveName.ToLower().Contains(searchString.ToLower())).ToList();
        }

        if (filtered.Count() < 1) { filtered = gameList; }
        foreach (ModelProperties game in filtered)
        {
            if (GUILayout.Button(game.descriptiveName))
            {
                if (modelSetup != null)
                {
                    modelSetup.SetupModelPropertiesFromEmulatorMasterGameList(game.id, modelSetup.emulator);
                }
                EditorWindow window = GetWindow(typeof(EditorModelSetupEmulator)) as EditorModelSetupEmulator;
                window.Close();
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }
}

public class EditorModelSetupEmulator : EditorWindow
{
    private static ModelSetup modelSetup;
    Vector2 scrollPos;

    [InitializeOnLoadMethod]
    static void Init()
    {
        ModelSetup.ShowSelectEmulatorWindow = ShowWindow;
    }

    public static void ShowWindow(GameObject gameObject)
    {
        modelSetup = gameObject.GetComponent<ModelSetup>();
        EditorWindow window = GetWindow(typeof(EditorModelSetupEmulator)) as EditorModelSetupEmulator;
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Select an Emulator Configuration");
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);

        GUILayout.BeginVertical();
        foreach (EmulatorConfiguration emulatorsConfiguration in ArcadeManager.emulatorsConfigurationList)
        {
            if (GUILayout.Button(emulatorsConfiguration.emulator.descriptiveName))
            {
                modelSetup.emulator = emulatorsConfiguration.emulator.id;
                EditorWindow window = GetWindow(typeof(EditorModelSetupEmulator)) as EditorModelSetupEmulator;
                window.Close();
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }
}

public class EditorModelSetupSelectModel : EditorWindow
{
    private static ModelSetup modelSetup;
    private static ModelType modelType;
    private static List<string> assetNames = new List<string>();
    Vector2 scrollPos;

    [InitializeOnLoadMethod]
    static void Init()
    {
        ModelSetup.ShowSelectModelWindow = ShowWindow;
    }

    public static void ShowWindow(GameObject obj)
    {
        Debug.Log("show");
        if (obj == null) { return; }
        Debug.Log("me " + obj.name);
        modelSetup = obj.GetComponent<ModelSetup>();
        if (modelSetup == null) { return; }
        modelType = ModelType.Game;
        if (obj.tag == "arcademodel") { modelType = ModelType.Arcade; }
        if (obj.tag == "propmodel") { modelType = ModelType.Prop; }
        assetNames = FileManager.GetListOfAssetNames(modelType.ToString(), true);
        assetNames = assetNames.Concat(FileManager.GetListOfAssetNames(modelType.ToString(), false)).Distinct().ToList();
        EditorWindow window = GetWindow(typeof(EditorModelSetupSelectModel)) as EditorModelSetupSelectModel;
        window.Show();
    }

    void OnGUI()
    {
        
        GUILayout.Label("Select " + modelType + " model");
      //  List<string> assetNames = new List<string>();
      
        ///Debug.Log("ccount " + assetNames.Count);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);
        GUILayout.BeginVertical();
        foreach (string assetName in assetNames)
        {
            if (GUILayout.Button(assetName))
            {
                Debug.Log("i am " + modelSetup.descriptiveName);

                modelSetup.model = assetName;
                EditorWindow window = GetWindow(typeof(EditorModelSetupSelectModel)) as EditorModelSetupSelectModel;
                window.Close();
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }
}

public class EditorEmulatorSetupSelectMasterGamelist : EditorWindow
{
    private static EmulatorSetup emulatorSetup;
    Vector2 scrollPos;

    [InitializeOnLoadMethod]
    static void Init()
    {
        Arcade.EmulatorSetup.ShowSelectMasterGamelistWindow = ShowWindow;
    }

    public static void ShowWindow(EmulatorSetup emulatorSetup)
    {
        EditorEmulatorSetupSelectMasterGamelist.emulatorSetup = emulatorSetup;
        EditorWindow window = GetWindow(typeof(EditorEmulatorSetupSelectMasterGamelist)) as EditorEmulatorSetupSelectMasterGamelist;
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Select a Master Gamelist for " + emulatorSetup.descriptiveName + " with id " + emulatorSetup.id);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);
        GUILayout.BeginVertical();
        if (GUILayout.Button("Select Mame, Hyperspin or 3darcade Master Gamelist"))
        {
            FileManager.DialogGetMasterGamelist(emulatorSetup.id, "xml,atf");
            EditorWindow window = GetWindow(typeof(EditorEmulatorSetupSelectMasterGamelist)) as EditorEmulatorSetupSelectMasterGamelist;
            window.Close();
        }
        if (GUILayout.Button("Select Mame CatVer .ini file."))
        {
            FileManager.DialogGetMasterGamelist(emulatorSetup.id, "ini");
            EditorWindow window = GetWindow(typeof(EditorEmulatorSetupSelectMasterGamelist)) as EditorEmulatorSetupSelectMasterGamelist;
            window.Close();
        }
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    } 
}
#endif
}
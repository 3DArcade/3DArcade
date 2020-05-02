using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Arcade
{
    [ExecuteInEditMode]
    public class ArcadeManager : MonoBehaviour
    {
        public delegate void ShowSelectArcadeConfigurationWindowDelegate();
        public static ShowSelectArcadeConfigurationWindowDelegate ShowSelectArcadeConfigurationWindow;

        // Statics for easy access to the current configuration from everywhere.
        public static OS currentOS;
        public static string applicationPath = Application.streamingAssetsPath; // Set in Awake to a different path for iOS
        public static AvailableModels availableModels = new AvailableModels();
        // ArcadeConfiguration
        public static string arcadesConfigurationPath = "/3darcade~/Configuration/Arcades/";
        public static List<ArcadeConfiguration> arcadesConfigurationList = new List<ArcadeConfiguration>();
        public static ArcadeConfiguration arcadeConfiguration = new ArcadeConfiguration();
        public static ArcadeConfiguration menuConfiguration = new ArcadeConfiguration();
        public static LoadSaveArcadeConfiguration loadSaveArcadeConfiguration;
        // EmulatorConfiguration
        public static string emulatorsConfigurationPath = "/3darcade~/Configuration/Emulators/";
        public static List<EmulatorConfiguration> emulatorsConfigurationList = new List<EmulatorConfiguration>();
        public static LoadSaveEmulatorConfiguration loadSaveEmulatorConfiguration;
        // Reference to all currently loaded external model assets.
        public static List<AssetBundle> modelAssets = new List<AssetBundle>();

        // Zoning
        public static Dictionary<ArcadeType, Dictionary<int, List<int>>> allZones = new Dictionary<ArcadeType, Dictionary<int, List<int>>>();
        public static Dictionary<ArcadeType, Dictionary<int, List<GameObject>>> visibleZones = new Dictionary<ArcadeType, Dictionary<int, List<GameObject>>>();

        // Arcade Control, rename to controls and camera's?
        public static Dictionary<ArcadeType, GameObject> arcadeControls = new Dictionary<ArcadeType, GameObject>();
        public static Dictionary<ArcadeType, Camera> arcadeCameras = new Dictionary<ArcadeType, Camera>();

        // TODO: Move these below to ArcadeStateManager, they are states the fe is in?
        public static ArcadeStates arcadeState = ArcadeStates.LoadingArcade;
        public static ArcadeType activeArcadeType = ArcadeType.FpsArcade;
        public static ArcadeType activeMenuType = ArcadeType.None;
        public static List<string> arcadeHistory = new List<string>();

        // General Configuration TODO: move this to a better place
        [Header("GENERAL CONFIGURATION")]
        public GeneralConfiguration generalConfiguration = new GeneralConfiguration();

        // Arcade Configuration options
        [Space]
        [Header("ARCADE CONFIGURATION")]
        public string descriptiveName = "Default";
        [Tooltip("To add a new Arcade Configuration change the name and id of this Arcade Configuration and then save it.")]
        public string id = "default";
        public ArcadeType arcadeType = ArcadeType.FpsArcade;
        public GameLauncherMethod gameLauncherMethod = GameLauncherMethod.Internal;
        public bool externalModels = true;
        public bool showFPS;
        public ModelSharedProperties modelSharedProperties;
        public List<FpsArcadeProperties> fpsArcadeProperties = new List<FpsArcadeProperties>();
        public List<CylArcadeProperties> cylArcadeProperties = new List<CylArcadeProperties>();
        public List<Zone> zones = new List<Zone>();

        // Arcade Controllers
        public GameObject arcadeControl;
        public GameObject menuControl;

        //Editor options
        private static readonly bool EditorModeShowMainMenuOnPlay = true;
        private static readonly bool EditorModeSaveChangesMadeOnPlay = false;

#if UNITY_EDITOR
#pragma warning disable IDE0051 // Remove unused private members
        [UnityEditor.MenuItem("CONTEXT/ArcadeManager/Load Arcade Configuration")]
        private static void LoadArcadeConfigurationMenuOption()
        {
            ShowSelectArcadeConfigurationWindow?.Invoke();
        }

        [UnityEditor.MenuItem("CONTEXT/ArcadeManager/Save Arcade Configuration")]
        private static void SaveArcadeConfigurationMenuOption()
        {
            loadSaveArcadeConfiguration.SaveArcade();
        }

        //[MenuItem("CONTEXT/ArcadeManager/Set Main Menu Preview Image")]
        //private static void GetArcadePreviewImage(MenuCommand menuCommand)
        //{
        //    var image = Arcade.FileManager.DialogGetFilePart(null, ArcadeManager.applicationPath, FileManager.FilePart.Path_Name_Extension, "jpg,png");
        //    if (image != null)
        //    {
        //        if (ArcadeManager.arcadeState == ArcadeStates.ArcadesConfigurationMenu || Application.isEditor)
        //        {
        //            string destinationFolder = ArcadeManager.applicationPath + ArcadeManager.arcadesConfigurationPath;
        //            string destinationFilename = ArcadeManager.arcadeConfiguration.id + FileManager.getFilePart(FileManager.FilePart.Extension, null, null, image);
        //            FileManager.DeleteFile(destinationFolder, ArcadeManager.arcadeConfiguration.id + ".jpg");
        //            FileManager.DeleteFile(destinationFolder, ArcadeManager.arcadeConfiguration.id + ".png");
        //            FileManager.CopyFile(image, destinationFolder, destinationFilename);
        //        }
        //    }
        //}
#pragma warning restore IDE0051 // Remove unused private members
#endif

        // NB Only use this and the next function to get and set the current arcadeconfiguration.
        // Update the static arcadeConfiguration property with the properties that can be changed in the editor and then return it.
        public ArcadeConfiguration GetArcadeConfiguration()
        {
            arcadeConfiguration.descriptiveName = descriptiveName;
            arcadeConfiguration.id = id;
            arcadeConfiguration.arcadeType = arcadeType.ToString();
            arcadeConfiguration.gameLauncherMethod = gameLauncherMethod.ToString();
            arcadeConfiguration.externalModels = externalModels;
            arcadeConfiguration.showFPS = showFPS;
            arcadeConfiguration.modelSharedProperties = modelSharedProperties;
            arcadeConfiguration.camera = new CameraProperties
            {
                position = arcadeControl.transform.position,
                rotation = arcadeControl.transform.GetChild(0).transform.rotation,
                height = arcadeControl.transform.GetChild(0).transform.localPosition.y
            };
            arcadeConfiguration.fpsArcadeProperties = fpsArcadeProperties;
            arcadeConfiguration.cylArcadeProperties = cylArcadeProperties;
            arcadeConfiguration.zones = zones;
            // Add arcadeconfigurations arcade, game and prop lists when nescessary
            if (arcadeType == ArcadeType.FpsArcade || !Application.isPlaying)
            {
                List<ModelProperties> gameModelList = GetListOfModelProperties(ModelType.Game);
                arcadeConfiguration.gameModelList = gameModelList;
            }
            if (arcadeState != ArcadeStates.MoveCabs)
            {
                List<ModelProperties> arcadeModelList = GetListOfModelProperties(ModelType.Arcade);
                arcadeConfiguration.arcadeModelList = arcadeModelList;
                List<ModelProperties> propModelList = GetListOfModelProperties(ModelType.Prop);
                arcadeConfiguration.propModelList = propModelList;
            }
            return arcadeConfiguration;

            List<ModelProperties> GetListOfModelProperties(ModelType modelType)
            {
                GameObject obj = transform.Find(modelType.ToString() + "Models").gameObject;
                List<ModelProperties> list = new List<ModelProperties>();
                if (obj == null)
                {
                    Debug.Log("no items found");
                    return null;
                }
                int count = obj.transform.childCount;
                for (int i = 0; i < count; i++)
                {
                    GameObject child;
                    ModelSetup gameModelSetup;
                    if (!Application.isPlaying)
                    {
                        child = obj.transform.GetChild(i).gameObject;

                        gameModelSetup = child.GetComponent<ModelSetup>();
                    }
                    else
                    {
                        child = obj.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject;

                        gameModelSetup = obj.transform.GetChild(i).gameObject.GetComponent<ModelSetup>();
                    }

                    // TODO: Null check should not be needed!
                    if (gameModelSetup == null)
                    {
                        continue;
                    }
                    ModelProperties modelProperties = gameModelSetup.GetModelProperties();
                    modelProperties.position = child.transform.position;
                    modelProperties.rotation = child.transform.rotation;
                    modelProperties.scale = child.transform.lossyScale;
                    list.Add(modelProperties);
                }
                return list;
            }
        }

        // Set the static arcadeConfiguration property and update the properties that can be changed in the editor.
        public void SetArcadeConfiguration(ArcadeConfiguration arcadeConfiguration)
        {
            descriptiveName = arcadeConfiguration.descriptiveName;
            id = arcadeConfiguration.id;
            _ = System.Enum.TryParse(arcadeConfiguration.arcadeType, true, out arcadeType);
            _ = System.Enum.TryParse(arcadeConfiguration.gameLauncherMethod, true, out gameLauncherMethod);
            externalModels = arcadeConfiguration.externalModels;
            showFPS = arcadeConfiguration.showFPS;

            // TODO: Transforms setup not here
            arcadeControl.transform.position = arcadeConfiguration.camera.position;
            arcadeControl.transform.rotation = Quaternion.identity;
            arcadeControl.transform.localRotation = Quaternion.identity;
            arcadeControl.transform.GetChild(0).transform.rotation = Quaternion.identity;
            arcadeControl.transform.GetChild(0).transform.localRotation = arcadeConfiguration.camera.rotation;
            arcadeControl.transform.GetChild(0).transform.position = Vector3.zero;
            arcadeControl.transform.GetChild(0).transform.localPosition = new Vector3(0, arcadeConfiguration.camera.height, 0);
            RigidbodyFirstPersonController rigidbodyFirstPersonController = arcadeControl.GetComponent<RigidbodyFirstPersonController>();
            if (rigidbodyFirstPersonController != null)
            {
                rigidbodyFirstPersonController.Setup();
            }
            fpsArcadeProperties = arcadeConfiguration.fpsArcadeProperties;
            cylArcadeProperties = arcadeConfiguration.cylArcadeProperties;
            modelSharedProperties = arcadeConfiguration.modelSharedProperties;
            zones = arcadeConfiguration.zones;
            UnityEngine.RenderSettings.ambientIntensity = modelSharedProperties.renderSettings.ambientIntensity; // TODO: not here

            ArcadeManager.arcadeConfiguration = arcadeConfiguration;
        }

        private void Awake()
        {
            // Setup statics for easy access to the current arcade configuaration and importent gameObjects. Use singleton instead? I do it with swift why not here?

            loadSaveArcadeConfiguration = new LoadSaveArcadeConfiguration(this); // Is there no better way to get a refrence to our ArcadeManager class from the LoadSaveArcade class?
            loadSaveEmulatorConfiguration = new LoadSaveEmulatorConfiguration();

            generalConfiguration = FileManager.LoadJSONData<GeneralConfiguration>(Path.Combine(applicationPath + "/3darcade~/Configuration/GeneralConfiguration.json"));
            if (generalConfiguration == null)
            {
                generalConfiguration = FileManager.LoadJSONData<GeneralConfiguration>(Path.Combine(Application.dataPath + "/Resources/cfg/GeneralConfiguration.json"));
            }

            arcadeControls[ArcadeType.FpsArcade] = arcadeControl;
            arcadeControls[ArcadeType.CylArcade] = arcadeControl;
            arcadeControls[ArcadeType.FpsMenu] = menuControl;
            arcadeControls[ArcadeType.CylMenu] = menuControl;
            arcadeCameras[ArcadeType.FpsArcade] = arcadeControl.GetComponentInChildren<Camera>();
            arcadeCameras[ArcadeType.CylArcade] = arcadeControl.GetComponentInChildren<Camera>();
            arcadeCameras[ArcadeType.FpsMenu] = menuControl.GetComponentInChildren<Camera>();
            arcadeCameras[ArcadeType.CylMenu] = menuControl.GetComponentInChildren<Camera>();

            if (Application.platform.ToString().Contains("OSX"))
            {
                currentOS = OS.MacOS;
            }
            if (Application.platform.ToString().Contains("Windows"))
            {
                currentOS = OS.Windows;
            }
            if (Application.platform.ToString().Contains("Linux"))
            {
                currentOS = OS.Linux;
            }
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                currentOS = OS.iOS;
                applicationPath = Application.dataPath + "/Raw";
            }
            if (Application.platform == RuntimePlatform.tvOS)
            {
                currentOS = OS.tvOS;
            }
            print("Current OS = " + currentOS.ToString());
#if UNITY_EDITOR
            availableModels.game = FileManager.GetListOfAssetNames(ModelType.Game.ToString(), true)
                                              .Concat(FileManager.GetListOfAssetNames(ModelType.Game.ToString(), false))
                                              .Distinct()
                                              .OrderBy(x => x)
                                              .ToArray();
            FileManager.SaveJSONData(availableModels, Path.Combine(applicationPath, "3darcade~/Configuration"), "AvailableModels.json");
            // FileManager
#else
            availableModels = FileManager.LoadJSONData<AvailableModels>(Path.Combine(ArcadeManager.applicationPath, "3darcade~/Configuration/AvailableModels.json"));
#endif
            print("Available Game models " + availableModels.game.Length);
            // TODO: Arcade Configurations and Emulator Configurations can be large, preloading is faster but can take a large amount of memory... Change to Skurdt's database approach!
            loadSaveEmulatorConfiguration.LoadEmulators();
            loadSaveArcadeConfiguration.LoadArcadesConfigurationList();

            //Clean up Libretro
            FileManager.DeleteFiles(applicationPath + "/3darcade~/Libretro/temp/");

            print("Start State = " + arcadeState);

            //  string[] xy = typeof(ModelProperties).GetProperties().Select(x => x.Name).ToList().ToArray();

        }

#if UNITY_EDITOR
        void Update()
        {
            if (loadSaveArcadeConfiguration == null)
            {
                print("Saving a script caused a reload, calling Awake() in ArcadeManager to initialize some default properties again.");
                Awake();
            }
        }
#endif

        private void Start()
        {
#if UNITY_EDITOR

            // When we return from play mode Start() is called again, we use that to reset and load our arcade configuration. We need to do this because we loose our script references when we exit play mode. That will cause lots of havoc, if you then try to save an arcadeconfiguration.
            if (!Application.isPlaying)
            {
                loadSaveArcadeConfiguration.ResetArcade();
                loadSaveArcadeConfiguration.LoadArcadesConfigurationList();
                List<ArcadeConfiguration> arcadeConfigurations = arcadesConfigurationList.Where(x => x.id == id).ToList();
                if (arcadeConfigurations.Count > 0)
                {
                    arcadeConfiguration = arcadeConfigurations[0];
                    bool success = loadSaveArcadeConfiguration.LoadArcade(arcadeConfiguration);
                    print("Loaded arcade configuration " + arcadeConfiguration.descriptiveName + " successfully is " + success);
                }
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                return;
            }

            // Show the main menu
            if (EditorModeShowMainMenuOnPlay)
            {
                loadSaveArcadeConfiguration.ResetArcade();
                if (!ShowMainMenu())
                {
                    Debug.Log("Main Menu could not be loaded");
                    // TODO: Show an error dialog
                }
            }
            else
            {
                arcadeConfiguration = GetArcadeConfiguration();
                loadSaveArcadeConfiguration.ResetArcade();
                loadSaveArcadeConfiguration.StartArcade(arcadeConfiguration);
            }
#else

            // Show the main menu
            loadSaveArcadeConfiguration.ResetArcade();
            if (!ShowMainMenu())
            {
                Debug.Log("Main Menu could not be loaded");
                // TODO: Show an error dialog
            }
#endif
            bool ShowMainMenu()
            {
                if (StartArcadeWith(generalConfiguration.mainMenuArcadeConfiguration))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                Debug.Log("Paused");
                if (EditorModeSaveChangesMadeOnPlay && EditorModeShowMainMenuOnPlay)
                {
                    loadSaveArcadeConfiguration.SaveArcade();
                    loadSaveArcadeConfiguration.ResetArcade();
                    _ = loadSaveArcadeConfiguration.LoadArcade(arcadeConfiguration);
                }
            }
        }

        private void OnApplicationQuit()
        {
            Debug.Log("Quited!");
        }

        //void OnDestroy()
        //{
        //}

        // Load arcadeconfigurations from file and then show them in the main menu.

        public static bool StartArcadeWith(string arcadeConfigurationID)
        {
            ArcadeConfiguration arcadeConfiguration = loadSaveArcadeConfiguration.GetArcadeConfigurationByID(arcadeConfigurationID);
            if (arcadeConfiguration != null)
            {
                loadSaveArcadeConfiguration.StartArcade(arcadeConfiguration);
                return true;
            }
            return false;
        }
    }


}

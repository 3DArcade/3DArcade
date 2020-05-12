using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Arcade
{
    [SelectionBase]
    public class ModelSetup : MonoBehaviour
    {
        #region EditorUI
#pragma warning disable IDE0051 // Remove unused private members
        public delegate void ShowSelectEmulatorWindowDelegate(GameObject obj);
        public static ShowSelectEmulatorWindowDelegate ShowSelectEmulatorWindow;
        public delegate void ShowSelectModelWindowDelegate(GameObject obj);
        public static ShowSelectModelWindowDelegate ShowSelectModelWindow;
        public delegate void ShowBuildAssetBundleWindowDelegate(GameObject obj, string assetPath, string assetName);
        public static ShowBuildAssetBundleWindowDelegate ShowBuildAssetBundleWindow;
        public delegate void ShowSelectGameWindowDelegate(GameObject obj, List<ModelProperties> gameList);
        public static ShowSelectGameWindowDelegate ShowSelectGameWindow;

        private void GetGame()
        {
            EmulatorConfiguration emulatorConfiguration = ArcadeManager.loadSaveEmulatorConfiguration.GetEmulatorConfiguration(emulator);
            if (emulatorConfiguration != null)
            {
                List<ModelProperties> gamelist = emulatorConfiguration.masterGamelist;
                ShowSelectGameWindow?.Invoke(gameObject, gamelist);
            }
        }

        private void GetEmulator()
        {
            ShowSelectEmulatorWindow?.Invoke(gameObject);
        }

        private void GetModel()
        {
            ShowSelectModelWindow?.Invoke(gameObject);
        }
#pragma warning restore IDE0051 // Remove unused private members
        #endregion

        [ContextMenuItem("Select a Game from Master Gamelist", "GetGame")]
        public string descriptiveName;
        [Arcade("Game")]
        public string id;
        public string idParent;
        [Arcade("Emulator"), ContextMenuItem("Select an Emulator Configuration", "GetEmulator")]
        public string emulator;
        [Arcade("Model"), ContextMenuItem("Select Model", "GetModel")]
        public string model;
        public bool grabbable = false;
        public float animatedTextureSpeed = 2.0f;
        public string screen;
        public string manufacturer;
        public string year;
        public string genre;
        public bool mature;
        public bool available;
        public bool runnable;
        public GameLauncherMethod gameLauncherMethod = GameLauncherMethod.None;
        public int playCount;
        public int zone;
        public List<Trigger> triggers;
        public List<string> triggerIDs;

        [HideInInspector] public bool isSelected = false;
        [HideInInspector] public bool isPlaying = false;
        [HideInInspector] public ModelSharedProperties modelSharedProperties;

        //private List<GameObject> children;
        //private new Renderer renderer;
        private BoxCollider boxCol;
        private Rigidbody rigid;
        //private Texture2D tempTexture = null;

        public AnimationCurve volumeCurve;

        private void Awake()
        {
            volumeCurve = new AnimationCurve(new Keyframe[]
            {
                new Keyframe(0.8f, 1.0f, -2.6966875f,  -2.6966875f,  0.2f,        0.10490462f),
                new Keyframe(1.5f, 0.3f, -0.49866775f, -0.49866775f, 0.28727788f, 0.2f),
                new Keyframe(3.0f, 0.0f, -0.08717632f, -0.08717632f, 0.5031141f,  0.2f)
            });
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if ((id == null && emulator == null) || (id == "" && emulator == ""))
            {
                string thisName = gameObject.name;
                int index = thisName.IndexOf("(", System.StringComparison.Ordinal);
                if (index > 0)
                {
                    thisName = thisName.Substring(0, index);
                }
                SetupModelPropertiesFromEmulatorMasterGameList(thisName, "mame");
            }
            //  SetupModel();
        }

        //void OnValidate()
        //{
        //    if (updateModelPropertiesFromMasterGamelist)
        //    {
        //        updateModelPropertiesFromMasterGamelist = false;
        //        SetupPropertiesFromEmulatorMasterGameList(id, emulator);
        //    }
        //}

        [MenuItem("CONTEXT/ModelSetup/Update from MasterGamelist")]
        private static void MenuOptionUpdateFromMasterGamelist(MenuCommand menuCommand)
        {
            ModelSetup modelSetup = menuCommand.context as ModelSetup;
            string model = modelSetup.id.Trim();
            if (model == "")
            {
                model = modelSetup.transform.gameObject.name;
                model = model.Substring(0, model.IndexOf("(", System.StringComparison.Ordinal));
            }
            modelSetup.SetupModelPropertiesFromEmulatorMasterGameList(model, modelSetup.emulator);
        }

        [MenuItem("CONTEXT/ModelSetup/Build Prefab(s)")]
        private static void MenuOptionGetAssetPath(MenuCommand menuCommand)
        {
            ModelSetup modelSetup = menuCommand.context as ModelSetup;
            GameObject obj = modelSetup.transform.gameObject;

            string assetPath = Application.dataPath.Replace("Assets", "") + AssetDatabase.GetAssetPath(obj);
            string assetName = FileManager.GetFilePart(FileManager.FilePart.Name, null, null, assetPath);
            assetPath = FileManager.GetFilePart(FileManager.FilePart.Path, null, null, assetPath);
            print(assetPath + assetName + ".prefab");
            ShowBuildAssetBundleWindow?.Invoke(obj, assetPath, assetName);
        }
        [MenuItem("CONTEXT/ModelSetup/Build Prefab(s)", true)]
        private static bool MenuOptionGetAssetPathValidation(MenuCommand menuCommand)
        {
            ModelSetup modelSetup = menuCommand.context as ModelSetup;
            GameObject obj = modelSetup.transform.gameObject;
            return AssetDatabase.GetAssetPath(obj).Contains("Assets/Resources") ? true : false;
            // modelSetup.SetupPropertiesFromEmulatorMasterGameList(modelSetup.id, modelSetup.emulator);
        }
#endif

        public void SetModelProperties(ModelProperties modelProperties)
        {
            descriptiveName = modelProperties.descriptiveName;
            id = modelProperties.id;
            idParent = modelProperties.idParent;
            emulator = modelProperties.emulator;
            model = modelProperties.model;
            //System.Enum.TryParse(modelProperties.animationType, true, out animationType);
            grabbable = modelProperties.grabbable;
            //lightmap = modelProperties.lightmap;
            //animatedTextureSequence = modelProperties.animatedTextureSequence;
            animatedTextureSpeed = modelProperties.animatedTextureSpeed;

            //attachToCamera = modelProperties.attachToCamera; // TODO: Move to TriggerEvent System? setParent to, set Transform
            //hideWhenArcadeIsActive = modelProperties.hideWhenArcadeIsActive;
            //receiveSelectedModelArtWork = modelProperties.receiveSelectedModelArtWork; //  TODO: Move to TriggerEvent System?
            //receiveActiveMenuRenderTexture = modelProperties.receiveActiveMenuRenderTexture; // TODO: Move to TriggerEvent System?

            screen = modelProperties.screen;
            manufacturer = modelProperties.manufacturer;
            year = modelProperties.year;
            genre = modelProperties.genre;
            mature = modelProperties.mature;
            available = modelProperties.available;
            runnable = modelProperties.runnable;
            _ = System.Enum.TryParse(modelProperties.gameLauncherMethod, true, out gameLauncherMethod);
            playCount = modelProperties.playCount;
            //audioProperties = modelProperties.audioProperties;
            zone = modelProperties.zone;
            triggers = modelProperties.triggers;
            //print("trigger " + triggers.Count);
            //if (triggers.Count > 0)
            //{
            //    print("animation " + triggers[0].animationProperties.name);
            //}
            triggerIDs = modelProperties.triggerIDs;

        }

        public ModelProperties GetModelProperties()
        {
            return new ModelProperties
            {
                descriptiveName = descriptiveName,
                id = id,
                idParent = idParent,
                emulator = emulator,
                model = model,
                //modelProperties.animationType = animationType.ToString();
                grabbable = grabbable,
                //modelProperties.lightmap = lightmap;
                //modelProperties.animatedTextureSequence = animatedTextureSequence;
                animatedTextureSpeed = animatedTextureSpeed,

                //modelProperties.attachToCamera = attachToCamera; // TODO: Move to TriggerEvent System? setParent to, set Transform
                //modelProperties.hideWhenArcadeIsActive = hideWhenArcadeIsActive;
                //modelProperties.receiveSelectedModelArtWork = receiveSelectedModelArtWork; //  TODO: Move to TriggerEvent System?
                //modelProperties.receiveActiveMenuRenderTexture = receiveActiveMenuRenderTexture; // TODO: Move to TriggerEvent System?

                screen = screen,
                manufacturer = manufacturer,
                year = year,
                genre = genre,
                mature = mature,
                available = available,
                runnable = runnable,
                gameLauncherMethod = gameLauncherMethod.ToString(),
                playCount = playCount,
                //modelProperties.audioProperties = audioProperties;
                zone = zone,
                triggers = triggers,
                triggerIDs = triggerIDs
            };
        }

        public void Setup(ModelProperties modelProperties, ModelSharedProperties modelSharedProperties)
        {
            this.modelSharedProperties = modelSharedProperties;
            SetModelProperties(modelProperties);

            List<GameObject> thisChildren = new List<GameObject>();
            if (!Application.isPlaying)
            {
                foreach (Transform child in transform)
                {
                    if (child.gameObject != null)
                    {
                        thisChildren.Add(child.gameObject);
                    }
                }
            }
            else
            {
                thisChildren = new List<GameObject>();
                transform.GetChild(0).transform.tag = transform.tag;
                foreach (Transform child in transform.GetChild(0).transform)
                {
                    if (child.gameObject != null)
                    {
                        thisChildren.Add(child.gameObject);
                    }
                }
            }

            EmulatorConfiguration[] emulatorList = ArcadeManager.emulatorsConfigurationList.Where(x => x.emulator.id == emulator).ToArray();
            if (emulatorList.Length == 0)
            {
                return;
            }

            EmulatorProperties emulatorSelected = emulatorList[0].emulator;
            if (emulatorSelected == null)
            {
                return;
            }

            // TODO: setup some default paths for marquees, screenshots and generics.

            // Marquee setup
            if (thisChildren.Count > 0)
            {
                // image and image cylcing (cycling with id_*.ext)
                List<Texture2D> textureList = FileManager.LoadImagesFromFolder(ArcadeManager.applicationPath + FileManager.CorrectFilePath(emulatorSelected.marqueePath), id);
                if (textureList.Count < 1)
                {
                    textureList = FileManager.LoadImagesFromFolder(ArcadeManager.applicationPath + FileManager.CorrectFilePath(emulatorSelected.marqueePath), idParent);
                }

                // video
                string url = FileManager.GetVideoPathFromFolder(ArcadeManager.applicationPath + FileManager.CorrectFilePath(emulatorSelected.marqueePath), id);
                if (url == null)
                {
                    url = FileManager.GetVideoPathFromFolder(ArcadeManager.applicationPath + FileManager.CorrectFilePath(emulatorSelected.marqueePath), idParent);
                }

                // if we only have one or zero images we dont have to setup videoplayer/image cycling
                if (url == null && textureList.Count <= 1)
                {
                    ModelImageSetup marquee = thisChildren[0].GetComponent<ModelImageSetup>();
                    if (marquee == null)
                    {
                        marquee = thisChildren[0].AddComponent<ModelImageSetup>();
                    }

                    Texture2D tex = textureList.Count > 0 ? textureList[0] : null;
                    marquee.Setup(tex, modelSharedProperties.renderSettings, modelProperties, (gameObject.CompareTag("gamemodel") || gameObject.CompareTag("propmodel") ? ModelComponentType.Marquee : ModelComponentType.Generic));
                }
                else
                {
                    SetupVideo(thisChildren[0], textureList, url, (gameObject.CompareTag("gamemodel") || gameObject.CompareTag("propmodel") ? ModelComponentType.Marquee : ModelComponentType.Generic), modelSharedProperties);
                }
            }

            // Screen setup
            if (thisChildren.Count > 1)
            {
                // image and image cycling (cycling with id_*.ext)
                List<Texture2D> textureList = FileManager.LoadImagesFromFolder(ArcadeManager.applicationPath + FileManager.CorrectFilePath(emulatorSelected.screenPath), id);
                if (textureList.Count < 1)
                {
                    textureList = FileManager.LoadImagesFromFolder(ArcadeManager.applicationPath + FileManager.CorrectFilePath(emulatorSelected.screenPath), idParent);
                }

                if (textureList.Count <= 1)
                {
                    List<Texture2D> tList = FileManager.LoadImagesFromFolder(ArcadeManager.applicationPath + FileManager.CorrectFilePath(emulatorSelected.titlePath), id);
                    if (tList.Count < 1)
                    {
                        tList = FileManager.LoadImagesFromFolder(ArcadeManager.applicationPath + FileManager.CorrectFilePath(emulatorSelected.titlePath), idParent);
                    }
                    if (tList.Count > 0)
                    {
                        textureList.AddRange(tList);
                    }

                }

                // video
                string url = FileManager.GetVideoPathFromFolder(ArcadeManager.applicationPath + FileManager.CorrectFilePath(emulatorSelected.screenPath), id);
                if (url == null)
                {
                    url = FileManager.GetVideoPathFromFolder(ArcadeManager.applicationPath + FileManager.CorrectFilePath(emulatorSelected.screenPath), idParent);
                }
                if (url == null)
                {
                    url = FileManager.GetVideoPathFromFolder(ArcadeManager.applicationPath + FileManager.CorrectFilePath(emulatorSelected.screenVideoPath), id);
                }
                if (url == null)
                {
                    url = FileManager.GetVideoPathFromFolder(ArcadeManager.applicationPath + FileManager.CorrectFilePath(emulatorSelected.screenVideoPath), idParent);
                }
                SetupVideo(thisChildren[1], textureList, url, ModelComponentType.Screen, modelSharedProperties);
            }

            // Generic Texture setup
            if (thisChildren.Count > 2)
            {
                // image and image cycling (cycling with id_*.ext)
                List<Texture2D> textureList = FileManager.LoadImagesFromFolder(ArcadeManager.applicationPath + FileManager.CorrectFilePath(emulatorSelected.genericPath), id);
                if (textureList.Count < 1)
                {
                    textureList = FileManager.LoadImagesFromFolder(ArcadeManager.applicationPath + FileManager.CorrectFilePath(emulatorSelected.genericPath), idParent);
                }

                // video
                string url = FileManager.GetVideoPathFromFolder(ArcadeManager.applicationPath + FileManager.CorrectFilePath(emulatorSelected.genericPath), id);
                if (url == null)
                {
                    url = FileManager.GetVideoPathFromFolder(ArcadeManager.applicationPath + FileManager.CorrectFilePath(emulatorSelected.genericPath), idParent);
                }

                // if we only have one or zero images we dont have to setup videoplayer/image cycling
                if (url == null && textureList.Count == 1)
                {
                    ModelImageSetup generic = thisChildren[2].GetComponent<ModelImageSetup>();
                    if (generic == null)
                    {
                        generic = thisChildren[2].AddComponent<ModelImageSetup>();
                    }
                    Texture2D tex = textureList[0];
                    generic.Setup(tex, modelSharedProperties.renderSettings, modelProperties, ModelComponentType.Generic);
                }
                else if (url != null || textureList.Count > 1)
                {
                    SetupVideo(thisChildren[2], textureList, url, ModelComponentType.Generic, modelSharedProperties);
                }
            }
        }

        private void SetupVideo(GameObject child, List<Texture2D> textureList, string url, ModelComponentType modelComponentType, ModelSharedProperties modelSharedProperties)
        {
            ModelVideoSetup modelVideoSetup = child.GetComponent<ModelVideoSetup>();
            if (modelVideoSetup == null)
            {
                modelVideoSetup = child.AddComponent<ModelVideoSetup>();
            }

            UnityEngine.Video.VideoPlayer video = child.GetComponent<UnityEngine.Video.VideoPlayer>();
            AudioSource taudio = child.GetComponent<AudioSource>();
            if (taudio == null && modelSharedProperties.spatialSound == true)
            {
                if (url != null)
                {
                    taudio = child.AddComponent<AudioSource>();
                    taudio.playOnAwake = false;
                    taudio.spatialBlend = 1f;
                    taudio.spatialize = true;
                    taudio.minDistance = 0f;
                    taudio.maxDistance = 3f;
                    taudio.enabled = false;
                    taudio.rolloffMode = AudioRolloffMode.Custom;
                    taudio.SetCustomCurve(AudioSourceCurveType.CustomRolloff, volumeCurve);
                }
            }

            if (video == null)
            {
                if (url != null)
                {
                    video = child.AddComponent<UnityEngine.Video.VideoPlayer>();
                }
            }
            else
            {
                if (url == null)
                {
                    if (Application.isPlaying)
                    {
                        Destroy(video);
                    }
                    else
                    {
                        DestroyImmediate(video);
                    }
                }
            }

            if (video != null)
            {
                video.Pause();
                if (modelSharedProperties.spatialSound)
                {
                    video.audioOutputMode = UnityEngine.Video.VideoAudioOutputMode.AudioSource;
                    video.SetTargetAudioSource(0, taudio);
                }
                video.enabled = false;
            }

            if (modelVideoSetup != null)
            {
                modelVideoSetup.Setup(textureList, url, animatedTextureSpeed, modelComponentType, GetModelProperties(), modelSharedProperties);
            }
        }

        private void SetupModel()
        {
            // Add rigidbody to gameObject
            if (rigid == null)
            {
                rigid = GetComponent<Rigidbody>();
                if (rigid == null)
                {
                    rigid = gameObject.AddComponent<Rigidbody>();
                    rigid.mass = 20;
                }
            }

            if (GetComponent<MeshCollider>() != null || GetComponentInChildren<MeshCollider>() != null || GetComponentInChildren<BoxCollider>() != null)
            {
                return;
            }

            boxCol = gameObject.GetComponent<BoxCollider>();
            if (boxCol == null)
            {
                boxCol = gameObject.AddComponent<BoxCollider>();
            }

            transform.position = new Vector3(0, 0, 0);
            Renderer[] rr = GetComponentsInChildren<Renderer>();
            Bounds b = rr[0].bounds;
            foreach (Renderer r in rr)
            {
                b.Encapsulate(r.bounds);
            }
            boxCol.center = new Vector3(0, b.size.y / 2, 0);
            boxCol.size = new Vector3(b.size.x, b.size.y, b.size.z);
        }

        public void SetupModelPropertiesFromEmulatorMasterGameList(string nameID, string emulatorID)
        {
            if (ArcadeManager.emulatorsConfigurationList.Count < 1)
            {
                _ = ArcadeManager.loadSaveEmulatorConfiguration.LoadEmulatorsConfigurationList();
            }

            List<EmulatorConfiguration> emulatorConfiguration = ArcadeManager.emulatorsConfigurationList.Where(x => x.emulator.id == emulatorID).ToList();
            if (emulatorConfiguration.Count < 1)
            {
                return;
            }

            List<ModelProperties> modelList = emulatorConfiguration[0].masterGamelist.Where(x => x.id.ToLower() == nameID.ToLower()).ToList();
            if (modelList.Count < 1)
            {
                return;
            }

            ModelProperties modelProperties = modelList[0];
            SetModelProperties(modelProperties);
        }
    }
}

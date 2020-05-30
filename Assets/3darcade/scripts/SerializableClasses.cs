using System.Collections.Generic;
using UnityEngine;

namespace Arcade
{
    [System.Serializable]
    public class CameraProperties
    {
        public Vector3 position;
        public Quaternion rotation;
        public float height = 2.5f;
        public float aspectRatio = 0f; // 0 = use Unity's default setting, screen's aspect ratio. CylArcadeProperties can override this setting.
        public bool orthographic = false;
        public float fieldOfView = 60f;
        public float nearClipPlane = 0.1f;
        public float farClipPlane = 100f;
        public Rect viewportRect = new Rect(0, 0, 1, 1);
        public bool allowDynamicResolution = false;
    }

    [System.Serializable]
    public class RenderSettings
    {
        public float ambientIntensity = 1.0f;
        public float marqueeIntensity = 1.6f;
        public float screenRasterIntensity = 2.4f;
        public float screenVectorIntenstity = 6f;
        public float screenPinballIntensity = 1f;
    }

    [System.Serializable]
    public class ModelSharedProperties
    {
        public bool spatialSound = false;
        [ArcadePopUp(typeof(ModelVideoEnabled))]
        public string videoOnModelEnabled = ModelVideoEnabled.VisibleUnobstructed.ToString();
        public RenderSettings renderSettings = new RenderSettings();
    }

    [System.Serializable]
    public class FpsArcadeProperties
    {
        // FpsArcade specific properties
    }

    [System.Serializable]
    public class CylArcadeProperties
    {
        // CylArcade specific properties
        public int gamePosition = 0; // first game is 0
        public int sprockets = 18;
        public int selectedSprocket = 8; // first sprocket = 0
        public int radius = 5;
        public float modelSpacing = 0.25f;
        public bool modelWidthAxisY;
        public bool modelIsKinemtatic = true;
        public bool cylArcadeOnScreenSelectedModel = true;

        public float cameraAspectRatio = 0f; // 0 = use Unity's default setting, screen's aspect ratio. Overrides the CameraProperties aspectRatio Setting.
        public float cameraTranslation = 0f;
        public float cameraMinTranslation = -0.7f;
        public float cameraMaxTranslation = 0.7f;
        public bool cameraTranslationDirectionAxisY;
        public bool cameraTranslationReverse;
        public float cameraTranslationdamping = 4f;

        public float cameraRotationdamping = 8f;
        public float cameraLocalEularAngleX = 0;
        public float cameraLocalEularAngleY = 180;
        public float cameraLocalEularAngleZ = 0;

        public bool cameraLocalEularAngleRotation = true;
        public float cameraLocalDefaultEularAngleRotation = 0f;
        public float cameraLocalMinEularAngleRotation = -15f;
        public float cameraLocalMaxEularAngleRotation = 15f;
        public bool mouseLook = true;
        public bool mouseLookAxisY = true;
        public bool mouseLookReverse = true;

        public Vector3 cameraLocalTranslation = new Vector3(0, 1.6f, 0);

        public float sprocketLocalEularAngleX = 0;
        public float sprocketLocalEularAngleY = 180;
        public float sprocketLocalEularAngleZ = 0;
    }

    [System.Serializable]
    public class Zone
    {
        public int zone;
        public List<int> visibleZones;
    }

    [System.Serializable]
    public class AnimationProperties
    {
        public string name = "";
        public bool loop;
        public float speed;
        [ArcadePopUp(typeof(PlayMode))]
        public string playMode = PlayMode.StopSameLayer.ToString();
        public int layer = 0;
    }

    [System.Serializable]
    public class AudioProperties
    {
        public string name = "";
        public int priority = 128;
        public float volume = 1;
        public bool loop;
        public bool playOnAwake;
        public bool Randomize;
        public float spatialBlend = 1;
        public bool spatialize = true;
        public float minDistance = 4;
        public float maxDistance = 200;
        public List<AudioFile> audioFiles;
        public override bool Equals(object obj)
        {
            AudioProperties other = obj as AudioProperties;
            if (other == null)
            {
                return false;
            }
            if (name != other.name)
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return 363513814 + EqualityComparer<string>.Default.GetHashCode(name);
        }

        public static bool operator ==(AudioProperties left, AudioProperties right)
        {
            return EqualityComparer<AudioProperties>.Default.Equals(left, right);
        }

        public static bool operator !=(AudioProperties left, AudioProperties right)
        {
            return !(left == right);
        }
    }

    [System.Serializable]
    public class TriggerTransformProperties
    {
        public bool setParent = false;
        public bool setIsKinematic = false;
        public bool setPosition = false;
        public bool setRotation = false;
        public bool setScale = false;
        public bool setIsActive = false;

        public string parent = "";
        public bool isKinematic = false;
        public Vector3 position = new Vector3(0f, 0f, 0f);
        public Quaternion rotation;
        public Vector3 scale = new Vector3(1f, 1f, 1f);
        public bool isActive = true;
    }

    [System.Serializable]
    public class Trigger
    {
        [ArcadePopUp(typeof(TriggerEvent))]
        public string triggerEvent = "";
        [Tooltip("Use self for this object and camera for the active camera.")]
        public List<string> triggerSource; // special values: self, camera
        [Tooltip("Use self for this object and camera for the active camera.")]
        public List<string> triggerTarget; // special values: self, camera
        [ArcadePopUp(typeof(TriggerAction))]
        public string triggerAction = "";
        public List<AnimationProperties> animationProperties;
        public List<AudioProperties> audioProperties;
        public List<TriggerTransformProperties> triggerTansformProperties;
    }

    [System.Serializable]
    public class ModelProperties
    {
        public string descriptiveName;
        public string id;
        public string idParent;
        public string emulator;
        public string model;
        public bool grabbable; // not implemented
        //public bool lightmap; // not implemented, probably unnescessary
        //public bool animatedTextureSequence; // not used anymore
        public float animatedTextureSpeed = 2.0f;
        public Vector3 position = new Vector3(0f, 0f, 0f);
        public Quaternion rotation;
        public Vector3 scale = new Vector3(1f, 1f, 1f);
        public string screen;
        public string manufacturer;
        public string year;
        public string genre;
        public bool mature;
        public bool runnable; // TODO: implement
        public bool available; // partially implemented
        public string gameLauncherMethod;
        public int playCount;
        public int zone;
        public List<Trigger> triggers;
        public List<string> triggerIDs;
    }

    [System.Serializable]
    public class ArcadeConfiguration
    {
        public string descriptiveName = "";
        public string id = "";
        public string arcadeType = ArcadeType.FpsArcade.ToString();
        public string gameLauncherMethod;
        public bool externalModels = true;
        public bool showFPS;
        public CameraProperties camera;
        public ModelSharedProperties modelSharedProperties = new ModelSharedProperties();
        public List<FpsArcadeProperties> fpsArcadeProperties = new List<FpsArcadeProperties>();
        public List<CylArcadeProperties> cylArcadeProperties = new List<CylArcadeProperties>();

        public List<Zone> zones = new List<Zone>();
        public List<ModelProperties> gameModelList = new List<ModelProperties>();
        public List<ModelProperties> arcadeModelList = new List<ModelProperties>();
        public List<ModelProperties> propModelList = new List<ModelProperties>();
    }

    [System.Serializable]
    public class AvailableModels
    {
        public string[] arcade;
        public string[] game;
        public string[] prop;
    }

    [System.Serializable]
    public class AudioFile
    {
        public string file = "";
        public string path = "";
    }

    [System.Serializable]
    public class ModelFilter
    {
        // add all props to choose from
        // [ArcadePopUp(typeof(ModelProperties))]
        [ArcadePopUp(typeof(ModelProperties))]
        public string modelProperty = "";
        public string modelPropertyValue = "";
        [ArcadePopUp(typeof(ModelFilterOperator))]
        public string modelFilterOperator = "";
    }

    [System.Serializable]
    public class DefaultModelFilter
    {
        public List<ModelFilter> modelFilters;
        public string model = "";
    }

    [System.Serializable]
    public class EmulatorProperties
    {
        public string descriptiveName = "";
        public string id = "";
        public string about = "";
        public string executable = "";
        public string extension = "";
        public string libretroCore = "";
        public string arguments = "";
        public string options = "";
        public string emulatorPath = "";
        public string gamePath = "";
        public string workingDir = "";
        public string marqueePath = "";
        public string screenPath = "";
        public string screenVideoPath = "";
        public string genericPath = "";
        public string titlePath = "";
        public string infoPath = "";
        public string gameLauncherMethod = GameLauncherMethod.External.ToString();
        public bool outputCommandLine;
        public List<DefaultModelFilter> defaultModelFilters;
    }

    [System.Serializable]
    public class EmulatorConfiguration
    {
        public EmulatorProperties emulator;
        public string lastMasterGamelistUpdate = "";
        public string md5MasterGamelist = "";
        public List<ModelProperties> masterGamelist = new List<ModelProperties>();
    }

    [System.Serializable]
    public class MasterGamelistConfiguration
    {
        public string descriptiveName = "";
        public string id = "";
        public string info = "";
        public string lastUpdate = "";
        public List<ModelProperties> masterGamelist;
    }

    [System.Serializable]
    public class GeneralConfiguration
    {
        [ArcadePopUp("Arcade")] // arcadeConfigurationList
        public string mainMenuArcadeConfiguration = "mainmenu";
        public List<DefaultModelFilter> defaultModelFilters;
    }

    public class TriggerWrapper
    {
        public List<GameObject> triggerSourceGameObjects;
        public List<GameObject> triggerTargetGameObjects;
        public Trigger trigger;
    }
}

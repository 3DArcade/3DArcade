using System.Collections.Generic;
using UnityEngine;

namespace Arcade
{
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
    public class AvailableModels
    {
        public string[] arcade;
        public string[] game;
        public string[] prop;
    }
}

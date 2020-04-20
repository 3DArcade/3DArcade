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
}

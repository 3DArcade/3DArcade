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

using System.Collections.Generic;
using UnityEngine;

namespace Arcade_r
{
    [System.Serializable]
    public sealed class RenderSettings
    {
        public float AmbientIntensity;
        public float MarqueeIntensity;
        public float ScreenRasterIntensity;
        public float ScreenVectorIntenstity;
        public float ScreenPinballIntensity;
    }

    [System.Serializable]
    public sealed class AudioSettings
    {
        public bool SpatialSound;
    }

    [System.Serializable]
    public sealed class VideoSettings
    {
        public string VideoOnModelEnabled;
    }

    [System.Serializable]
    public sealed class CameraSettings
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public float Height;
        public bool Orthographic;
        public float FieldOfView;
        public float AspectRatio;
        public float NearClipPlane;
        public float FarClipPlane;
        public Rect ViewportRect;
    }

    [System.Serializable]
    public sealed class FpsArcadeProperties
    {
    }

    [System.Serializable]
    public sealed class CylArcadeProperties
    {
        public int GamePosition;
        public int Sprockets;
        public int SelectedSprocket;
        public int Radius;
        public float ModelSpacing;
        public bool ModelWidthAxisY;
        public bool ModelIsKinemtatic;
        public bool CylArcadeOnScreenSelectedModel;

        public float CameraAspectRatio;
        public float CameraTranslation;
        public float CameraMinTranslation;
        public float CameraMaxTranslation;
        public bool CameraTranslationDirectionAxisY;
        public bool CameraTranslationReverse;
        public float CameraTranslationdamping;

        public float CameraRotationdamping;
        public float CameraLocalEularAngleX;
        public float CameraLocalEularAngleY;
        public float CameraLocalEularAngleZ;

        public bool CameraLocalEularAngleRotation;
        public float CameraLocalDefaultEularAngleRotation;
        public float CameraLocalMinEularAngleRotation;
        public float CameraLocalMaxEularAngleRotation;
        public bool MouseLook;
        public bool MouseLookAxisY;
        public bool MouseLookReverse;

        public Vector3 CameraLocalTranslation;

        public float SprocketLocalEularAngleX;
        public float SprocketLocalEularAngleY;
        public float SprocketLocalEularAngleZ;
    }

    [System.Serializable]
    public sealed class Zone
    {
        public int zone;
        public int[] VisibleZones;
    }

    [System.Serializable]
    public sealed class AnimationProperties
    {
        public string Name;
        public bool Loop;
        public float Speed;
        public string PlayMode;
        public int Layer;
    }

    [System.Serializable]
    public sealed class AudioFile
    {
        public string File;
        public string Path;
    }

    [System.Serializable]
    public sealed class AudioProperties
    {
        public string Name;
        public int Priority;
        public float Volume;
        public bool Loop;
        public bool PlayOnAwake;
        public bool Randomize;
        public float SpatialBlend;
        public bool Spatialize;
        public float MinDistance;
        public float MaxDistance;
        public AudioFile[] AudioFiles;

        public override bool Equals(object obj)
        {
            AudioProperties other = obj as AudioProperties;
            if (other == null)
            {
                return false;
            }

            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return 363513814 + EqualityComparer<string>.Default.GetHashCode(Name);
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
    public sealed class TriggerTransformProperties
    {
        public bool SetParent;
        public bool SetIsKinematic;
        public bool SetPosition;
        public bool SetRotation;
        public bool SetScale;
        public bool SetIsActive;

        public string Parent;
        public bool IsKinematic;
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
        public bool IsActive;
    }

    [System.Serializable]
    public sealed class Trigger
    {
        public string TriggerEvent;
        public string[] TriggerSource; // special values: self, camera
        public string[] TriggerTarget; // special values: self, camera
        public string TriggerAction;
        public AnimationProperties[] AnimationProperties;
        public AudioProperties[] AudioProperties;
        public TriggerTransformProperties[] TriggerTansformProperties;
    }

    [System.Serializable]
    public sealed class ModelConfiguration : IConfiguration
    {
        string IConfiguration.DescriptiveName => DescriptiveName;
        string IConfiguration.Id => Id;

        public string DescriptiveName;
        public string Id;
        public string IdParent;
        public string Emulator;
        public string Model;
        public bool Grabbable;
        public float AnimatedTextureSpeed;
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
        public string Screen;
        public string Manufacturer;
        public string Year;
        public string Genre;
        public bool Mature;
        public bool Runnable;
        public bool Available;
        public string GameLauncherMethod;
        public int PlayCount;
        public int Zone;
        public Trigger[] Triggers;
        public string[] TriggerIDs;
    }

    [System.Serializable]
    public sealed class ArcadeConfiguration : IConfiguration
    {
        string IConfiguration.DescriptiveName => DescriptiveName;
        string IConfiguration.Id => Id;

        public string DescriptiveName;
        public string Id;
        public string ArcadeType;
        public RenderSettings RenderSettings;
        public AudioSettings AudioSettings;
        public VideoSettings VideoSettings;
        public CameraSettings CameraSettings;

        public FpsArcadeProperties[] FpsArcadeProperties;
        public CylArcadeProperties[] CylArcadeProperties;
        public Zone[] Zones;

        public ModelConfiguration[] ArcadeModelList;
        public ModelConfiguration[] GameModelList;
        public ModelConfiguration[] PropModelList;
    }
}

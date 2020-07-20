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

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace Arcade
{
    public enum WheelVariant
    {
        CameraInsideHorizontal,
        CameraOutsideHorizontal,
        LineHorizontal,
        CameraInsideVertical,
        CameraOutsideVertical,
        LineVertical,
        LineCustom
    }

    [System.Serializable]
    public sealed class CylArcadeProperties
    {
        public CameraSettings CameraSettings;

        [JsonConverter(typeof(StringEnumConverter))]
        public WheelVariant WheelVariant;

        public bool InverseNavigation;
        public bool InverseList;
        public float WheelRadius;
        public int Sprockets;
        public int SelectedSprocket;
        public float SelectedPositionZ;
        public float ModelSpacing;

        public bool MouseLook;
        public bool MouseLookReverse;

        public Vector3 SprocketRotation;

        public float LineAngle;
        public bool HorizontalNavigation;
    }
}

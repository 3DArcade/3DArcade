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

using UnityEngine;

namespace Arcade
{
    [DisallowMultipleComponent, SelectionBase]
    public sealed class ModelConfigurationComponent : MonoBehaviour
    {
        public string DescriptiveName           = default;
        public string Id                        = default;
        public string Model                     = default;
        public InteractionType InteractionType  = default;
        public string Emulator                  = default;
        public string[] MarqueeImageDirectories = default;
        public string[] MarqueeVideoDirectories = default;
        public string[] ScreenImageDirectories  = default;
        public string[] ScreenVideoDirectories  = default;
        public string[] TitleImageDirectories   = default;
        public string[] GenericImageDirectories = default;
        public string[] GenericVideoDirectories = default;
        public string[] InfoDirectories         = default;
        public bool Grabbable                   = default;
        public bool MoveCabMovable              = default;
        public bool MoveCabGrabbable            = default;

        public string CloneOf                          = default;
        public string RomOf                            = default;
        public string Genre                            = default;
        public string Year                             = default;
        public string Manufacturer                     = default;
        public GameScreenType ScreenType               = default;
        public GameScreenOrientation ScreenOrientation = default;
        public bool Mature                             = default;
        public bool Available                          = default;
        public bool Runnable                           = default;

        public void FromModelConfiguration(ModelConfiguration modelConfiguration)
        {
            DescriptiveName         = modelConfiguration.DescriptiveName;
            Id                      = modelConfiguration.Id;
            Model                   = modelConfiguration.Model;
            InteractionType         = modelConfiguration.InteractionType;
            Emulator                = modelConfiguration.Emulator;
            MarqueeImageDirectories = modelConfiguration.MarqueeImageDirectories;
            MarqueeVideoDirectories = modelConfiguration.MarqueeVideoDirectories;
            ScreenImageDirectories  = modelConfiguration.ScreenImageDirectories;
            ScreenVideoDirectories  = modelConfiguration.ScreenVideoDirectories;
            TitleImageDirectories   = modelConfiguration.TitleImageDirectories;
            GenericImageDirectories = modelConfiguration.GenericImageDirectories;
            GenericVideoDirectories = modelConfiguration.GenericVideoDirectories;
            InfoDirectories         = modelConfiguration.InfoDirectories;
            Grabbable               = modelConfiguration.Grabbable;
            MoveCabMovable          = modelConfiguration.MoveCabMovable;
            MoveCabGrabbable        = modelConfiguration.MoveCabGrabbable;

            CloneOf           = modelConfiguration.CloneOf;
            RomOf             = modelConfiguration.RomOf;
            Genre             = modelConfiguration.Genre;
            Year              = modelConfiguration.Year;
            Manufacturer      = modelConfiguration.Manufacturer;
            ScreenType        = modelConfiguration.ScreenType;
            ScreenOrientation = modelConfiguration.ScreenOrientation;
            Mature            = modelConfiguration.Mature;
            Available         = modelConfiguration.Available;
            Runnable          = modelConfiguration.Runnable;
        }

        public ModelConfiguration ToModelConfiguration() => new ModelConfiguration
        {
            DescriptiveName         = DescriptiveName,
            Id                      = Id,
            Model                   = Model,
            InteractionType         = InteractionType,
            Emulator                = Emulator,
            MarqueeImageDirectories = MarqueeImageDirectories,
            MarqueeVideoDirectories = MarqueeVideoDirectories,
            ScreenImageDirectories  = ScreenImageDirectories,
            TitleImageDirectories   = TitleImageDirectories,
            ScreenVideoDirectories  = ScreenVideoDirectories,
            GenericImageDirectories = GenericImageDirectories,
            GenericVideoDirectories = GenericVideoDirectories,
            InfoDirectories         = InfoDirectories,
            Grabbable               = Grabbable,
            MoveCabMovable          = MoveCabMovable,
            MoveCabGrabbable        = MoveCabGrabbable,

            CloneOf           = CloneOf,
            RomOf             = RomOf,
            Genre             = Genre,
            Year              = Year,
            Manufacturer      = Manufacturer,
            ScreenType        = ScreenType,
            ScreenOrientation = ScreenOrientation,
            Mature            = Mature,
            Available         = Available,
            Runnable          = Runnable,

            Position = transform.localPosition,
            Rotation = MathUtils.CorrectEulerAngles(transform.localEulerAngles),
            Scale    = transform.localScale
        };
    }
}

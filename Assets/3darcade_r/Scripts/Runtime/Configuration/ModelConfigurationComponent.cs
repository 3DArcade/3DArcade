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

namespace Arcade_r
{
    [DisallowMultipleComponent, SelectionBase]
    public sealed class ModelConfigurationComponent : MonoBehaviour
    {
        public string DescriptiveName          = default;
        public string Id                       = default;
        public string Model                    = default;
        public InteractionType InteractionType = default;
        public string Emulator                 = default;
        public string GameList                 = default;
        public string MarqueeDirectory         = default;
        public string MarqueeVideoDirectory    = default;
        public string ScreenDirectory          = default;
        public string ScreenVideoDirectory     = default;
        public string TitleDirectory           = default;
        public string GenericDirectory         = default;
        public string GenericVideoDirectory    = default;
        public string InfoDirectory            = default;
        public bool Grabbable                  = default;
        public bool MoveCabMovable             = default;
        public bool MoveCabGrabbable           = default;

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

        public Trigger[] Triggers  = default;
        public string[] TriggerIDs = default;

        public void FromModelConfiguration(ModelConfiguration modelConfiguration)
        {
            DescriptiveName       = modelConfiguration.DescriptiveName;
            Id                    = modelConfiguration.Id;
            Model                 = modelConfiguration.Model;
            InteractionType       = modelConfiguration.InteractionType;
            Emulator              = modelConfiguration.Emulator;
            GameList              = modelConfiguration.GameList;
            MarqueeDirectory      = modelConfiguration.MarqueeDirectory;
            MarqueeVideoDirectory = modelConfiguration.MarqueeVideoDirectory;
            ScreenDirectory       = modelConfiguration.ScreenDirectory;
            TitleDirectory        = modelConfiguration.TitleDirectory;
            ScreenVideoDirectory  = modelConfiguration.ScreenVideoDirectory;
            GenericDirectory      = modelConfiguration.GenericDirectory;
            GenericVideoDirectory = modelConfiguration.GenericVideoDirectory;
            InfoDirectory         = modelConfiguration.InfoDirectory;
            Grabbable             = modelConfiguration.Grabbable;
            MoveCabMovable        = modelConfiguration.MoveCabMovable;
            MoveCabGrabbable      = modelConfiguration.MoveCabGrabbable;

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

            Triggers   = modelConfiguration.Triggers;
            TriggerIDs = modelConfiguration.TriggerIDs;
        }

        public ModelConfiguration ToModelConfiguration()
        {
            return new ModelConfiguration
            {
                DescriptiveName       = DescriptiveName,
                Id                    = Id,
                Model                 = Model,
                InteractionType       = InteractionType,
                Emulator              = Emulator,
                GameList              = GameList,
                MarqueeDirectory      = MarqueeDirectory,
                MarqueeVideoDirectory = MarqueeVideoDirectory,
                ScreenDirectory       = ScreenDirectory,
                TitleDirectory        = TitleDirectory,
                ScreenVideoDirectory  = ScreenVideoDirectory,
                GenericDirectory      = GenericDirectory,
                GenericVideoDirectory = GenericVideoDirectory,
                InfoDirectory         = InfoDirectory,
                Grabbable             = Grabbable,
                MoveCabMovable        = MoveCabMovable,
                MoveCabGrabbable      = MoveCabGrabbable,

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

                Triggers   = Triggers,
                TriggerIDs = TriggerIDs,

                Position = transform.localPosition,
                Rotation = MathUtils.CorrectEulerAngles(transform.localEulerAngles),
                Scale    = transform.localScale
            };
        }
    }
}

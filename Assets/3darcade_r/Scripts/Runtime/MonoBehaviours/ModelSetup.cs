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
    [DisallowMultipleComponent]
    public abstract class ModelSetup : MonoBehaviour
    {
        [SerializeField] protected string _descriptiveName;
        [SerializeField] protected string _id;
        [SerializeField] protected string _idParent;
        [SerializeField] protected string _emulator;
        [SerializeField] protected string _model;
        [SerializeField] protected bool _grabbable;
        [SerializeField] protected float _animatedTextureSpeed;
        [SerializeField] protected string _screen;
        [SerializeField] protected string _manufacturer;
        [SerializeField] protected string _year;
        [SerializeField] protected string _genre;
        [SerializeField] protected bool _mature;
        [SerializeField] protected bool _runnable;
        [SerializeField] protected bool _available;
        [SerializeField] protected GameLauncherMethod _gameLauncherMethod;
        [SerializeField] protected int _playCount;
        [SerializeField] protected int _zone;
        [SerializeField] protected Trigger[] _triggers;
        [SerializeField] protected string[] _triggerIDs;

        public string DescriptiveName => _descriptiveName;
        public string Id => _id;

        public ModelConfiguration ToModelConfiguration()
        {
            return new ModelConfiguration
            {
                Position             = transform.localPosition,
                Rotation             = MathUtils.CorrectEulerAngles(transform.localEulerAngles),
                Scale                = transform.localScale,

                DescriptiveName      = _descriptiveName,
                Id                   = _id,
                IdParent             = _idParent,
                Emulator             = _emulator,
                Model                = _model,
                Grabbable            = _grabbable,
                AnimatedTextureSpeed = _animatedTextureSpeed,
                Screen               = _screen,
                Manufacturer         = _manufacturer,
                Year                 = _year,
                Genre                = _genre,
                Mature               = _mature,
                Runnable             = _runnable,
                Available            = _available,
                GameLauncherMethod   = _gameLauncherMethod.ToString(),
                PlayCount            = _playCount,
                Zone                 = _zone,
                Triggers             = _triggers,
                TriggerIDs           = _triggerIDs
            };
        }

        public void FromModelConfiguration(ModelConfiguration cfg)
        {
            _descriptiveName      = cfg.DescriptiveName;
            _id                   = cfg.Id;
            _idParent             = cfg.IdParent;
            _emulator             = cfg.Emulator;
            _model                = cfg.Model;
            _grabbable            = cfg.Grabbable;
            _animatedTextureSpeed = cfg.AnimatedTextureSpeed;
            _screen               = cfg.Screen;
            _manufacturer         = cfg.Manufacturer;
            _year                 = cfg.Year;
            _genre                = cfg.Genre;
            _mature               = cfg.Mature;
            _runnable             = cfg.Runnable;
            _available            = cfg.Available;
            if (!System.Enum.TryParse(cfg.GameLauncherMethod, true, out _gameLauncherMethod))
            {
                _gameLauncherMethod = GameLauncherMethod.None;
            }
            _playCount  = cfg.PlayCount;
            _zone       = cfg.Zone;
            _triggers   = cfg.Triggers;
            _triggerIDs = cfg.TriggerIDs;
        }
    }
}

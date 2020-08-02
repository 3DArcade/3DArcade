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
    [System.Serializable]
    public sealed class GeneralConfiguration
    {
        public string StartingArcade;
        [JsonConverter(typeof(StringEnumConverter))]
        public ArcadeType StartingArcadeType;

        private readonly IVirtualFileSystem _virtualFileSystem;

        public GeneralConfiguration(IVirtualFileSystem virtualFileSystem)
        {
            _virtualFileSystem = virtualFileSystem;
        }

        public bool Load()
        {
            try
            {
                GeneralConfiguration cfg = FileSystem.JsonDeserialize<GeneralConfiguration>(_virtualFileSystem.GetFile("general_cfg"));
                Debug.Log($"[{GetType().Name}] Loaded general configuration.");
                StartingArcade     = cfg.StartingArcade;
                StartingArcadeType = cfg.StartingArcadeType;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }

            return false;
        }

        public bool Save()
        {
            try
            {
                FileSystem.JsonSerialize(_virtualFileSystem.GetFile("general_cfg"), this);
                Debug.Log($"[{GetType().Name}] Saved general configuration.");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }

            return false;
        }
    }
}

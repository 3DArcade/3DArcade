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

using System;
using UnityEngine;

namespace Arcade_r
{
    public sealed class ApplicationInitState : ApplicationState
    {
        public ApplicationInitState(ApplicationStateContext context)
        : base(context)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("> <color=green>Entered</color> ApplicationInitState");

            try
            {
                _data.CurrentOS = SystemUtils.GetCurrentOS();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return;
            }
            Debug.Log($"Current OS: {_data.CurrentOS}");

            string vfsRootDirectory = SystemUtils.GetDataPath();
            _data.VirtualFileSystem = InitVFS(vfsRootDirectory);
            Debug.Log($"Data path: {vfsRootDirectory}");

            _data.ArcadeRootObject = SetupGameObjectHierarchy();

            _data.RaycastLayers = LayerMask.GetMask("Arcade/ArcadeModels", "Arcade/GameModels", "Arcade/PropModels");

            _context.TransitionTo<ApplicationLoadingArcadeState>();
        }

        public override void OnExit()
        {
            Debug.Log("> <color=orange>Exited</color> ApplicationInitState");
        }

        private static VFS InitVFS(string rootDirectory)
        {
            VFS result = new VFS();

            if (Application.isEditor)
            {
                result.MountDirectory("models", "3darcade/models");
                result.MountDirectory("arcade_models", "3darcade/models/arcades");
                result.MountDirectory("game_models", "3darcade/models/games");
                result.MountDirectory("prop_models", "3darcade/models/props");
            }

            result.MountDirectory("cfgs", $"{rootDirectory}/Configuration");
            result.MountDirectory("arcade_cfgs", $"{rootDirectory}/Configuration/Arcades");
            result.MountDirectory("emulator_cfgs", $"{rootDirectory}/Configuration/Emulators");
            result.MountDirectory("emulators", $"{rootDirectory}/Emulators");
            result.MountDirectory("media", $"{rootDirectory}/Media");

            result.MountFile("general_cfg", "cfg/GeneralConfiguration.json");

            return result;
        }

        private static GameObject SetupGameObjectHierarchy()
        {
            GameObject parentObj = GameObject.Find("Arcade");
            if (parentObj == null)
            {
                parentObj = new GameObject("Arcade")
                {
                    layer = LayerMask.NameToLayer("Arcade")
                };
            }

            string[] childrenNames = new string[] { "ArcadeModels", "GameModels", "PropModels" };
            foreach (string childName in childrenNames)
            {
                GameObject childObj = GameObject.Find(childName);
                if (childObj == null)
                {
                    childObj = new GameObject(childName)
                    {
                        layer = LayerMask.NameToLayer($"Arcade/{childName}")
                    };
                    childObj.transform.SetParent(parentObj.transform);
                }
            }

            return parentObj;
        }
    }
}

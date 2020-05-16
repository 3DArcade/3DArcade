﻿/* MIT License

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
    public sealed class ApplicationLoadingArcadeState : ApplicationState
    {
        private GameObject[] _loadedModels;

        public ApplicationLoadingArcadeState(ApplicationStateContext context)
        : base(context)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("> <color=green>Entered</color> ApplicationLoadingWorldState");

            _loadedModels = new GameObject[]
            {
                Resources.Load<GameObject>("Games/1942"),
                Resources.Load<GameObject>("Games/1943"),
                Resources.Load<GameObject>("Games/alpine"),
                Resources.Load<GameObject>("Games/amidar"),
                Resources.Load<GameObject>("Games/arkanoid"),
                Resources.Load<GameObject>("Games/asteroid")
            };
            MaterialUtils.SetGPUInstancing(true, _loadedModels);

            Vector3 position    = new Vector3(49.4f, 0f, 20f);
            Quaternion rotation = Quaternion.LookRotation(Vector3.left, Vector3.up);

            foreach (GameObject loadedModel in _loadedModels)
            {
                GameObject model = Object.Instantiate(loadedModel, position, rotation);
                _ = model.AddComponent<GameModelSetup>();
                model.layer = LayerMask.NameToLayer("Arcade/GameModels");
                position.z--;
            }

            _context.TransitionTo<ApplicationRunningState>();
        }

        public override void OnExit()
        {
            Debug.Log("> <color=orange>Exited</color> ApplicationLoadingWorldState");
        }
    }
}

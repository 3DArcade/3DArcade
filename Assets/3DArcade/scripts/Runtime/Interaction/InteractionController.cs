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

namespace Arcade
{
    public static class InteractionController
    {
        public static event Action<ModelConfigurationComponent> OnCurrentModelConfigurationChanged;

        public static void FindInteractable(ref ModelConfigurationComponent modelConfigurationComponent, Camera camera, float maxDistance, LayerMask layers)
        {
            Ray ray = camera.ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, layers))
            {
                ModelConfigurationComponent hitModelConfigurationComponent = hitInfo.transform.GetComponent<ModelConfigurationComponent>();
                if (hitModelConfigurationComponent != null && hitModelConfigurationComponent.InteractionType != InteractionType.None)
                {
                    if (hitModelConfigurationComponent != modelConfigurationComponent)
                    {
                        modelConfigurationComponent = hitModelConfigurationComponent;
                        OnCurrentModelConfigurationChanged?.Invoke(modelConfigurationComponent);
                    }
                }
                else
                {
                    modelConfigurationComponent = null;
                    OnCurrentModelConfigurationChanged?.Invoke(null);
                }
            }
            else
            {
                modelConfigurationComponent = null;
                OnCurrentModelConfigurationChanged?.Invoke(null);
            }
        }

        public static void FindInteractable(ref ModelConfigurationComponent modelConfigurationComponent, ArcadeController arcadeController)
        {
            modelConfigurationComponent = arcadeController.CurrentGame;
            OnCurrentModelConfigurationChanged?.Invoke(modelConfigurationComponent);
        }
    }
}

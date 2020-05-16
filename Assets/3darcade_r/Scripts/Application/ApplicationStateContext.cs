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
    public sealed class ApplicationStateContext : FSM.Context<ApplicationState>
    {
        public readonly ApplicationData Data;

        private bool _badLuck;

        public ApplicationStateContext(PlayerControls playerControls, Camera camera, GameObject theAbyss)
        {
            Data = new ApplicationData
            {
                PlayerControls = playerControls,
                Camera         = camera,
                TheAbyss       = theAbyss
            };
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            YouAreNotSupposedToBeHere();
        }

        private void YouAreNotSupposedToBeHere()
        {
            if (!_badLuck && Data.PlayerControls.transform.position.y < -340f)
            {
                Data.PlayerControls.transform.position = new Vector3(0f, Data.PlayerControls.transform.position.y, 0f);
                _ = Object.Instantiate(Data.TheAbyss);
                _badLuck = true;
            }
        }
    }
}

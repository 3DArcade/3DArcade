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
    public sealed class ArcadeInternalGameState : ArcadeState
    {
        private readonly InternalGameController _libretroController;

        public ArcadeInternalGameState(ArcadeContext context)
        : base(context) => _libretroController = new InternalGameController(_context.CurrentPlayerControls.transform);

        public override void OnEnter()
        {
            Debug.Log($">> <color=green>Entered</color> {GetType().Name}");

            ScreenNodeTag screenNodeTag = _context.CurrentModelConfiguration.GetComponentInChildren<ScreenNodeTag>(true);
            if (screenNodeTag != null)
            {
                EmulatorConfiguration emulator = _context.GetEmulatorForCurrentModelConfiguration();
                if (emulator != null)
                {
                    if (_libretroController.StartGame(screenNodeTag, emulator.Executable, emulator.GamesDirectories, _context.CurrentModelConfiguration.Id))
                    {
                        _context.VideoPlayerController?.StopAllVideos();
                        return;
                    }
                }
            }

            if (_context.CurrentArcadeType == ArcadeType.Fps)
                _context.TransitionTo<ArcadeFpsNormalState>();
            else if (_context.CurrentArcadeType == ArcadeType.Cyl)
                _context.TransitionTo<ArcadeCylNormalState>();
        }

        public override void OnExit()
        {
            Debug.Log($">> <color=orange>Exited</color> {GetType().Name}");
            _libretroController.StopGame();
        }

        public override void Update(float dt)
        {
            _libretroController.UpdateGame();

            if (_context.CurrentPlayerControls.GlobalActions.Quit.triggered)
            {
                if (_context.CurrentArcadeType == ArcadeType.Fps)
                    _context.TransitionTo<ArcadeFpsNormalState>();
                else if (_context.CurrentArcadeType == ArcadeType.Cyl)
                    _context.TransitionTo<ArcadeCylNormalState>();
            }
        }
    }
}

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

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arcade
{
    public sealed class VideoPlayerControllerFps : VideoPlayerController
    {
        private const float OVERLAPSPHERE_RADIUS       = 1.8f;
        private const int NUM_CABS_WITH_VIDEOS_PLAYING = 3;
        private const int NUM_COLLIDERS_TO_PROCESS     = 10;

        private readonly List<ModelConfigurationComponent> _activeVideos;
        private readonly Collider[] _overlapSphereHits;
        private readonly LayerMask _layerMask;

        private Transform _player;

        public VideoPlayerControllerFps(LayerMask layerMask)
        {
            _activeVideos      = new List<ModelConfigurationComponent>();
            _overlapSphereHits = new Collider[NUM_COLLIDERS_TO_PROCESS];
            _layerMask         = layerMask;
        }

        public override void SetPlayer(Transform player) => _player = player;

        public override void UpdateVideosState()
        {
            if (_player == null)
                return;

            for (int i = 0; i < _overlapSphereHits.Length; ++i)
                _overlapSphereHits[i] = null;

            _ = Physics.OverlapSphereNonAlloc(_player.position, OVERLAPSPHERE_RADIUS, _overlapSphereHits, _layerMask);

            ModelConfigurationComponent[] inRange = _overlapSphereHits.Where(col => col != null)
                                                                            .OrderBy(col => MathUtils.DistanceFast(col.transform.position, _player.position))
                                                                            .Take(NUM_CABS_WITH_VIDEOS_PLAYING)
                                                                            .Select(col => col.GetComponent<ModelConfigurationComponent>())
                                                                            .Where(mc => mc != null)
                                                                            .ToArray();

            foreach (ModelConfigurationComponent modelConfigurationComponent in inRange)
            {
                if (!_activeVideos.Contains(modelConfigurationComponent))
                    _activeVideos.Add(modelConfigurationComponent);

                PlayVideo(modelConfigurationComponent);
            }

            ModelConfigurationComponent[] toDisable = _activeVideos.Except(inRange)
                                                                   .ToArray();
            foreach (ModelConfigurationComponent modelConfigurationComponent in toDisable)
            {
                StopVideo(modelConfigurationComponent);
                _ = _activeVideos.Remove(modelConfigurationComponent);
            }
        }

        public override void StopAllVideos()
        {
            foreach (ModelConfigurationComponent modelConfigurationComponent in _activeVideos)
                StopVideo(modelConfigurationComponent);
            _activeVideos.Clear();
        }
    }
}

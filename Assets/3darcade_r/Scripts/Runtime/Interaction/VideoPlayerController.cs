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
using UnityEngine.Video;

namespace Arcade_r
{
    public class VideoPlayerController
    {
        private const float OVERLAPSPHERE_RADIUS   = 1.8f;
        private const int NUM_VIDEOS_WITH_SOUND    = 3;
        private const int NUM_COLLIDERS_TO_PROCESS = 10;

        private readonly Transform _player;
        private readonly List<VideoPlayer> _activeVideos;
        private readonly Collider[] _overlapSphereHits;
        private readonly LayerMask _layerMask;

        public VideoPlayerController(Transform player, LayerMask layerMask)
        {
            _player            = player;
            _activeVideos      = new List<VideoPlayer>();
            _overlapSphereHits = new Collider[NUM_COLLIDERS_TO_PROCESS];
            _layerMask         = layerMask;
        }

        public void UpdateVideosState()
        {
            for (int i = 0; i < _overlapSphereHits.Length; ++i)
            {
                _overlapSphereHits[i] = null;
            }

            _ = Physics.OverlapSphereNonAlloc(_player.position, OVERLAPSPHERE_RADIUS, _overlapSphereHits, _layerMask);

            VideoPlayer[] inRange = _overlapSphereHits.Distinct()
                                                      .Where(col => col != null)
                                                      .Select(col => col.GetComponentInChildren<VideoPlayer>())
                                                      .Where(vp => vp != null && vp.isPrepared)
                                                      .OrderBy(vp => MathUtils.DistanceFast(vp.transform.position, _player.position))
                                                      .Take(NUM_VIDEOS_WITH_SOUND)
                                                      .ToArray();

            VideoPlayer[] toEnable = inRange.Where(vp => vp.isPaused)
                                            .ToArray();

            foreach (VideoPlayer videoPlayer in toEnable)
            {
                if (!_activeVideos.Contains(videoPlayer))
                {
                    VideoSetPlayingState(videoPlayer, true);
                    _activeVideos.Add(videoPlayer);
                }
            }

            VideoPlayer[] toDisable = _activeVideos.Except(inRange)
                                                   .Except(toEnable)
                                                   .ToArray();
            foreach (VideoPlayer videoPlayer in toDisable)
            {
                VideoSetPlayingState(videoPlayer, false);
                _ = _activeVideos.Remove(videoPlayer);
            }
        }

        public void StopAllVideos()
        {
            foreach (VideoPlayer videoPlayer in _activeVideos)
            {
                VideoSetPlayingState(videoPlayer, false);
            }
            _activeVideos.Clear();
        }

        private void VideoSetPlayingState(VideoPlayer videoPlayer, bool state)
        {
            if (state && videoPlayer.isPaused)
            {
                videoPlayer.EnableAudioTrack(0, true);
                videoPlayer.Play();
            }
            else if (!state && !videoPlayer.isPaused)
            {
                videoPlayer.EnableAudioTrack(0, false);
                videoPlayer.Pause();
            }
        }
    }
}

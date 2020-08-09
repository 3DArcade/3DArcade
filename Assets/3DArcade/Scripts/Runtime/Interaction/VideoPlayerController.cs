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
using UnityEngine.Video;

namespace Arcade
{
    public abstract class VideoPlayerController
    {
        public virtual void SetPlayer(Transform player)
        {
        }

        public virtual void UpdateVideosState()
        {
        }

        public virtual void StopAllVideos()
        {
        }

        public void PlayVideo(MonoBehaviour monoBehaviour)
        {
            VideoSetPlayingState(monoBehaviour, true);
        }

        public void StopVideo(MonoBehaviour monoBehaviour)
        {
            VideoSetPlayingState(monoBehaviour, false);
        }

        protected void VideoSetPlayingState(MonoBehaviour monoBehaviour, bool state)
        {
            if (monoBehaviour == null)
            {
                return;
            }

            VideoPlayer[] videoPlayers = monoBehaviour.GetComponentsInChildren<VideoPlayer>();
            foreach (VideoPlayer videoPlayer in videoPlayers)
            {
                VideoSetPlayingState(videoPlayer, state);
            }
        }

        protected void VideoSetPlayingState(VideoPlayer videoPlayer, bool state)
        {
            if (!videoPlayer.enabled)
            {
                return;
            }

            videoPlayer.EnableAudioTrack(0, state);

            if (state)
            {
                videoPlayer.Play();
            }
            else
            {
                videoPlayer.Stop();
            }
        }
    }
}

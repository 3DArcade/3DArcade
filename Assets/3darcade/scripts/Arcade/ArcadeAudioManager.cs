using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

namespace Arcade
{
    public class ArcadeAudioManager : MonoBehaviour
    {
        public static List<ModelVideoSetup> ActiveVideos = new List<ModelVideoSetup>();

        private const int FRAMES_TO_SKIP = 10;

        private static List<ModelVideoSetup> tempVideosList = new List<ModelVideoSetup>();
        private static int _frames = 0;

        private void Update()
        {
            // No need to update every frame.
            if (_frames++ % FRAMES_TO_SKIP != 0)
            {
                _frames = 0;
                return;
            }

            _ = ActiveVideos.RemoveAll(x => x == null);

            tempVideosList = ActiveVideos
                .Where(x =>
                    x.videoPlayer != null
                    && x.videoPlayer.isActiveAndEnabled
                    && x.videoPlayer.isPlaying
                    && !x.videoPlayer.isPaused
                    && x.videoPlayer.isPrepared)
                .ToList();

            foreach (ModelVideoSetup item in tempVideosList)
            {
                //  print("sources " + item.modelProperties.id + " ispaused " + item.videoPlayer.isActiveAndEnabled);
                if (item.videoPlayer.audioOutputMode == VideoAudioOutputMode.Direct)
                {
                    item.videoPlayer.SetDirectAudioMute(0, true);
                }
                else if (item.videoPlayer.audioOutputMode == VideoAudioOutputMode.AudioSource)
                {
                    AudioSource audioSource = item.videoPlayer.GetTargetAudioSource(0);
                    if (audioSource != null)
                    {
                        audioSource.mute = true;
                        audioSource.enabled = false;
                    }
                }
            }
            //  print("sourcesC " + temp.Count);
            tempVideosList.Sort((x, y) => x.distance.CompareTo(y.distance));
            EnableAudio(3);
            _ = tempVideosList.RemoveAll(x => x.arcadeLayer == true);
            EnableAudio(1);
            void EnableAudio(int maximumVideosWithSound)
            {
                int t = 0;
                while (t < maximumVideosWithSound)
                {
                    if (tempVideosList.Count > t)
                    {
                        if (tempVideosList[t].videoPlayer.audioOutputMode == VideoAudioOutputMode.Direct)
                        {
                            tempVideosList[t].videoPlayer.SetDirectAudioMute(0, false);
                        }
                        else if (tempVideosList[t].videoPlayer.audioOutputMode == VideoAudioOutputMode.AudioSource)
                        {
                            AudioSource audioSource = tempVideosList[t].videoPlayer.GetTargetAudioSource(0);
                            if (audioSource != null)
                            {
                                audioSource.mute = false;
                                audioSource.enabled = true;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                    ++t;
                }
            }
        }
    }
}

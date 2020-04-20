using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Arcade;
using System.Linq;

namespace Arcade
{
    public class ArcadeAudioManager : MonoBehaviour
    {
        public static List<ModelVideoSetup> activeVideos = new List<ModelVideoSetup>();
        private static int frames = 0;
        private static readonly int framesToSkip = 10;
       
        void Update()
        {
            // No need to update every frame.
            frames++;
            if (frames < framesToSkip) { return; }
            frames = 0;

            activeVideos.RemoveAll(x => x == null);
            List<ModelVideoSetup> temp = new List<ModelVideoSetup>();
            temp.AddRange(activeVideos);
            temp.RemoveAll(x => x.videoPlayer == null);
            temp.RemoveAll(x => !x.videoPlayer.isActiveAndEnabled);
            temp.RemoveAll(x => !x.videoPlayer.isPlaying);
            temp.RemoveAll(x => x.videoPlayer.isPaused);
            temp.RemoveAll(x => !x.videoPlayer.isPrepared);

            foreach (ModelVideoSetup item in temp)
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
           temp.Sort((x,y) => x.distance.CompareTo(y.distance));
            EnableAudio(3);
            temp.RemoveAll(x => x.arcadeLayer == true);
            EnableAudio(1);
            void EnableAudio(int maximumVideosWithSound)
            {
                int t = 0;
                while (t < maximumVideosWithSound)
                {
                    if (temp.Count > t)
                    {
                        if (temp[t].videoPlayer.audioOutputMode == VideoAudioOutputMode.Direct)
                        {
                            temp[t].videoPlayer.SetDirectAudioMute(0, false);
                        }
                        else if (temp[t].videoPlayer.audioOutputMode == VideoAudioOutputMode.AudioSource)
                        {
                            AudioSource audioSource = temp[t].videoPlayer.GetTargetAudioSource(0);
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
                    t += 1;
                }
            }
        }
    }
}

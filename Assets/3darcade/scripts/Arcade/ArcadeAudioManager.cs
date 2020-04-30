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

        private List<ModelVideoSetup> tempVideosList = new List<ModelVideoSetup>();
        private int _frames = 0;

        public Transform _player;
        public float  _radius = 1.4f;
        public LayerMask _layerMask;

        public Collider[] hits = new Collider[10];
        public VideoPlayer[] closestVideoPlayers;

        private void Update()
        {
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            // No need to update every frame.
            //if (_frames++ % FRAMES_TO_SKIP != 0)
            //{
            //    _frames = 0;
            //    return;
            //}

            _ = ActiveVideos.RemoveAll(x => x == null);

            tempVideosList = ActiveVideos
                .Where(x =>
                       x.videoPlayer != null
                    && x.videoPlayer.isActiveAndEnabled
                    && x.videoPlayer.isPlaying
                    && !x.videoPlayer.isPaused
                    && x.videoPlayer.isPrepared)
                .ToList();

            VideoPlayer[] activeVideoPlayers = tempVideosList.Select(x => x.GetComponentInChildren<VideoPlayer>())
                                                             .Where(x => x != null && x.isActiveAndEnabled && x.isPlaying && !x.isPaused && x.isPrepared)
                                                             .ToArray();

            foreach (VideoPlayer videoPlayer in activeVideoPlayers)
            {
                //  print("sources " + item.modelProperties.id + " ispaused " + item.videoPlayer.isActiveAndEnabled);
                VideoSetAudioState(videoPlayer, false);
            }

            if (Physics.OverlapSphereNonAlloc(_player.position, _radius, hits, _layerMask) == 0)
            {
                return;
            }

            closestVideoPlayers = hits.Select(c => c.GetComponentInChildren<VideoPlayer>())
                                      .Where(vp => vp != null)
                                      .OrderBy(vp => (vp.transform.position - _player.position).sqrMagnitude)
                                      .Take(3)
                                      .ToArray();

            foreach (VideoPlayer videoPlayer in closestVideoPlayers)
            {
                VideoSetAudioState(videoPlayer, true);
            }

            //  print("sourcesC " + temp.Count);
            //tempVideosList.Sort((x, y) => x.distance.CompareTo(y.distance));
            //EnableAudio(3);
            //_ = tempVideosList.RemoveAll(x => x.arcadeLayer == true);
            //EnableAudio(1);

            //void EnableAudio(int maximumVideosWithSound)
            //{
            //    int t = 0;
            //    while (t < maximumVideosWithSound)
            //    {
            //        if (tempVideosList.Count > t)
            //        {
            //            SetVideoMuteState(tempVideosList[t].videoPlayer, false);
            //        }
            //        else
            //        {
            //            break;
            //        }
            //        ++t;
            //    }
            //}

            sw.Stop();
            if (_frames++ % 60 == 0)
            {
                Debug.Log(sw.Elapsed.TotalMilliseconds);
                _frames = 0;
            }
        }

        private static void VideoSetAudioState(VideoPlayer videoPlayer, bool state)
        {
            if (videoPlayer.audioOutputMode == VideoAudioOutputMode.Direct)
            {
                videoPlayer.SetDirectAudioMute(0, !state);
            }
            else if (videoPlayer.audioOutputMode == VideoAudioOutputMode.AudioSource)
            {
                AudioSource audioSource = videoPlayer.GetTargetAudioSource(0);
                if (audioSource != null)
                {
                    audioSource.mute    = !state;
                    audioSource.enabled = state;
                }
            }
        }
    }
}

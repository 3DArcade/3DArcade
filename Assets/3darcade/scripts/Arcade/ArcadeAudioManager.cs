using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

namespace Arcade
{
    public class ArcadeAudioManager : MonoBehaviour
    {
        public static List<ModelVideoSetup> ActiveVideos = new List<ModelVideoSetup>();

        [SerializeField] private Transform _player;
        [SerializeField] private float  _radius = 1.4f;
        [SerializeField] private LayerMask _layerMask;

        private const int NUM_VIDEOS_WITH_SOUND    = 3;
        private const int NUM_COLLIDERS_TO_PROCESS = 10;
        private const int NUM_FRAMES_TO_SKIP       = 10;

        private readonly Collider[] _overlapSphereHits = new Collider[NUM_COLLIDERS_TO_PROCESS];

        private int _skipFrameCounter = 0;

        private void Update()
        {
            // No need to update every frame.
            if (_skipFrameCounter++ % NUM_FRAMES_TO_SKIP != 0)
            {
                _skipFrameCounter = 0;
                return;
            }

            _ = ActiveVideos.RemoveAll(x => x == null);

            _ = Physics.OverlapSphereNonAlloc(_player.position, _radius, _overlapSphereHits, _layerMask);

            IEnumerable<VideoPlayer> toEnable = _overlapSphereHits.Where(col => col != null)
                                                                  .Select(col => col.GetComponentInChildren<VideoPlayer>())
                                                                  .Where(vp => vp != null && vp.isPlaying && vp.isPrepared)
                                                                  .Distinct()
                                                                  .OrderBy(vp => MathUtils.DistanceFast(vp.transform.position, _player.position))
                                                                  .Take(NUM_VIDEOS_WITH_SOUND);
            foreach (VideoPlayer videoPlayer in toEnable)
            {
                AudioSource audioSource = videoPlayer.GetTargetAudioSource(0);
                if (audioSource != null && !audioSource.enabled)
                {
                    VideoSetAudioState(videoPlayer, true);
                }
            }

            IEnumerable<VideoPlayer> toDisable = ActiveVideos.Select(mvs => mvs.GetComponent<VideoPlayer>())
                                                             .Where(vp => vp != null)
                                                             .Except(toEnable);
            foreach (VideoPlayer videoPlayer in toDisable)
            {
                AudioSource audioSource = videoPlayer.GetTargetAudioSource(0);
                if (audioSource != null && audioSource.enabled)
                {
                    VideoSetAudioState(videoPlayer, false);
                }
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

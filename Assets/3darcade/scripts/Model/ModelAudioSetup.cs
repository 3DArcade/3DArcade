using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Arcade
{
    public class ModelAudioSetup : MonoBehaviour
    {
        public bool JukeboxEnabled;
        private bool updateInProgress;
        private bool loop;
        private List<string> clips = new List<string>();
        private int clipIndex;
        public AudioProperties audioProperties = null;
        private AudioSource audioSource;
        private AudioClip audioClip;

        public void Setup(AudioProperties audioProperties)
        {
            if (audioProperties == null || !Application.isPlaying)
            { return; }

            // Resume if not null
            if (this.audioProperties != null)
            {
                if (audioProperties.Equals(this.audioProperties))
                {
                    //print("equals");
                    audioSource.Play();
                    JukeboxEnabled = true;
                }
            }
            else
            {
                //print("setupaudio");
                clips = new List<string>();
                this.audioProperties = audioProperties;
                audioSource = gameObject.GetComponent<UnityEngine.AudioSource>();
                if (audioSource == null)
                {
                    audioSource = gameObject.AddComponent<UnityEngine.AudioSource>();
                }
                audioSource.priority = audioProperties.priority;
                audioSource.volume = audioProperties.volume;
                audioSource.playOnAwake = audioProperties.playOnAwake;
                audioSource.spatialBlend = audioProperties.spatialBlend;
                audioSource.spatialize = audioProperties.spatialize;
                audioSource.minDistance = audioProperties.minDistance;
                audioSource.maxDistance = audioProperties.maxDistance;
                audioSource.loop = audioProperties.loop;

                foreach (AudioFile audioFile in audioProperties.audioFiles)
                {
                    string file = FileManager.FileExists(ArcadeManager.applicationPath + FileManager.CorrectFilePath(audioFile.path.Trim()), audioFile.file.Trim());
                    if (file != null)
                    {
                        clips.Add("file://" + file);
                    }
                    else
                    {
                        List<string> files = FileManager.GetAudioPathsFromFolder(ArcadeManager.applicationPath + FileManager.CorrectFilePath(audioFile.path.Trim()));
                        //print(files[0]);
                        foreach (string i in files)
                        {
                            clips.Add("file://" + i);
                        }
                    }
                    if (clips.Count > 0)
                    {
                        if (clips.Count == 1)
                        {
                            audioSource.loop = audioProperties.loop;
                        }
                        else
                        {
                            audioSource.loop = false;
                        }
                        loop = audioProperties.loop;
                        if (audioProperties.Randomize)
                        { clips.Shuffle(); }
                        clipIndex = -1;
                        JukeboxEnabled = true;
                    }
                }
            }
        }

        private void Update()
        {
            if (JukeboxEnabled)
            {
                if (!audioSource.isPlaying && !updateInProgress)
                {
                    clipIndex += 1;
                    if (clipIndex > clips.Count - 1)
                    {
                        if (loop)
                        {
                            clipIndex = 0;
                            updateInProgress = true;
                            _ = StartCoroutine(LoadAudio(clips[clipIndex]));
                        }
                        else
                        {
                            JukeboxEnabled = false;
                        }
                    }
                    else
                    {
                        updateInProgress = true;
                        _ = StartCoroutine(LoadAudio(clips[clipIndex]));
                    }
                }
            }
        }

        private IEnumerator LoadAudio(string file)
        {
            using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(file, AudioType.UNKNOWN))
            {
                yield return uwr.SendWebRequest();
                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    Debug.LogError(uwr.error);
                    yield break;
                }
                audioClip = DownloadHandlerAudioClip.GetContent(uwr);
            }

            if (audioClip != null)
            {
                audioClip.name = audioProperties.name;
                PlayAudioFile();
                updateInProgress = false;
            }
        }

        private void PlayAudioFile()
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        private void OnApplicationQuit()
        {
            if (audioSource != null)
            {
                DestroyImmediate(gameObject.GetComponent<UnityEngine.AudioSource>());
            }
        }
    }

    public static class ShuffleMe
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}

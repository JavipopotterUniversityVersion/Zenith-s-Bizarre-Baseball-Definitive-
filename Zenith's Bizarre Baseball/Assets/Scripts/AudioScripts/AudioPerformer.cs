using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPerformer : MonoBehaviour
{

    AudioPlayer[] _audioPlayer;
    static private AudioPerformer _instance;
    private void Awake()
    {

        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            _audioPlayer = Resources.LoadAll<AudioPlayer>("SoundPlayers");

            for (int i = 0; i < _audioPlayer.Length; i++)
            {
                AudioSource currentSource = gameObject.AddComponent<AudioSource>();

                _audioPlayer[i].OnAudioOneShotPlay.
                AddListener((AudioClip clip, float volume, float pitch, bool loop) =>
                    {
                        if(currentSource.volume != volume) currentSource.volume = volume;
                        if(currentSource.pitch != pitch) currentSource.pitch = pitch;
                        if(currentSource.clip != clip) currentSource.clip = clip;
                        if(currentSource.loop != loop) currentSource.loop = loop;

                        currentSource.Play();
                    }
                );

                _audioPlayer[i].OnAudioPlay.
                AddListener((AudioClip clip, float volume, float pitch, bool loop) =>
                    {
                        if(currentSource.volume != volume) currentSource.volume = volume;
                        if(currentSource.pitch != pitch) currentSource.pitch = pitch;
                        if(currentSource.clip != clip) currentSource.clip = clip;
                        if(currentSource.loop != loop) currentSource.loop = loop;
                        
                        currentSource.PlayOneShot(clip);
                    }
                );

                _audioPlayer[i].OnAudioStop.AddListener(() => currentSource.Stop());
            }
        }
    }
}

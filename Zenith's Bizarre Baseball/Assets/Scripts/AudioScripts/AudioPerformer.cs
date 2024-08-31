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
            DontDestroyOnLoad(gameObject);
            _audioPlayer = Resources.LoadAll<AudioPlayer>("SoundPlayers");

            for (int i = 0; i < _audioPlayer.Length; i++)
            {
                AudioHandler _audioHandler = gameObject.AddComponent<AudioHandler>();
                _audioHandler.SetAudioPlayer(_audioPlayer[i]);
            }
        }
    }

    public void SetVolume(AudioType type, float volume)
    {
        for (int i = 0; i < _audioPlayer.Length; i++)
        {
            if (_audioPlayer[i].audioType == type)
            {
                _audioPlayer[i].SetVolume(volume);
            }
        }
    }

    public static void SetSFXVolume(float volume) => _instance.SetVolume(AudioType.SFX, volume);
    public static void SetMusicVolume(float volume) => _instance.SetVolume(AudioType.Music, volume);
}

public enum AudioType
{
    SFX,
    Music
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AudioChannel", menuName = "Audio/AudioChannel", order = 1)]
public class AudioChannel : ScriptableObject
{
    [SerializeField] AudioPlayer _currentAudio;

    [SerializeField] float fadeTime = 0.5f;
    public void SetFadeTime(float time) => fadeTime = time;

    public void Play(AudioPlayer audio)
    {
        if(_currentAudio != null) _currentAudio.FadeOut(fadeTime);
        _currentAudio = audio;
        _currentAudio.FadeIn(fadeTime);
    }

    public void Stop()
    {
        if(_currentAudio != null) _currentAudio.FadeOut(fadeTime);
    }
}

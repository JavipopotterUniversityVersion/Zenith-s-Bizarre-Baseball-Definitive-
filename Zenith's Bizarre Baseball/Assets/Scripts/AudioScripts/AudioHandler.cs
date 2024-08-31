using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    AudioSource _source;
    AudioPlayer _audioPlayer;
    [SerializeField] int _simultaneousSounds;
    int simultaneousCount = 0;
    float _localVolume;

    public void SetAudioPlayer(AudioPlayer audioPlayer)
    {
        _audioPlayer = audioPlayer;
        _audioPlayer.OnPlay.AddListener(Play);
        _audioPlayer.OnPlayOneShot.AddListener(PlayOneShot);
        _audioPlayer.OnVolumeSet.AddListener(SetVolume);
        _audioPlayer.OnPitchSet.AddListener(SetPitch);
        _audioPlayer.OnAudioStop.AddListener(Stop);
        _audioPlayer.OnFadeIn.AddListener(FadeIn);
        _audioPlayer.OnFadeOut.AddListener(FadeOut);
        _source.loop = _audioPlayer.Loop;
        _localVolume = _audioPlayer.LocalVolume;
        _source.volume = _localVolume;
        _source.pitch = _audioPlayer.Pitch;
        _simultaneousSounds = _audioPlayer.SimultaneousSounds;
    }
    
    private void Awake()
    {
        _source = gameObject.AddComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        if(clip != null && _source.clip != clip) _source.clip = clip;
        _source.Play();
    }

    public void PlayOneShot(AudioClip clip)
    {
        if(clip != null && simultaneousCount < _simultaneousSounds) 
            StartCoroutine(PlayOneShotRoutine(clip));
    }

    IEnumerator PlayOneShotRoutine(AudioClip clip)
    {
        simultaneousCount++;
        _source.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        simultaneousCount--;
    }

    public void Stop() => _source.Stop();
    public void Pause() => _source.Pause();
    public void UnPause() => _source.UnPause();

    public void FadeOut(float time)
    { 
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(time));
    }
    IEnumerator FadeRoutine(float duration)
    {
        float startVolume = _source.volume;
        while (_source.volume > 0)
        {
            _source.volume -= startVolume * Time.fixedDeltaTime / duration;
            yield return new WaitForFixedUpdate();
        }
        _source.Stop();
        _source.volume = startVolume;
    }

    public void FadeIn(float time)
    {
        StopAllCoroutines();
        StartCoroutine(FadeInRoutine(time));
    }
    IEnumerator FadeInRoutine(float duration)
    {
        float startVolume = _source.volume;
        _source.volume = 0;
        Play(_audioPlayer.audioClip());
        while (_source.volume < startVolume)
        {
            _source.volume += startVolume * Time.fixedDeltaTime / duration;
            yield return new WaitForFixedUpdate();
        }
        _source.volume = startVolume;
    }

    public void SetVolume(float volume)
    {
        _source.volume = volume * _localVolume;
    }
    public void SetPitch(float pitch) => _source.pitch = pitch;

    private void OnDestroy() {
        _audioPlayer.OnPlay.RemoveListener(Play);
        _audioPlayer.OnPlayOneShot.RemoveListener(PlayOneShot);
        _audioPlayer.OnVolumeSet.RemoveListener(SetVolume);
        _audioPlayer.OnPitchSet.RemoveListener(SetPitch);
        _audioPlayer.OnAudioStop.RemoveListener(Stop);
    }
}

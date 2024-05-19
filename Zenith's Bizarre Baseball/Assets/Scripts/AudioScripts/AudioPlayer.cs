using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
//using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewSoundEffect", menuName = "Audio/New Sound Effect")]
public class AudioPlayer : ScriptableObject
{
    public AudioClip[] clips;
    public Vector2 volume = new Vector2(1f, 1f);
    public Vector2 pitch = new Vector2(1f, 1f);
    [SerializeField] int playIndex;
    [SerializeField] private SoundClipOrder playOrder;
    [SerializeField] private bool _loop;

    private UnityEvent<AudioClip, float, float, bool> _onAudioOnShotPlay = new UnityEvent<AudioClip, float, float, bool>();
    public UnityEvent<AudioClip, float, float, bool> OnAudioOneShotPlay => _onAudioOnShotPlay;

    private UnityEvent<AudioClip, float, float, bool> _onAudioPlay = new UnityEvent<AudioClip, float, float, bool>();
    public UnityEvent<AudioClip, float, float, bool> OnAudioPlay => _onAudioPlay;

    private UnityEvent onAudioStop = new UnityEvent();
    public UnityEvent OnAudioStop => onAudioStop;


    private AudioClip audioClip()
    {
        //utiliza la pista de audio actual
        var clip = clips[playIndex >= clips.Length ? 0 : playIndex];

        //busca la proxima pista de audio
        switch (playOrder)
        {
            case SoundClipOrder.inOrder:
                playIndex = (playIndex + 1) % clips.Length;
                break;
            case SoundClipOrder.random:
                playIndex = Random.Range(0,clips.Length);
                break;
            case SoundClipOrder.reverse:
                playIndex = (playIndex - 1) % clips.Length;
                break;

        }


        return clip;
    }

    public void PlayMusic()
    {
        if (clips.Length == 0)  //por si acaso falta una pista de audio
        {
            Debug.Log($"Falta el clip de audio {name}");
        }

        float actualVolume = Random.Range(volume.x, volume.y);
        float actualPitch = Random.Range(pitch.x, pitch.y);

        _onAudioOnShotPlay?.Invoke(audioClip(), actualVolume, actualPitch, _loop);
    }

    public void Play()
    {
        if (clips.Length == 0)  //por si acaso falta una pista de audio
        {
            Debug.Log($"Falta el clip de audio {name}");
        }

        float actualVolume = Random.Range(volume.x, volume.y);
        float actualPitch = Random.Range(pitch.x, pitch.y);

        _onAudioPlay?.Invoke(audioClip(), actualVolume, actualPitch, _loop);
    }

    public void Stop() => onAudioStop?.Invoke();

    enum SoundClipOrder
    {
        random,
        inOrder,
        reverse
    }
}

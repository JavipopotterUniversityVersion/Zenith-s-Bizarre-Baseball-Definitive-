using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewSoundEffect", menuName = "Audio/New Sound Effect")]
public class AudioPlayer : ScriptableObject
{
    public AudioClip[] clips;
    [Range(0,1)] public float _localVolume = 1;
    public float LocalVolume => _localVolume;

    public Vector2 pitch = new Vector2(1f, 1f);
    public float Pitch => Random.Range(pitch.x, pitch.y);

    [SerializeField] int playIndex;
    [SerializeField] private SoundClipOrder playOrder;
    [SerializeField] private bool _loop;
    public bool Loop => _loop;
    public AudioType audioType = AudioType.SFX;

    [SerializeField] int _simultaneousSounds = 3;
    public int SimultaneousSounds => _simultaneousSounds;

    #region Events
    private UnityEvent<AudioClip> _onPlay = new UnityEvent<AudioClip>();
    public UnityEvent<AudioClip> OnPlay => _onPlay;

    private UnityEvent<AudioClip> _onPlayOneShot = new UnityEvent<AudioClip>();
    public UnityEvent<AudioClip> OnPlayOneShot => _onPlayOneShot;

    UnityEvent<float> _onPitchSet = new UnityEvent<float>();
    public UnityEvent<float> OnPitchSet => _onPitchSet;

    UnityEvent<float> _onVolumeSet = new UnityEvent<float>();
    public UnityEvent<float> OnVolumeSet => _onVolumeSet;

    private UnityEvent _onAudioStop = new UnityEvent();
    public UnityEvent OnAudioStop => _onAudioStop;

    private UnityEvent<float> _onFadeIn = new UnityEvent<float>();
    public UnityEvent<float> OnFadeIn => _onFadeIn;

    private UnityEvent<float> _onFadeOut = new UnityEvent<float>();
    public UnityEvent<float> OnFadeOut => _onFadeOut;
    #endregion

    public AudioClip audioClip()
    {
        if (clips.Length == 0) return null;
        
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
        _onPlay?.Invoke(audioClip());
    }
    public void Play()
    {
        SetPitch(Pitch);
        _onPlayOneShot?.Invoke(audioClip());
    }

    public void SetPitch(float pitch) => _onPitchSet?.Invoke(pitch);
    public void SetVolume(float volume) => _onVolumeSet?.Invoke(volume);

    public void Stop() => _onAudioStop?.Invoke();

    public void FadeOut(float time) => _onFadeOut?.Invoke(time);
    public void FadeIn(float time) => _onFadeIn?.Invoke(time);

    enum SoundClipOrder
    {
        random,
        inOrder,
        reverse
    }
}

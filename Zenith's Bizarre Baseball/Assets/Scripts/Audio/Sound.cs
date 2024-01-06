using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound : MonoBehaviour
{
    public string _name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;
    [SerializeField] bool dynamicPitch = false;
    public bool loop = false;
    [SerializeField] Vector2 range = new Vector2(0.8f, 1.2f);

    [HideInInspector]
    public AudioSource source;
    public bool nowPlaying;

    public void OnPlay() {
        nowPlaying = true;

        if(range != Vector2.zero)
        {
            source.pitch = Random.Range(range.x, range.y);
        }

        if(dynamicPitch)
        {
            StartCoroutine(DynamicPitch());
        }
    }

    public void OnStop()
    {
        nowPlaying = false;
    }

    IEnumerator DynamicPitch()
    {
        while(nowPlaying)
        {
            source.pitch = Random.Range(range.x, range.y);
            yield return new WaitForSeconds(0.1f);
        }
    }
}

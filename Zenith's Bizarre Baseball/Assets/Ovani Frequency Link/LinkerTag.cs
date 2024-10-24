using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class LinkerTag : MonoBehaviour
{
    public FrequencyLink[] TargetLinks = new FrequencyLink[] { };
    [HideInInspector] public AudioSource AudioSource;
    void Start()
    {
        foreach (var link in TargetLinks)
            link.Sources.Add(this);
        AudioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (AudioSource == null)
            AudioSource = GetComponent<AudioSource>();
    }
}

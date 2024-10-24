using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(LinkerTag))]
public class TagInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Play"))
        {
            ((LinkerTag)target).GetComponent<AudioSource>().Play();
            foreach (var link in ((LinkerTag)target).TargetLinks)
                link.Sources.Add(((LinkerTag)target));
        }
        if (GUILayout.Button("Stop"))
            ((LinkerTag)target).GetComponent<AudioSource>().Stop();
        GUILayout.EndHorizontal();
    }
}

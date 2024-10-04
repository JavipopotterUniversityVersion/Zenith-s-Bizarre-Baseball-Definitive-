using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Dialogue/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string CharacterName;
    public SerializableDictionary<string, UnityEngine.Sprite> Emotions;
    public string[] ExpressionKeys => Emotions.Keys.ToArray();
    public AudioPlayer Voice;

    public UnityEngine.Sprite GetEmotion(string key)
    {
        if(Emotions.ContainsKey(key)) return Emotions[key];
        return Emotions[ExpressionKeys[0]];
    }

    #if UNITY_EDITOR
    public Texture2D GetExpression(string key)
    {
        if(Emotions.ContainsKey(key)) return AssetPreview.GetAssetPreview(Emotions[key]);
        return null;
    }
    #endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(CharacterData))]
public class CharacterDataEditor : Editor
{
    private CharacterData _characterData;

    private void OnEnable()
    {
        _characterData = (CharacterData) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(_characterData.Emotions == null) _characterData.Emotions = new SerializableDictionary<string, UnityEngine.Sprite>();

        GUILayout.Space(10);

        GUILayout.Label("Character Expressions", EditorStyles.boldLabel);

        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        foreach (var key in _characterData.ExpressionKeys)
        {
            GUILayout.BeginVertical("box");
            GUILayout.Label(key, GUILayout.Width(100));
            GUILayout.Label(_characterData.GetExpression(key), GUILayout.Width(100), GUILayout.Height(100));
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
}
#endif
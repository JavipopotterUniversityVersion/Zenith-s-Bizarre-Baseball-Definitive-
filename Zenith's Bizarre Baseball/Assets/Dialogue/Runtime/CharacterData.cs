using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Dialogue/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string CharacterName;
    public SerializableDictionary<string, UnityEngine.Sprite> CharacterExpressions;
    public string[] ExpressionKeys => CharacterExpressions.Keys.ToArray();
    public AudioPlayer CharacterVoice;

    public Texture2D GetExpression(string key)
    {
        if(CharacterExpressions.ContainsKey(key)) return AssetPreview.GetAssetPreview(CharacterExpressions[key]);
        return null;
    }
}

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

        if(_characterData.CharacterExpressions == null) _characterData.CharacterExpressions = new SerializableDictionary<string, UnityEngine.Sprite>();

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

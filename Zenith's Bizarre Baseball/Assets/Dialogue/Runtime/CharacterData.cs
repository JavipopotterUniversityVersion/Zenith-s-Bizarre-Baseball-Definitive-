using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Dialogue/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string CharacterName;
    public SerializableDictionary<string, UnityEngine.Sprite> CharacterExpressions;
    public string[] ExpressionKeys => CharacterExpressions.Keys.ToArray();
    public AudioPlayer CharacterVoice;
}

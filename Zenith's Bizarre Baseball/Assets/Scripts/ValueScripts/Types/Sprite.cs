using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Sprite", menuName = "Value/Sprite")]
public class Sprite : ScriptableObject
{
    [SerializeField] UnityEngine.Sprite _value;
    public UnityEngine.Sprite Value => _value;

    UnityEvent _onSpriteSet = new UnityEvent();
    public UnityEvent OnSpriteSet => _onSpriteSet;

    [SerializeField] SerializableDictionary<string, UnityEngine.Sprite> _spriteDictionary;
}

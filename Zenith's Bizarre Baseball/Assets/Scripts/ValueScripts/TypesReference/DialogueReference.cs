using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Reference", menuName = "Value/Dialogue")]
public class DialogueReference : ScriptableObject
{
    [SerializeField] Dialogue _value;
    public Dialogue Value => _value;

    public void SetDialogue(Dialogue _dialogue)
    {
        _value = _dialogue;
    }
}
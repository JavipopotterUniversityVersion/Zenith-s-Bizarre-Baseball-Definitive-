using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[CreateAssetMenu(fileName = "Dialogue", menuName = "DialogueSystem/Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField] Intervention[] dialogueLines;
    public Intervention[] DialogueLines => dialogueLines;

    [SerializeField] UnityEvent onDialogueEnd = new UnityEvent();
    public UnityEvent OnDialogueEnd => onDialogueEnd;

    [SerializeField] StringProcessor _stringProcessor;
    public StringProcessor StringProcessor => _stringProcessor;
}

[Serializable]
public class Intervention
{
    [SerializeField] CondEventVector _enter;
    public CondEventVector Enter => _enter;

    [TextArea(3, 10)]
    public string[] lines;
    public string[] Lines => lines;

    [SerializeField] CondEventVector _exit;
    public CondEventVector Exit => _exit;
}

[Serializable]
public struct CondEventVector
{
    [SerializeField] ScriptableCondition[] condition;
    public readonly bool CheckCondition() => ScriptableCondition.CheckAllConditions(condition);
    public readonly bool IsEmpty() => condition.Length == 0;

    [SerializeField] UnityEvent _event;
    public readonly void Invoke() => _event.Invoke();
}
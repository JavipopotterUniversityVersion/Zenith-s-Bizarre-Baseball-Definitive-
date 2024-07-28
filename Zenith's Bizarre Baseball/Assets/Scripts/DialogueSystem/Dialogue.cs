using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[CreateAssetMenu(fileName = "Dialogue", menuName = "DialogueSystem/Dialogue")]
public class Dialogue : ScriptableObject, IDialogue
{
    [SerializeField] Intervention[] dialogueLines;
    public Intervention[] DialogueLines => dialogueLines;

    [SerializeField] UnityEvent onDialogueEnd = new UnityEvent();
    public UnityEvent OnDialogueEnd => onDialogueEnd;

    [SerializeField] StringProcessor _stringProcessor;
    public StringProcessor StringProcessor => _stringProcessor;

    [SerializeField] DialogueOption[] options;
    public DialogueOption[] Options => options;
}

[Serializable]
public class DialogueClass : IDialogue
{
    [SerializeField] Intervention[] _dialogueLines;
    public Intervention[] DialogueLines => _dialogueLines;

    [SerializeField] UnityEvent _onDialogueEnd = new UnityEvent();
    public UnityEvent OnDialogueEnd => _onDialogueEnd;

    [SerializeField] StringProcessor _stringProcessor;
    public StringProcessor StringProcessor => _stringProcessor;

    [SerializeField] DialogueOption[] _options;
    public DialogueOption[] Options => _options;
}

public interface IDialogue
{
    Intervention[] DialogueLines { get; }
    UnityEvent OnDialogueEnd { get; }
    StringProcessor StringProcessor { get; }
    DialogueOption[] Options { get; }
}

[Serializable]
public class Intervention
{
    [SerializeField] Processor interventionCondition;
    public bool CanEnter => interventionCondition.ResultBool();

    [TextArea(3, 10)]
    public string[] lines;
    public string[] Lines => lines;

    [SerializeField] ScriptableCondition[] exitConditions;
    public bool noExitConditions => exitConditions.Length == 0;
    public bool canExitIntervention => ScriptableCondition.CheckAllConditions(exitConditions);
}
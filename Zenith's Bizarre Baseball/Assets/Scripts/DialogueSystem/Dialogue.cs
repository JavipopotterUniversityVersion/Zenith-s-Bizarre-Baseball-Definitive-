using System;
using MyBox;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Dialogue", menuName = "DialogueSystem/Dialogue")]
public class Dialogue : ScriptableObject, IDialogue
{
    [SerializeField] Intervention[] dialogueLines;
    public Intervention[] DialogueLines => dialogueLines;

    [SerializeField] DialogueOption[] options;
    public DialogueOption[] Options => options;

    [Space(10)]
    [SerializeField] UnityEvent onDialogueEnd = new UnityEvent();
    public UnityEvent OnDialogueEnd => onDialogueEnd;

    [SerializeField] StringProcessor _stringProcessor;
    public StringProcessor StringProcessor => _stringProcessor;
}

[Serializable]
public class DialogueClass : IDialogue
{
    [SerializeField] Intervention[] _dialogueLines;
    public Intervention[] DialogueLines => _dialogueLines;

    [SerializeField] DialogueOption[] _options;
    public DialogueOption[] Options => _options;

    [SerializeField] StringProcessor _stringProcessor;
    public StringProcessor StringProcessor => _stringProcessor;

    [SerializeField] UnityEvent _onDialogueEnd = new UnityEvent();
    public UnityEvent OnDialogueEnd => _onDialogueEnd;
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
    public bool background;
    [ConditionalField(nameof(background), false)] public UnityEngine.Sprite backgroundSprite;

    [SerializeField] Processor interventionCondition;
    public bool CanEnter => interventionCondition.ResultBool();

    [TextArea(3, 10)]
    public string[] lines;
    public string[] Lines => lines;

    [SerializeField] ScriptableCondition[] exitConditions;
    public bool noExitConditions => exitConditions.Length == 0;
    public bool canExitIntervention => ScriptableCondition.CheckAllConditions(exitConditions);
}
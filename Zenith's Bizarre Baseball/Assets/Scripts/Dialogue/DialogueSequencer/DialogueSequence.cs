using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DialogueSequence", menuName = "Dialogue/DialogueSequence")]
public class DialogueSequence : ScriptableObject, IReadable
{
    [SerializeField] SaveSet _persistentData;
    [SerializeField] List<SequenceElement> dialogues = new List<SequenceElement>();
    UnityEvent onDialogueAdded = new UnityEvent();
    public UnityEvent OnDialogueAdded => onDialogueAdded;
    bool _autoRemove = true;
    public int Count => dialogues.Count;
    [SerializeField] Processor _processor;

    public bool ContainsDialogue(DialogueContainer dialogue)
    {
        return dialogues.Exists(x => x.dialogue == dialogue);
    }

    public void RemoveDialogue(DialogueContainer dialogue)
    {
        int index = dialogues.FindIndex(x => x.dialogue == dialogue);
        if(index != -1) dialogues.RemoveAt(index);
        _persistentData.SaveDialogueSequences();
    }

    public void AddDialogue(DialogueContainer dialogue) => AddDialogue(new SequenceElement(dialogue, _autoRemove));
    public void AddDialogue(SequenceElementAsset sequenceElement) => AddDialogue(sequenceElement.sequenceElement);
    public void AddDialogue(SequenceElement sequenceElement)
    {
        if(ContainsDialogue(sequenceElement.dialogue)) return;

        dialogues.Add(sequenceElement);
        _persistentData.SaveDialogueSequences();
        onDialogueAdded.Invoke();
    }

    public void AddToFirst(DialogueContainer dialogue) => AddToFirst(new SequenceElement(dialogue, _autoRemove));
    public void AddToFirst(SequenceElementAsset sequenceElement) => AddToFirst(sequenceElement.sequenceElement);
    public void AddToFirst(SequenceElement sequenceElement)
    {
        if(ContainsDialogue(sequenceElement.dialogue)) return;

        dialogues.Insert(0, sequenceElement);
        _persistentData.SaveDialogueSequences();
        onDialogueAdded.Invoke();
    }

    public DialogueContainer GetDialogue(int index)
    {
        index = Mathf.Clamp(index, 0, dialogues.Count - 1);
        if(_processor.ResultOf(dialogues[index].condition, 1) == 0) index--;

        DialogueContainer dialogue = dialogues[index].dialogue;
        if(dialogues[index].autoRemove) dialogues.Remove(dialogues[index]);

        return dialogue;
    }

    public void SetAutoRemove(bool autoRemove) => _autoRemove = autoRemove;

    public float Read() => Count;
    public void ResetData() => dialogues.Clear();
}

[Serializable]
public struct SequenceElement
{
    public DialogueContainer dialogue;
    public bool autoRemove;
    public string condition;

    public SequenceElement(DialogueContainer dialogue, bool autoRemove = false, string condition = "")
    {
        this.dialogue = dialogue;
        this.autoRemove = autoRemove;
        this.condition = condition;
    }
}
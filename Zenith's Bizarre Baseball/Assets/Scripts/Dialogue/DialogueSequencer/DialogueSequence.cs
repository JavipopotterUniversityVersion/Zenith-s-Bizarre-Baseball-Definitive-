using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSequence", menuName = "Dialogue/DialogueSequence")]
public class DialogueSequence : ScriptableObject
{
    [SerializeField] SaveSet _persistentData;
    [SerializeField] List<SequenceElement> dialogues = new List<SequenceElement>();
    bool _autoRemove = true;
    public int Count => dialogues.Count;
    [SerializeField] Processor _processor;

    public void AddDialogue(DialogueContainer dialogue)
    {
        dialogues.Add(new SequenceElement(dialogue, _autoRemove));
        _persistentData.SaveDialogueSequences();
    }
    public void AddToFirst(DialogueContainer dialogue)
    {
        dialogues.Insert(0, new SequenceElement(dialogue, _autoRemove));
        _persistentData.SaveDialogueSequences();
    }

    public void RemoveDialogue(DialogueContainer dialogue)
    {
        int index = dialogues.FindIndex(x => x.dialogue == dialogue);
        if(index != -1) dialogues.RemoveAt(index);
        _persistentData.SaveDialogueSequences();
    }

    public void AddDialogue(SequenceElementAsset sequenceElement)
    {
        dialogues.Add(sequenceElement.sequenceElement);
        _persistentData.SaveDialogueSequences();
    }

    public void AddToFirst(SequenceElementAsset sequenceElement)
    {
        dialogues.Insert(0, sequenceElement.sequenceElement);
        _persistentData.SaveDialogueSequences();
    }

    public DialogueContainer GetDialogue(int index)
    {
        if(_processor.ResultOf(dialogues[index].condition, 1) == 0) index--;
        index = Mathf.Clamp(index, 0, dialogues.Count - 1);

        DialogueContainer dialogue = dialogues[index].dialogue;
        if(dialogues[index].autoRemove) dialogues.Remove(dialogues[index]);

        return dialogue;
    }

    public void SetAutoRemove(bool autoRemove) => _autoRemove = autoRemove;
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
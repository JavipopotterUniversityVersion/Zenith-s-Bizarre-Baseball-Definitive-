using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSequence", menuName = "Dialogue/DialogueSequence")]
public class DialogueSequence : ScriptableObject
{
    [SerializeField] List<SequenceElement> dialogues = new List<SequenceElement>();
    bool _autoRemove = true;
    public int Count => dialogues.Count;

    public void AddDialogue(DialogueContainer dialogue) => dialogues.Add(new SequenceElement(dialogue, _autoRemove));
    public void AddToFirst(DialogueContainer dialogue) => dialogues.Insert(0, new SequenceElement(dialogue, _autoRemove));

    public void RemoveDialogue(DialogueContainer dialogue)
    {
        int index = dialogues.FindIndex(x => x.dialogue == dialogue);
        if(index != -1) dialogues.RemoveAt(index);
    }

    public DialogueContainer GetDialogue(int index)
    {
        index = Mathf.Clamp(index, 0, dialogues.Count - 1);

        DialogueContainer dialogue = dialogues[index].dialogue;
        if(dialogues[index].autoRemove) dialogues.Remove(dialogues[index]);

        return dialogue;
    }

    public void SetAutoRemove(bool autoRemove) => _autoRemove = autoRemove;

    [Serializable]
    struct SequenceElement
    {
        public DialogueContainer dialogue;
        public bool autoRemove;

        public SequenceElement(DialogueContainer dialogue, bool autoRemove = false) => (this.dialogue, this.autoRemove) = (dialogue, autoRemove);
    }
}

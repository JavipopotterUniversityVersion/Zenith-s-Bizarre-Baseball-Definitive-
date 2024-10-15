using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceHasDialogueCondition : MonoBehaviour, ICondition, IReadable
{
    [SerializeField] DialogueSequence _sequenceToCheck;
    [SerializeField] DialogueContainer _dialogueToCheck;

    public bool CheckCondition() => _sequenceToCheck.ContainsDialogue(_dialogueToCheck);
    public float Read() => CheckCondition() ? 1 : 0;

    private void OnValidate() {
        if(_sequenceToCheck == null || _dialogueToCheck == null) return;

        name = $"{_sequenceToCheck.name} has {_dialogueToCheck.name}";
    }
}

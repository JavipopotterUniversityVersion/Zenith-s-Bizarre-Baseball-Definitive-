using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueSequencer : MonoBehaviour
{
    [SerializeField] UnityEvent<DialogueContainer> _onGetDialogue = new UnityEvent<DialogueContainer>();
    [SerializeField] UnityEvent _onNoMoreDialogue = new UnityEvent();
    [SerializeField] DialogueSequence _dialogueSequence;
    [SerializeField] UnityEvent _onDialogueAdded = new UnityEvent();

    int _currentDialogueIndex = 0;

    private void Awake() {
        _dialogueSequence.OnDialogueAdded.AddListener(() => _onDialogueAdded.Invoke());
    }

    private void OnDestroy() {
        _dialogueSequence.OnDialogueAdded.RemoveListener(() => _onDialogueAdded.Invoke());
    }

    public void GetNextDialogue()
    {
        DialogueContainer dialogue = _dialogueSequence.GetDialogue(_currentDialogueIndex);
        if(dialogue != null) _onGetDialogue.Invoke(dialogue);
        _currentDialogueIndex++;

        if(_currentDialogueIndex >= _dialogueSequence.Count || dialogue == null) _onNoMoreDialogue.Invoke();
    }

    public void SetIndex(int index) => _currentDialogueIndex = index;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DialogueCaster", menuName = "DialogueSystem/DialogueCaster")]
public class DialogueCaster : ScriptableObject
{
    UnityEvent<Dialogue> onDialogueCast = new UnityEvent<Dialogue>();
    public UnityEvent<Dialogue> OnDialogueCast => onDialogueCast;

    public UnityEvent onStopDialogue = new UnityEvent();
    public UnityEvent OnStopDialogue => onStopDialogue;

    public UnityEvent<DialogueContainer> onStartDialogue = new UnityEvent<DialogueContainer>();

    public void StartDialogue(DialogueContainer dialogue) => onStartDialogue.Invoke(dialogue);

    public void CastDialogue(Dialogue dialogue) => onDialogueCast.Invoke(dialogue);
    public void CastDialogue(DialogueReference dialogueReference) => onDialogueCast.Invoke(dialogueReference.Value);
    public void StopDialogue() => onStopDialogue.Invoke();
}

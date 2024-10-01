using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [CreateAssetMenu(fileName = "New Dialogue Reference", menuName = "Dialogue/DialogueReference")]
public class DialogueContainerReference : ScriptableObject
{
    [SerializeField] DialogueContainer _dialogueContainer;
    public DialogueContainer Dialogue => _dialogueContainer;

    public void SetDialogue(DialogueContainer dialogue) => _dialogueContainer = dialogue;
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DialogueInterpreter : MonoBehaviour
{
    [SerializeField] GameObject _dialogueCanvas;
    [SerializeField] TextMeshProUGUI _dialogueText;
    [SerializeField] Float _timeBetweenCharacters;
    [SerializeField] DialogueCaster _dialogueCaster;

    [Header("Events")]
    [SerializeField] UnityEvent onDialogueStart = new UnityEvent();
    [SerializeField] UnityEvent onCharWrite = new UnityEvent();
    [SerializeField] UnityEvent onDialogueEnd = new UnityEvent();


    private void Awake() {
        _dialogueCaster.OnDialogueCast.AddListener(StartDialogue);
    }

    private void OnDestroy() {
        _dialogueCaster.OnDialogueCast.RemoveListener(StartDialogue);
    }

    public void StartDialogue(DialogueReference dialogueReference)
    {
        StartDialogue(dialogueReference.Value);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        StopAllCoroutines();
        _dialogueCanvas.SetActive(true);
        onDialogueStart.Invoke();

        StartCoroutine(InterpretDialogue(dialogue));
    }

    IEnumerator InterpretDialogue(Dialogue dialogue)
    {
        foreach (Intervention intervention in dialogue.DialogueLines)
        {
            if(intervention.Enter.CheckCondition() == false) continue;
            intervention.Enter.Invoke();

            foreach (string dialogueLine in intervention.Lines)
            {
                _dialogueText.text = dialogueLine;
                _dialogueText.maxVisibleCharacters = 0;

                for(int i = 0; i < dialogueLine.Length; i++)
                {
                    _dialogueText.maxVisibleCharacters = i + 1;
                    onCharWrite.Invoke();
                    yield return new WaitForSeconds(_timeBetweenCharacters.Value);
                }

                intervention.Exit.Invoke();
                while(!intervention.Exit.CheckCondition()) yield return null;
            }
        }

        dialogue.OnDialogueEnd.Invoke();
        onDialogueEnd.Invoke();
        _dialogueCanvas.SetActive(false);
    }
}

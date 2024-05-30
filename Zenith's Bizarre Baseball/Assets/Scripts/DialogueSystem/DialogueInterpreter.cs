using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    [SerializeField] UnityEvent onInterventionStart = new UnityEvent();
    [SerializeField] UnityEvent onCharWrite = new UnityEvent();
    [SerializeField] UnityEvent onInterventionEnd = new UnityEvent();
    [SerializeField] UnityEvent onDialogueEnd = new UnityEvent();

    [SerializeField] InputActionReference _nextLineInput;

    bool _next;
    bool next
    {
        get
        {
            bool value = _next;
            _next = false;
            return value;
        }
    }


    private void Awake() {
        _dialogueCaster.OnDialogueCast.AddListener(StartDialogue);
        _nextLineInput.action.Enable();
        _nextLineInput.action.performed += Next;
    }

    private void OnDestroy() {
        _dialogueCaster.OnDialogueCast.RemoveListener(StartDialogue);
        _nextLineInput.action.Disable();
        _nextLineInput.action.performed -= Next;
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
            onInterventionStart.Invoke();
            intervention.Enter.Invoke();

            if(intervention.lines.Length == 0)
            {
                intervention.Exit.Invoke();
                onInterventionEnd.Invoke();

                if(intervention.Exit.IsEmpty())
                {
                    while(!Input.GetButtonDown("NextLine")) yield return null;
                }
                else while(!intervention.Exit.CheckCondition()) yield return new WaitForEndOfFrame();
            }

            foreach (string dialogueLine in intervention.Lines)
            {
                _dialogueText.text = dialogue.StringProcessor.Process(dialogueLine);
                _dialogueText.maxVisibleCharacters = 0;

                for(int i = 0; i < dialogueLine.Length; i++)
                {
                    _dialogueText.maxVisibleCharacters = i + 1;
                    onCharWrite.Invoke();

                    yield return new WaitForSecondsRealtime(_timeBetweenCharacters.Value);

                    if(next)
                    {
                        _dialogueText.maxVisibleCharacters = dialogueLine.Length;
                        break;
                    }
                }

                intervention.Exit.Invoke();
                onInterventionEnd.Invoke();

                if(intervention.Exit.IsEmpty())
                {
                    while(!next) yield return null;
                    yield return new WaitForEndOfFrame();
                }
                else while(!intervention.Exit.CheckCondition()) yield return new WaitForEndOfFrame();
            }
        }

        dialogue.OnDialogueEnd.Invoke();
        onDialogueEnd.Invoke();
        _dialogueCanvas.SetActive(false);
    }

    void Next(InputAction.CallbackContext context)
    {
        _next = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Linq;

public class DialogueInterpreter : MonoBehaviour
{
    [SerializeField] GameObject _dialogueCanvas;
    [SerializeField] TextMeshProUGUI _dialogueText;
    [SerializeField] Float _timeBetweenCharacters;
    [SerializeField] DialogueCaster _dialogueCaster;
    [SerializeField] AudioPlayer _charWriteSound;

    [Header("Events")]
    [SerializeField] UnityEvent onDialogueStart = new UnityEvent();
    [SerializeField] UnityEvent onInterventionStart = new UnityEvent();
    [SerializeField] UnityEvent onInterventionEnd = new UnityEvent();
    [SerializeField] UnityEvent onDialogueEnd = new UnityEvent();

    [SerializeField] InputActionReference _nextLineInput;
    [SerializeField] SerializableDictionary<string, String> _stringDictionary;
    [SerializeField] SerializableDictionary<string, Sprite> _spriteDictionary;
    [SerializeField] SerializableDictionary<string, Float> _floatDictionary;
    [SerializeField] SerializableDictionary<string, AudioPlayer> _audioDictionary;

    bool _next = false;
    bool next
    {
        set => _next = value;
        get
        {
            bool value = _next;
            _next = false;
            return value;
        }
    }


    private void Awake() {
        _dialogueCaster.OnDialogueCast.AddListener(StartDialogue);
        _dialogueCaster.onStopDialogue.AddListener(StopDialogue);
        _nextLineInput.action.Enable();
    }

    private void OnDestroy() {
        _dialogueCaster.OnDialogueCast.RemoveListener(StartDialogue);
        _dialogueCaster.onStopDialogue.RemoveListener(StopDialogue);
        _nextLineInput.action.Disable();
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

    public void StopDialogue()
    {
        StopAllCoroutines();
        _dialogueCanvas.SetActive(false);
    }

    IEnumerator InterpretDialogue(Dialogue dialogue)
    {
        yield return new WaitForEndOfFrame();
        _nextLineInput.action.performed += Next;
        
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

                bool skip = false;

                for(int i = 0; i < _dialogueText.text.Length; i++)
                {
                    if(_dialogueText.text[i] == '<')
                    {
                        string input = _dialogueText.text.Split('<')[1].Split('>')[0];

                        if(input == "break")
                        {
                            next = true;
                            break;
                        }

                        bool found = SearchSpriteSet(input) || SearchStringSet(input) || SearchFloatSet(input) || SearchSound(input) || SearchVoice(input);

                        if(!found) Debug.LogWarning($"Input {input} not found, did you pretend to ignore it?");

                        _dialogueText.text = _dialogueText.text.Replace($"<{input}>", "");
                        i--;

                        continue;
                    }

                    _dialogueText.maxVisibleCharacters = i + 1;
                    if(!skip) _charWriteSound.Play();

                    if(next) skip = true;

                    if(!skip) yield return new WaitForSecondsRealtime(_timeBetweenCharacters.Value);
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

        _nextLineInput.action.performed -= Next;
    }

    void Next(InputAction.CallbackContext context)
    {
        _next = true;
    }

    bool SearchSpriteSet(string input)
    {
        bool found = false;

        string[] keys = input.Split(":")[0].Split(",").Select(x => x.Trim()).ToArray();
        string value = input.Split(":")[1].Trim();

        foreach(string key in keys)
        {
            if(_spriteDictionary.ContainsKey(key))
            {
                _spriteDictionary[key].SetSprite(value);
                found = true;
            }
        }

        return found;
    }

    bool SearchStringSet(string input)
    {
        string key = input.Split(":")[0].Trim();
        string[] values = input.Split(":")[1].Split(",").Select(x => x.Trim()).ToArray();

        if(values.Length == 0) 
        {
            _stringDictionary[key].SetString("");
            return true;
        }

        if(_stringDictionary.ContainsKey(key))
        {
            values.ToList().ForEach(x => _stringDictionary[key].SetString(x));
            return true;
        }

        return false;
    }

    bool SearchFloatSet(string input)
    {
        string key = input.Split(":")[0].Trim();
        string value = input.Split(":")[1].Trim();

        if(_floatDictionary.ContainsKey(key))
        {
            _floatDictionary[key].SetValue(float.Parse(value));
            return true;
        }

        return false;
    }

    bool SearchSound(string input)
    {
        if(_audioDictionary.ContainsKey(input))
        {
            _audioDictionary[input].Play();
            return true;
        }

        return false;
    }

    bool SearchVoice(string input)
    {
        string key = input.Split(":")[0].Trim();
        string value = input.Split(":")[1].Trim();

        if(key == "voice")
        {
            if(_audioDictionary.ContainsKey(value))
            {
                _charWriteSound = _audioDictionary[value];
                return true;
            }
        }
        return false;
    }
}

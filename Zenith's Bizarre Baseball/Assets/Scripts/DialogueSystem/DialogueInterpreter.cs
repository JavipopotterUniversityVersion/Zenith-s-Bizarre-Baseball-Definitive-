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

    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] TextMeshProUGUI _dialogueText;

    [SerializeField] Float _timeBetweenCharacters;
    [SerializeField] DialogueCaster _dialogueCaster;
    [SerializeField] AudioPlayer _charWriteSound;

    [SerializeField] Sprite _backgroundSprite;

    [Header("Events")]
    [SerializeField] UnityEvent onDialogueStart = new UnityEvent();
    [SerializeField] UnityEvent onDialogueEnd = new UnityEvent();

    [SerializeField] InputActionReference _nextLineInput;
    [SerializeField] SerializableDictionary<string, Float> _floatDictionary;
    [SerializeField] SerializableDictionary<string, AudioPlayer> _audioDictionary;

    [SerializeField] CharData[] _characterDatas;

    Character[] _characters;
    DialogueOptionReceiver[] dialogueOptionReceivers;

    Character _lastCharacter;
    Character LastCharacter
    {
        get => _lastCharacter;
        set => _lastCharacter = value;
    }

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
        _characters = GetComponentsInChildren<Character>(true);
        dialogueOptionReceivers = GetComponentsInChildren<DialogueOptionReceiver>(true);
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

    public void StartDialogue(Dialogue dialogue) => StartDialogue(dialogue as IDialogue);

    public void StopDialogue()
    {
        StopAllCoroutines();
        _dialogueCanvas.SetActive(false);
    }

    public void StartDialogue(IDialogue dialogue)
    {
        StopAllCoroutines();

        DeactivateOptions();
        ClearAllCharacters();
        
        _dialogueCanvas.SetActive(true);
        onDialogueStart.Invoke();

        StartCoroutine(InterpretDialogue(dialogue));
    }

    IEnumerator InterpretDialogue(IDialogue dialogue)
    {
        yield return new WaitForEndOfFrame();
        _nextLineInput.action.performed += Next;

        dialogue.OnDialogueStart.Invoke();
        
        foreach (Intervention intervention in dialogue.DialogueLines)
        {
            if(intervention.CanEnter == false) continue;

            _backgroundSprite.SetSprite(intervention.backgroundSprite);

            if(intervention.lines.Length == 0)
            {
                if(intervention.noExitConditions == true)
                {
                    while(!Input.GetButtonDown("NextLine")) yield return null;
                }
                else while(!intervention.canExitIntervention) yield return new WaitForEndOfFrame();
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

                        bool found = SearchShortcut(_dialogueText, input, ref i) || SearchCharacter(input) || SearchFloatSet(input) || SearchSound(input) || SearchVoice(input);

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

                if(intervention.noExitConditions == true)
                {
                    while(!next) yield return null;
                    yield return new WaitForEndOfFrame();
                }
                else while(!intervention.canExitIntervention) yield return new WaitForEndOfFrame();
            }
        }

        dialogue.OnDialogueEnd.Invoke();

        _nextLineInput.action.performed -= Next;

        if(!SetOptions(dialogue.Options))
        {
            _dialogueCanvas.SetActive(false);
            onDialogueEnd.Invoke();
        }
    }

    bool SetOptions(DialogueOption[] options)
    {
        if(options.Length == 0) return false;

        int receiverIndex = 0;
        for(int i = 0; i < options.Length; i++)
        {
            if(options[i].appearCondition.ResultBool())
            {
                dialogueOptionReceivers[receiverIndex].ReceiveOption(options[i], this);
                receiverIndex++;
            }
        }

        return true;
    }

    public void DeactivateOptions()
    {
        foreach(DialogueOptionReceiver receiver in dialogueOptionReceivers) receiver.gameObject.SetActive(false);
    }

    void Next(InputAction.CallbackContext context)
    {
        _next = true;
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

    bool SearchShortcut(TextMeshProUGUI text, string input, ref int i)
    {
        if(CharData.SearchCharacter(_characterDatas, input, out CharData characterData))
        {
            string fadeLast = "";
            if(LastCharacter != null && LastCharacter.CurrentCharacter != null)
            {
                if(LastCharacter.CurrentCharacter.name != input)
                {
                    fadeLast = $"<{LastCharacter.CurrentCharacter.name}:an_Fade>";
                }
            }

            text.text = fadeLast + $"<{input}:an_Talk_Appear / default>" + text.text.Split($"<{input}>")[1] + $"<{input}:an_Idle>";
            i = 0;
            return true;
        }

        return false;
    }

    bool SearchCharacter(string input)
    {
        string name;
        string[] commands;

        if(input.Contains(":") == false) 
        {
            name = LastCharacter.CurrentCharacter.name;
            commands = input.Split('/');
        }
        else
        {
            name = input.Split(":")[0].Trim();
            commands = input.Split(":")[1].Trim().Split('/');
        }

        if(CharData.SearchCharacter(_characterDatas, name, out CharData characterData))
        {
            Character character = Character.SetFirstFreeCharacter(_characters, characterData);

            foreach(string command in commands) InterpretCommand(command, character);

            _nameText.text = characterData.name;
            _nameText.color = characterData.NameColor;
            _dialogueText.color = characterData.DialogueColor;
            _charWriteSound = characterData.voice;

            LastCharacter = character;
            return true;
        }
        return false;
    }

    public void InterpretCommand(string command, Character character)
    {
        command = command.Trim();
        string[] commandParts = command.Split('_');
        commandParts[0] = commandParts[0].ToLower();

        switch(commandParts[0])
        {
            case "an":
                for(int i = 1; i < commandParts.Length; i++) character.SetAnimation(commandParts[i]);
                break;
            case "icon":
                character.SetSprite(commandParts[1]);
                break;
            case "xpos":
                float y = commandParts.Length == 3 ? float.Parse(commandParts[2]) : 0;
                character.SetRectX(float.Parse(commandParts[1]), y);
                break;
            case "disable":
                character.DisableCharacter();
                _lastCharacter = _characters.First(c => c.Occupied);
                break;
            default:
                character.SetSprite(commandParts[0]);
                break;
        }
    }

    void ClearAllCharacters()
    {
        foreach(Character character in _characters) character.DisableCharacter();
    }
}

[System.Serializable]
public class CharData
{
    public string name;
    public AudioPlayer voice;
    [SerializeField] SerializableDictionary<string, UnityEngine.Sprite> spriteSheet;
    public bool selected;

    [SerializeField] Color _nameColor = Color.white;
    public Color NameColor => _nameColor;

    [SerializeField] Color _dialogueColor = Color.white;
    public Color DialogueColor => _dialogueColor;

    public UnityEngine.Sprite GetSprite(string name) => spriteSheet[name.ToLower()];

    public static bool SearchCharacter(CharData[] characters, string name, out CharData character)
    {
        int i = 0;
        while(i < characters.Length - 1 && characters[i].name != name) i++;
        if(characters[i].name == name) character = characters[i];
        else character = new CharData();

        return characters[i].name == name;
    }
}

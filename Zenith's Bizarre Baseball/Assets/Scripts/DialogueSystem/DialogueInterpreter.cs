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

    [Header("Events")]
    [SerializeField] UnityEvent onDialogueStart = new UnityEvent();
    [SerializeField] UnityEvent onDialogueEnd = new UnityEvent();

    [SerializeField] InputActionReference _nextLineInput;
    [SerializeField] SerializableDictionary<string, Float> _floatDictionary;
    [SerializeField] SerializableDictionary<string, AudioPlayer> _audioDictionary;

    [SerializeField] Character[] _characters;
    [SerializeField] CharacterData[] _characterDatas;

    DialogueOptionReceiver[] dialogueOptionReceivers;

    Character _lastCharacter;

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
        dialogueOptionReceivers = GetComponentsInChildren<DialogueOptionReceiver>();
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
        
        foreach (Intervention intervention in dialogue.DialogueLines)
        {
            if(intervention.CanEnter == false) continue;

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

                        bool found = SearchCharacter(input) || SearchFloatSet(input) || SearchSound(input) || SearchVoice(input);

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
        onDialogueEnd.Invoke();
        _dialogueCanvas.SetActive(false);

        _nextLineInput.action.performed -= Next;

        SetOptions(dialogue.Options);
    }

    void SetOptions(DialogueOption[] options)
    {
        if(options.Length == 0) return;

        int receiverIndex = 0;
        for(int i = 0; i < options.Length; i++)
        {
            if(options[i].appearCondition.ResultBool())
            {
                dialogueOptionReceivers[receiverIndex].ReceiveOption(options[i], this);
                receiverIndex++;
            }
        }
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

    bool SearchCharacter(string input)
    {
        string name;
        string[] commands;

        if(input.Contains(":") == false) 
        {
            name = _lastCharacter.CurrentCharacter.name;
            commands = input.Split('/');
        }
        else
        {
            name = input.Split(":")[0].Trim();
            commands = input.Split(":")[1].Trim().Split('/');
        }

        if(CharacterData.SearchCharacter(_characterDatas, name, out CharacterData characterData))
        {
            Character character = Character.SetFirstFreeCharacter(_characters, characterData);

            foreach(string command in commands) InterpretCommand(command, character);

            _nameText.text = characterData.name;
            _charWriteSound = characterData.voice;

            _lastCharacter = character;
            return true;
        }
        return false;
    }

    public void InterpretCommand(string command, Character character)
    {
        command = command.Trim();
        string[] commandParts = command.Split('_');
        commandParts[0] = commandParts[0].ToLower();
        print("Interpreting command: " + command);

        switch(commandParts[0])
        {
            case "an":
                character.SetAnimation(commandParts[1]);
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
                Debug.LogWarning($"Command {commandParts[0]} not found");
                break;
        }
    }

    void ClearAllCharacters()
    {
        foreach(Character character in _characters) character.DisableCharacter();
    }
}

[System.Serializable]
public class CharacterData
{
    public string name;
    public AudioPlayer voice;
    [SerializeField] SerializableDictionary<string, UnityEngine.Sprite> spriteSheet;
    public bool selected;

    public UnityEngine.Sprite GetSprite(string name) => spriteSheet[name.ToLower()];

    public static bool SearchCharacter(CharacterData[] characters, string name, out CharacterData character)
    {
        int i = 0;
        while(i < characters.Length && characters[i].name != name) i++;
        
        if(characters[i].name == name) character = characters[i];
        else character = new CharacterData();

        return characters[i].name == name;
    }
}

using System.Collections;
using System.Collections.Generic;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.Events;

public class Interpreter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] TextMeshProUGUI _dialogueText;
    [SerializeField] Image _backgroundImage;
    [SerializeField] CharacterRefs[] _characters = new CharacterRefs[2];
    [SerializeField] InputActionReference _nextAction;
    [SerializeField] InputActionReference _skipAction;
    [SerializeField] GameObject _canvas;
    [SerializeField] ObjectProcessor _processor;
    [SerializeField] StringProcessor _stringRefs;
    [SerializeField] DialogueCaster _caster;

    Option[] _options;
    int lineIndex = 0;

    int _lastCharacterIndex = 0;
    CharacterRefs _currentCharacter => _characters[_lastCharacterIndex];
    bool _waitForOption = false;
    bool WaitForOption
    {
        get => _waitForOption;
        set
        {
            if(value) _nextAction.action.Disable();
            else _nextAction.action.Enable();

            _waitForOption = value;
        }
    }
    bool _next;
    bool _skip;

    [SerializeField] UnityEvent onStartDialogue;
    [SerializeField] UnityEvent onEndDialogue;
    [SerializeField] AudioPlayer _beepSound;

    private void Awake() 
    {
        _caster.onStartDialogue.AddListener(InterpretDialogue);
        _options = GetComponentsInChildren<Option>(true);
        _options.ForEach(o => o.Initialize(this));
    }

    private void OnDestroy() {
        _caster.onStartDialogue.RemoveListener(InterpretDialogue);
    }

    public void InterpretDialogue(DialogueContainer dialogue)
    {
        onStartDialogue.Invoke();
        StopAllCoroutines();
        StartCoroutine(InterpretDialogueRoutine(dialogue));
    }

    public void InterpretDialogue(DialogueContainerReference containerReference)
    {
        InterpretDialogue(containerReference.Dialogue);
    }

    IEnumerator InterpretDialogueRoutine(DialogueContainer dialogue)
    {
        string translation = dialogue.Translation;
        string[] lines = translation.Split('@');

        lineIndex = 0;
        _lastCharacterIndex = 0;

        bool endDialogue = false;
        _canvas.SetActive(true);

        _skip = false;

        _nextAction.action.Enable();
        _nextAction.action.performed += Next;
        _skipAction.action.Enable();
        _skipAction.action.performed += Skip;

        while (lineIndex < lines.Length && !endDialogue)
        {
            string line = lines[lineIndex];

            _dialogueText.text = line;
            _dialogueText.maxVisibleCharacters = 0;

            int charIndex = 0;

            while(charIndex < _dialogueText.text.Length && !endDialogue)
            {
                CheckCommands(ref charIndex, ref endDialogue, ref lineIndex, lines);

                if(CheckNext(_next))
                {
                    _dialogueText.maxVisibleCharacters = _dialogueText.text.Length;
                    while(charIndex < _dialogueText.text.Length)
                    {
                        CheckCommands(ref charIndex, ref endDialogue, ref lineIndex, lines);
                        charIndex++;
                    }
                    break;
                }
                else 
                charIndex++;

                _beepSound.Play();

                _dialogueText.maxVisibleCharacters = charIndex;

                if(!_skip) yield return new WaitForSeconds(0.02f);
                else yield return new WaitForEndOfFrame();
            }

            if(WaitForOption) while(WaitForOption) yield return null;
            else while(!CheckNext(_next) && !_skip) yield return null;

            lineIndex++;
        }

        _nextAction.action.performed -= Next;
        _nextAction.action.Disable();
        _skipAction.action.performed -= Next;
        _skipAction.action.Disable();

        _canvas.SetActive(false);

        onEndDialogue.Invoke();
    }

    void Next(InputAction.CallbackContext context) => _next = true;
    void Skip(InputAction.CallbackContext context)
    {
        _skip = true;
        _next = false;
    }

    bool CheckNext(bool next)
    {
        if (next)
        {
            _next = false;
            return true;
        }

        return false;
    }

    void CheckCommands(ref int index, ref bool endDialogue, 
    ref int nextLineIndex, string[] lines)
    {
        if (_dialogueText.text[index] == '<')
        {
            string command = _dialogueText.text.Split("<")[1].Split(">")[0];
            _dialogueText.text = _dialogueText.text.Replace($"<{command}>", "");

            index--;

            string[] commandParts = command.Split(":");

            string commandType = commandParts[0];
            string commandValue = "";

            if(commandParts.Length > 1) commandValue = commandParts[1];

            switch (commandType)
            {
                case "NAME":
                    _nameText.text = commandValue;
                    break;
                case "BACKGROUND":
                    _backgroundImage.sprite = Resources.Load<Texture2D>($"Backgrounds/{commandValue}").AsSprite();
                    break;

                case "CHARACTER":
                    _lastCharacterIndex = (int)Enum.Parse(typeof(CharacterIndex), commandValue.Split("as")[1]);
                    CharacterData character = _currentCharacter.SetCharacter(commandValue.Split("as")[0]);
                    _nameText.text = character.CharacterName;
                    _beepSound = character.Voice;
                    break;

                case "EMOTION":
                    _currentCharacter.SetEmotion(commandValue);
                    break;

                case "ANIMATION":
                    _currentCharacter.PlayAnimation(commandValue);
                    break;

                case "CHOICE":
                    string[] options = commandValue.Split(",,");
                    SetOptions(options, lines);
                    break;

                case "GOTO":
                    CheckJump(lines, commandValue, ref nextLineIndex);
                    break;

                case "IF":
                    string condition = commandValue;
                    if(_processor.ResultOf(condition, 1) == 0) CheckJump(lines, "FALSE", ref nextLineIndex);
                    break;

                case "END":
                    endDialogue = true;
                    break;

                case "BREAK":
                    _next = true;
                    break;

                case "PROCESS":
                    _processor.ResultOf(commandValue, 1);
                    break;

                case "REF":
                    _dialogueText.text = _dialogueText.text.Replace($"<{command}>", _stringRefs.Process(commandValue));
                    break;
                default:
                    break;
            }
        }
    }

    void SetOptions(string[] options, string[] lines)
    {
        WaitForOption = true;
        int optionObjectIndex = 0;

        if(_skip) _skip = false;

        for (int i = 0; i < options.Length; i++)
        {
            string option = options[i];
            string optionText = option.Split('/')[0];
            string optionValue = option.Split('/')[1];

            if(_processor.ResultOf(optionValue, 1) == 1)
            {
                int optionLineIndex = 0;

                while(optionLineIndex < lines.Length && !lines[optionLineIndex].Contains($"<OPTION:{optionText}>")) optionLineIndex++;

                _options[optionObjectIndex].SetOption(optionText, optionLineIndex - 1);
                optionObjectIndex++;
            }
        }

        _options[0].GetComponent<CustomButton>().Select();

    }

    public void SelectOption(int index)
    {
        WaitForOption = false;
        lineIndex = index;
        _options.ForEach(o => o.gameObject.SetActive(false));
    }

    void CheckJump(string[] lines, string commandValue, ref int nextLineIndex)
    {
        int lineToCheck = 0;
        while (lineToCheck < lines.Length && !lines[lineToCheck].Contains($"<LABEL:{commandValue}>")) lineToCheck++;
        nextLineIndex = lineToCheck - 1;
    }

    [Serializable] class CharacterRefs
    {
        public CharacterData Data;
        public Image Renderer;
        public Animator Animator;

        public void SetEmotion(string key) 
        {
            if(Data.Emotions.ContainsKey(key)) Renderer.sprite = Data.Emotions[key];
        }
        public CharacterData SetCharacter(string characterName)
        {
            Data = Resources.Load<CharacterData>($"Characters/{characterName}");
            SetEmotion("");
            return Data;
        }

        public void PlayAnimation(string animation) 
        {
            Animator.ResetTrigger("Talk");
            Animator.SetTrigger(animation);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueOptionReceiver : MonoBehaviour
{
    [SerializeField] UnityEvent<string> onOptionTextReceived = new UnityEvent<string>();
    [SerializeField] UnityEvent<UnityEngine.Sprite> onOptionIconReceived = new UnityEvent<UnityEngine.Sprite>();

    public void ReceiveOption(DialogueOption option, DialogueInterpreter interpreter)
    {
        gameObject.SetActive(true);

        onOptionTextReceived.Invoke(option.optionText);
        onOptionIconReceived.Invoke(option.buttonIcon);

        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();

        if(transform.GetSiblingIndex() == 0) button.Select();
        GetComponent<Button>().onClick.AddListener(() => interpreter.StartDialogue(option.dialogue));
    }
}

[Serializable]
public class DialogueOption
{
    public string optionText;
    public UnityEngine.Sprite buttonIcon;

    public Processor appearCondition;

    public DialogueClass dialogue;
}
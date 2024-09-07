using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CustomButton))]
public class Option : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _optionText;
    Interpreter _interpreter;
    Button _button;
    int _optionIndex;

    public void Initialize(Interpreter interpreter)
    {
        _button = GetComponent<Button>();
        _interpreter = interpreter;
        _button.onClick.AddListener(() => _interpreter.SelectOption(_optionIndex));
    }

    public void SetOption(string text, int index)
    {
        gameObject.SetActive(true);
        _optionText.text = text;
        _optionIndex = index;
    }
}

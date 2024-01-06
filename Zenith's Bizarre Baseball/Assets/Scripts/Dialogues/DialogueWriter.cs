using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueWriter : MonoBehaviour {
    TextMeshProUGUI titleText;
    TextMeshProUGUI contentText;

    float _delay;
    public float delay
    {
        private get => _delay;
        set => _delay = value;
    }

    string _beepSound;
    public string beepSound
    {
        private get => _beepSound;
        set => _beepSound = value;
    }

    public void Write(string text)
    {
        StartCoroutine(_Write(text));
    }

    IEnumerator _Write(string text)
    {
        contentText.text = text;
        contentText.maxVisibleCharacters = 0;

        for (int i = 0; i < text.Length; i++)
        {
            contentText.maxVisibleCharacters++;
            yield return new WaitForSeconds(delay);
        }
    }

    public void WriteTitle(string text)
    {
        titleText.text = text;
    }
}
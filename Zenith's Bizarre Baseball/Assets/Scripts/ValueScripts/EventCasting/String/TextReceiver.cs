using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextReceiver : MonoBehaviour
{
    TMPro.TextMeshProUGUI _text;
    [SerializeField] String _string;
    [SerializeField] StringProcessor _stringProcessor;

    private void Awake()
    {
        _text = GetComponent<TMPro.TextMeshProUGUI>();
        _string.OnStringCall.AddListener(UpdateText);
        UpdateText();
    }

    public void SetString(String _string)
    {
        this._string.OnStringCall.RemoveListener(UpdateText);
        this._string = _string;
        _string.OnStringCall.AddListener(UpdateText);
        UpdateText();
    }

    private void OnEnable() => UpdateText();

    [ContextMenu("Update Text")]
    public void UpdateText() => _text.text = _stringProcessor.Process(_string.Value);

    private void OnDestroy() {
        _string.OnStringCall.RemoveListener(UpdateText);
    }
}

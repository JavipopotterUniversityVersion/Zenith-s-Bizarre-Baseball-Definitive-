using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextReceiver : MonoBehaviour
{
    TMPro.TextMeshProUGUI _text;
    [SerializeField] String _string;

    private void Awake()
    {
        _text = GetComponent<TMPro.TextMeshProUGUI>();
        _string.OnStringCall.AddListener(UpdateText);
    }

    void UpdateText() => _text.text = _string.Value;

    private void OnDestroy() {
        _string.OnStringCall.RemoveListener(UpdateText);
    }
}

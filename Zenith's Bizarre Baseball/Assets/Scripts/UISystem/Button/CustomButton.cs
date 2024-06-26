using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CustomButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    Button _button;

    [SerializeField] UnityEvent onSelect = new UnityEvent();
    [SerializeField] UnityEvent onDeselect = new UnityEvent();

    private void Awake() 
    {
        _button = GetComponent<Button>();
    }

    public void OnSelect(BaseEventData eventData) => onSelect.Invoke();

    public void OnDeselect(BaseEventData eventData) => onDeselect.Invoke();

    private void OnMouseEnter() => _button.Select();
}

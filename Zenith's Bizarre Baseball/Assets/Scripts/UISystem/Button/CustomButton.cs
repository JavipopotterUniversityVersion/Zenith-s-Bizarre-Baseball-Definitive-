using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CustomButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerClickHandler
{
    Button _button;

    [SerializeField] UnityEvent onSelect = new UnityEvent();
    [SerializeField] UnityEvent onDeselect = new UnityEvent();

    private void Awake() 
    {
        _button = GetComponent<Button>();
    }

    public void Select()
    {
        _button.Select();
        onSelect.Invoke();
    }

    public void OnSelect(BaseEventData eventData)
    {  
        onSelect.Invoke();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        onDeselect.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _button.Select();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // _button.onClick.Invoke();
    }
}

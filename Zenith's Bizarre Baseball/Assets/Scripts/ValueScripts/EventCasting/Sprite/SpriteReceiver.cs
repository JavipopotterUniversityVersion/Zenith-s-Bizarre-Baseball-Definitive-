using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SpriteReceiver : MonoBehaviour
{
    Image _image;
    [SerializeField] Sprite _sprite;
    [SerializeField] bool _updateOnSpriteSet = true;

    [SerializeField] UnityEvent _onSpriteSet = new UnityEvent();

    private void Awake()
    {
        _image = GetComponent<Image>();
         _sprite.OnSpriteSet.AddListener(OnSpriteSet);
    }

    public void UpdateSprite()
    {
        _image.sprite = _sprite.Value;
    }

    private void OnDestroy() {
        _sprite.OnSpriteSet.RemoveListener(UpdateSprite);
    }

    void OnSpriteSet()
    {
        if(_updateOnSpriteSet) UpdateSprite();
        _onSpriteSet.Invoke();
    }
}

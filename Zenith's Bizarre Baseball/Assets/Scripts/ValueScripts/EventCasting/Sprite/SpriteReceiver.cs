using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteReceiver : MonoBehaviour
{
    Image _image;
    [SerializeField] Sprite _sprite;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _sprite.OnSpriteSet.AddListener(UpdateSprite);
    }

    void UpdateSprite()
    {
        _image.sprite = _sprite.Value;
    }

    private void OnDestroy() {
        _sprite.OnSpriteSet.RemoveListener(UpdateSprite);
    }
}

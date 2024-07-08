using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RandomSprite : MonoBehaviour
{
    [SerializeField] SpriteChance[] _sprites;

    private void Awake() => SetRandonSprite();

    [ContextMenu("Set Random Sprite")]
    public void SetRandonSprite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = SpriteChance.GetRandomSprite(_sprites);
    }

    [Serializable]
    struct SpriteChance
    {
        public UnityEngine.Sprite sprite;
        [Range(0, 1)] public float chance;

        public static UnityEngine.Sprite GetRandomSprite(SpriteChance[] sprites)
        {
            float sum = sprites.Sum(s => s.chance);
            float random = UnityEngine.Random.Range(0, sum);
            float current = 0;

            foreach(SpriteChance sprite in sprites)
            {
                if(random < sprite.chance)
                {
                    return sprite.sprite;
                }
                current += sprite.chance;
            }

            return sprites[sprites.Length - 1].sprite;
        }
    }
}
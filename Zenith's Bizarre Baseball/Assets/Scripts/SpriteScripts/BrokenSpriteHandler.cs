using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;
using UnityEngine.Events;

public class BrokenSpriteHandler : MonoBehaviour
{
    Rigidbody2D[] rbs;
    SpriteRenderer[] spriteRenderers;
    [SerializeField] bool fadeOnBreak;

    [SerializeField] UnityEvent onFadeFinish;

    private void Awake() {
        rbs = GetComponentsInChildren<Rigidbody2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        rbs.ForEach(rb => rb.bodyType = RigidbodyType2D.Static);
    }

    public void BreakSprite()
    {
        foreach(var rb in rbs)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.simulated = true;
            float angle = Random.Range(0, 360);

            rb.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Random.Range(10, 20);
        }

        if(fadeOnBreak) StartCoroutine(StartFade());
    }

    IEnumerator StartFade()
    {
        float time = 0;
        while(time < 1)
        {
            time += Time.deltaTime;
            spriteRenderers.ForEach(sr => sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1 - time));
            yield return new WaitForEndOfFrame();
        }
        onFadeFinish.Invoke();
    }

    [SerializeField] UnityEngine.Sprite[] sprites;
    [ContextMenu("Create From Sprites")]
    void CreateFromSprites()
    {
        foreach(var sprite in sprites)
        {
            GameObject go = new GameObject(sprite.name);
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.AddComponent<SpriteRenderer>().sprite = sprite;
            var r = go.AddComponent<Rigidbody2D>();
            r.bodyType = RigidbodyType2D.Static;
            r.gravityScale = 0;
        }

        sprites = new UnityEngine.Sprite[0];
    }
}

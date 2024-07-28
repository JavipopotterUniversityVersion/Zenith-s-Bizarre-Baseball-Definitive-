using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    CharacterData _currentCharacter;
    public CharacterData CurrentCharacter
    {
        get => _currentCharacter;
        set
        {
            if(_currentCharacter != null) _currentCharacter.selected = false;
            _currentCharacter = value;
            if(_currentCharacter != null) _currentCharacter.selected = true;
        }
    }

    public bool Occupied => CurrentCharacter != null;
    Animator _animator;
    Image _spriteRenderer;

    public void SetAnimation(string animation) => _animator.Play(animation);
    public void SetSprite(string name)
    {
        _spriteRenderer.sprite = CurrentCharacter.GetSprite(name);
    }

    private void Awake() 
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponentInChildren<Image>();
    }

    public static Character SetFirstFreeCharacter(Character[] characters, CharacterData characterData)
    {
        int i = 0;
        while(i < characters.Length && characters[i].Occupied)
        {
            if(characters[i].CurrentCharacter == characterData) return characters[i];
            i++;
        }

        characters[i].CurrentCharacter = characterData;
        characters[i].gameObject.SetActive(true);
        return characters[i];
    }

    public void DisableCharacter()
    {
        CurrentCharacter = null;
        gameObject.SetActive(false);
    }

    public void SetRectX(float x, float time) => StartCoroutine(MoveRectX(x, time));

    IEnumerator MoveRectX(float x, float time)
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 target = new Vector2(x, rect.anchoredPosition.y);
        Vector2 start = rect.anchoredPosition;

        if(time == 0)
        {
            rect.anchoredPosition = target;
            yield break;
        }

        float t = 0;
        while(t < time)
        {
            rect.anchoredPosition = Vector2.Lerp(start, target, t / time);
            t += Time.deltaTime;
            yield return null;
        }
    }
    
}
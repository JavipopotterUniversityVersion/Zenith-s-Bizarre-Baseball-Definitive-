using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;

public class HeartHandler : MonoBehaviour
{
    const string LOST_ANIM = "Lost";
    const string RECOVER_ANIM = "Recover";
    const string BEAT_ANIM = "Beat";

    Heart[] hearts;
    Animator[] _parts;
    [SerializeField] Float _currentHealth;
    [SerializeField] Float _maxHealth;

    float lastHealth;
    float lastMaxHealth;

    private void Awake() {
        hearts = GetComponentsInChildren<Heart>(true);
        _currentHealth.OnValueChanged.AddListener(UpdateHearts);
        _maxHealth.OnValueChanged.AddListener(UpdateMaxHearts);
        _parts = new Animator[hearts.Length * 4];

        for(int i = 0; i < hearts.Length; i++)
        {
            Animator[] auxParts = hearts[i].GetParts();
            for(int c = 0; c < 4; c++)
            {
                _parts[i * 4 + c] = auxParts[c];
            }
            hearts[i].gameObject.SetActive((i + 1) <= (_maxHealth.Value/4));
        }
        lastHealth = _currentHealth.Value;
        lastMaxHealth = _maxHealth.Value;
    }

    private void OnDestroy() {
        _currentHealth.OnValueChanged.RemoveListener(UpdateHearts);
        _maxHealth.OnValueChanged.RemoveListener(UpdateMaxHearts);
    }

    void UpdateHearts()
    {
        if(lastHealth < _currentHealth.Value)
        {
            for(int i = (int)lastHealth; i < _currentHealth.Value; i++) _parts[i].Play(RECOVER_ANIM);
        }
        else
        {
            for(int i = (int)_currentHealth.Value; i < lastHealth; i++) _parts[i].Play(LOST_ANIM);
        }

        lastHealth = _currentHealth.Value;
    }

    void UpdateMaxHearts()
    {
        if(lastMaxHealth < _maxHealth.Value)
        {
            for(int i = (int)lastMaxHealth/4; i < _maxHealth.Value/4; i++)  hearts[i].gameObject.SetActive(true);
        }
        else
        {
            for(int i = (int)_maxHealth.Value/4; i < lastMaxHealth/4; i++) hearts[i].gameObject.SetActive(false);
        }

        lastMaxHealth = _maxHealth.Value;
    }
}

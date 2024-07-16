using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarHandler : MonoBehaviour
{
    float maxValue;

    [SerializeField] GameObject lifeBarContainer;
    [SerializeField] GameObject lifeBarFill;

    HealthHandler _associatedHealthHandler;

    private void Awake() 
    {
        lifeBarContainer.SetActive(false);
        SetHealthHandler(GetComponentInParent<HealthHandler>());
        _associatedHealthHandler.OnGetDamage.AddListener(ActivateBar);
    }

    void ActivateBar()
    {
        lifeBarContainer.SetActive(true);
        _associatedHealthHandler.OnGetDamage.RemoveListener(ActivateBar);
    }

    void SetHealthHandler(HealthHandler healthHandler)
    {
        SpriteRenderer sr = healthHandler.GetComponentInChildren<SpriteRenderer>();

        if (sr != null)
        {
            transform.SetParent(healthHandler.transform);
            transform.localPosition = new Vector3(0, sr.bounds.size.y + 0.5f, 0);

            if (_associatedHealthHandler != null) _associatedHealthHandler.OnHealthChanged.RemoveListener(UpdateLifeBar);

            _associatedHealthHandler = healthHandler;
            _associatedHealthHandler.OnHealthChanged.AddListener(UpdateLifeBar);
            maxValue = _associatedHealthHandler.GetMaxHealth();
            UpdateLifeBar(_associatedHealthHandler.CurrentHealth);
        }
        else
        {
            Debug.LogWarning("No Target SpriteRenderer found for this LifeBar");
        }
    }

    void UpdateLifeBar(float value)
    {
        lifeBarFill.transform.localScale = new Vector3(value / maxValue, 1, 1);
    }
}

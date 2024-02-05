using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifesManager : MonoBehaviour
{
    float _life = 10f;
    float life
    {
        get => _life;
        set
        {
            onLifeChange?.Invoke(value);
            _life = value;

            if (_life <= 0)
            {
                Die();
            }
        }
    }
    int knockBackMultiplier = 1;

    Rigidbody2D rb;

    public UnityEvent<float> onLifeChange = new UnityEvent<float>();
    public UnityEvent<string> onStateChange = new UnityEvent<string>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetDamage(float damage, Vector2 knockback)
    {
        life -= damage;
        onStateChange?.Invoke("GetDamage");
        rb.AddForce(knockback * knockBackMultiplier, ForceMode2D.Impulse);
    }

    protected virtual void Die()
    {
        onStateChange?.Invoke("Die");
    }
}

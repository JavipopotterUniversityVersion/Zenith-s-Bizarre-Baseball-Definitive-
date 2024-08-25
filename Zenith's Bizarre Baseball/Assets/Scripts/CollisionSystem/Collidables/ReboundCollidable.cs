using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReboundCollidable : ICollidable
{
    [SerializeField] Float _reboundMultiplier;
    [SerializeField] UnityEvent _onRebound;
    Rigidbody2D rb;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    public override void OnCollide(Collider2D collider)
    {
        rb.velocity = Vector2.Reflect(rb.velocity, (collider.transform.position - transform.position).normalized) * _reboundMultiplier.Value;
        StartCoroutine(CheckSuccess());
    }

    IEnumerator CheckSuccess()
    {
        yield return new WaitForEndOfFrame();
        _onRebound.Invoke();
    }
}
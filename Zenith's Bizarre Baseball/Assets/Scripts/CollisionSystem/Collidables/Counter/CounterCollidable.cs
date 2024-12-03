using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class CounterCollidable : ICollidable
{
    [SerializeField] ObjectProcessor _speedToAdd;
    [SerializeField] AnimationCurve _speedCurve;
    bool _recentBall = false;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out Rigidbody2D rb) && collider.TryGetComponent(out Knockable knockable))
        {
            float force = _speedToAdd.Result();

            float rbSpeed = rb.velocity.magnitude;
            force = force * _speedCurve.Evaluate(rbSpeed/force) * knockable.Reduction;

            Vector2 direction = _recentBall ? -rb.velocity.normalized : (collider.transform.position - transform.position).normalized;

            if(_recentBall == false) 
            {
                _recentBall = true;
                StartCoroutine(RecentBallRoutine());
            }

            rb.velocity = (rbSpeed + force) * direction;
        }
    }

    IEnumerator RecentBallRoutine()
    {
        yield return new WaitForEndOfFrame();
        _recentBall = false;
    }
}
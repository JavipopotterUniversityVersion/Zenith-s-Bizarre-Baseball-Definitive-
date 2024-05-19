using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementController : MonoBehaviour
{
    Rigidbody2D rb;
    public Rigidbody2D Rb => rb;
    [SerializeField] public float _speed = 10f;
    private float _OriginalSpeed;
    public float speed => _speed;

    [SerializeField] UnityEvent onStartMoving = new UnityEvent();
    public UnityEvent OnStartMoving => onStartMoving;

    [SerializeField] UnityEvent<Vector2> onMoving = new UnityEvent<Vector2>();
    public UnityEvent<Vector2> OnMoving => onMoving;

    [SerializeField] UnityEvent onStopMoving = new UnityEvent();
    public UnityEvent OnStopMoving => onStopMoving;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _OriginalSpeed = _speed;
    }

    public void Move(Vector2 direction)
    {
        if(direction != Vector2.zero)
        {
            if(rb.velocity == Vector2.zero) OnStartMoving.Invoke();
            else onMoving.Invoke(direction);
        }
        else if(rb.velocity != Vector2.zero && direction == Vector2.zero) onStopMoving.Invoke();

        rb.velocity = direction * speed;
    }

    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }
    public void ReturnOriginalSpeed()
    {
        _speed = _OriginalSpeed;
    }
}

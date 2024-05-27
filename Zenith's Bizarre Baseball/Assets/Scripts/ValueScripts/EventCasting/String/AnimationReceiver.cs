using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationReceiver : MonoBehaviour
{
    [SerializeField] String _animation;
    Animator _animator;

    private void Awake() => _animator = GetComponent<Animator>();
    private void OnEnable() => _animation.OnStringCall.AddListener(UpdateAnimation);
    private void OnDisable() => _animation.OnStringCall.RemoveListener(UpdateAnimation);
    void UpdateAnimation() => _animator.Play(_animation.Value);
}

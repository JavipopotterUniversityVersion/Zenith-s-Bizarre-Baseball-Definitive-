using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStateManager : EntityStateManager
{
    protected Transform player;

    protected override void Awake()
    {
        base.Awake();
        player = FindObjectOfType<PlayerLifesManager>(true).transform;
    }

    private void OnEnable() {
        StartCoroutine(UpdateState());
    }

    protected abstract IEnumerator UpdateState();

    private void OnDisable() {
        StopCoroutine(UpdateState());
    }
}

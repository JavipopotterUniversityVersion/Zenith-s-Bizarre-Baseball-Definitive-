using UnityEngine;
[RequireComponent(typeof(LifesManager))]
public abstract class EntityStateManager : StateManager
{
    protected LifesManager lifesManager;

    protected override void Awake()
    {
        lifesManager = GetComponent<LifesManager>();

        lifesManager.onStateChange.AddListener(ChangeState);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        lifesManager.onStateChange.RemoveListener(ChangeState);
    }
}
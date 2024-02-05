using UnityEngine;

public abstract class Item : Throwable
{
    bool _canBePickedUp = true;
    public bool canBePickedUp { get { return _canBePickedUp; } }

    [SerializeField] float useDelay = 0.25f;
    float useDelayTimer = 0f;

    public virtual void Use()
    {
        if(useDelayTimer > 0) return;
        useDelayTimer = useDelay;
    }

    private void Update() {
        if(useDelayTimer > 0)
        {
            useDelayTimer -= Time.deltaTime;
        }
    }

    public void PickUp(Transform itemHolder, LayerMask targetLayer)
    {
        transform.SetParent(itemHolder);

        this.targetLayer = targetLayer;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        transform.SetParent(null);
    }

    public override void Throw(Vector2 force, float angularForce)
    {
        Drop();
        base.Throw(force, angularForce);
    }

    protected override void SetBeingThrown(bool value)
    {
        base.SetBeingThrown(value);
        _canBePickedUp = !value;
    }
}
using UnityEngine;
public abstract class Throwable : MonoBehaviour
{
    float throwForceModifier = 1f;
    float throwAngularForceModifier = 1f;
    protected bool _beingThrown = false;

    protected LayerMask targetLayer;
    Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Throw(Vector2 force, float angularForce)
    {
        SetBeingThrown(true);
        rb.AddForce(force * throwForceModifier, ForceMode2D.Impulse);
        rb.AddTorque(angularForce * throwAngularForceModifier, ForceMode2D.Impulse);
    }

    protected virtual void SetBeingThrown(bool value) => _beingThrown = value;
}
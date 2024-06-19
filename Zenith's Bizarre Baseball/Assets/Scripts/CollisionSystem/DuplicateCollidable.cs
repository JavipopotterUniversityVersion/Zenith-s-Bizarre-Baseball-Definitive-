using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DuplicateCollidable : ICollidable
{
    [SerializeField] float[] angleOffsets;

    public void SetAngleOffsets(float[] angleOffsets)
    {
        this.angleOffsets = angleOffsets;
    }

    public void SetAngleOffsets(string angleOffsets)
    {
        this.angleOffsets = angleOffsets.Split(',').Select(float.Parse).ToArray();
    }

    public override void OnCollide(Collider2D collider)
    {
        List<Collider2D> colliders = new List<Collider2D>();
        foreach (var angleOffset in angleOffsets)
        {
            colliders.Add(Duplicate(collider, angleOffset));
        }

        StartCoroutine(DeactivateCollider(colliders));
    }

    IEnumerator DeactivateCollider(List<Collider2D> colliders)
    {
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        yield return new WaitForSeconds(0.1f);

        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }
    }

    private Collider2D Duplicate(Collider2D collider, float angleOffset)
    {
        Rigidbody2D rb = Instantiate(collider.gameObject, collider.transform.position, collider.transform.rotation).GetComponent<Rigidbody2D>();

        if(rb != null)
        {
            float angle = Mathf.Deg2Rad * (transform.eulerAngles.z + angleOffset + 90);
            Vector2 relativeDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            print(relativeDirection);

            rb.velocity = relativeDirection.normalized * collider.attachedRigidbody.velocity.magnitude;
        }

        return rb.GetComponent<Collider2D>();
    }
}
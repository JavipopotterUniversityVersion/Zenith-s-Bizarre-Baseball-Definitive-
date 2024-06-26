using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DuplicateCollidable : ICollidable, IGameObjectProcessor
{
    [SerializeField] float[] angleOffsets;
    [SerializeField] float initialDistance;
    [SerializeField] IRef<IGameObjectProcessor>[] _processors;

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
            colliders.Add(Duplicate(collider.gameObject, angleOffset).GetComponent<Collider2D>());
        }

        StartCoroutine(DeactivateCollider(colliders));
    }

    public void Process(GameObject gameObject)
    {
        foreach (var angleOffset in angleOffsets)
        {
            Duplicate(gameObject, angleOffset);
        }
        IGameObjectProcessor.Process(gameObject, _processors);
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

    private Rigidbody2D Duplicate(GameObject gameObject, float angleOffset)
    {
        GameObject clone = Instantiate(gameObject.gameObject, gameObject.transform.position, gameObject.transform.rotation);
        Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();

        if(rb != null)
        {
            float angle = Mathf.Deg2Rad * (transform.eulerAngles.z + angleOffset + 90);
            Vector2 relativeDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            rb.transform.position += (Vector3) relativeDirection * initialDistance;
            rb.velocity = relativeDirection.normalized * rb.velocity.magnitude;
        }

        IGameObjectProcessor.Process(clone, _processors);
        return rb;
    }
}
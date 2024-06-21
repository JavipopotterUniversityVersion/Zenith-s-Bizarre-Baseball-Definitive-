using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeactiveColliderProcess : MonoBehaviour, IGameObjectProcessor
{
    [SerializeField] float _deactivateDuration = 0.2f;

    public void Process(GameObject gameObject)
    {
        StartCoroutine(DeactiveCollider(gameObject.GetComponentsInChildren<Collider2D>()));
    }

    IEnumerator DeactiveCollider(Collider2D[] colliders)
    {
        colliders.ToList().ForEach(collider => collider.enabled = false);
        yield return new WaitForSeconds(_deactivateDuration);
        colliders.ToList().ForEach(collider => collider.enabled = true);
    }
}
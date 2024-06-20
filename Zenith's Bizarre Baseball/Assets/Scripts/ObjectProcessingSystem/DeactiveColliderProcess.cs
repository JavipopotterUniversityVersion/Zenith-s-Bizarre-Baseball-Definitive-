using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactiveColliderProcess : MonoBehaviour, IGameObjectProcessor
{
    [SerializeField] float _deactivateDuration = 0.2f;

    public void Process(GameObject gameObject)
    {
        StartCoroutine(DeactiveCollider(gameObject.GetComponent<Collider2D>()));
    }

    IEnumerator DeactiveCollider(Collider2D collider)
    {
        collider.enabled = false;
        yield return new WaitForSeconds(_deactivateDuration);
        collider.enabled = true;
    }
}
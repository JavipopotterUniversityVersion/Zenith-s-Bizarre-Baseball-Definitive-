using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconPooler : MonoBehaviour
{
    [SerializeField] SpriteRenderer _target;
    [SerializeField] GameObject _iconPrefab;
    [SerializeField] float offset = 0.5f;
    GameObject iconReference;

    private void Start() {
        iconReference = ObjectPooler.Instance.SpawnFromPool(transform.position, Quaternion.identity, _iconPrefab);
        if(_target == null) 
        {
            _target = transform.parent.parent.GetComponentInChildren<SpriteRenderer>();
            Debug.LogWarning("Target not set for IconPooler. Defaulting to parent's parent's SpriteRenderer.["
            + transform.parent.parent.name + "]");
        }
    }

    public void ActivateIcon() {
        iconReference.SetActive(true);
        float yPosition = _target.transform.position.y + _target.bounds.size.y + offset;
        iconReference.transform.position = new Vector3(_target.transform.position.x, yPosition, _target.transform.position.z);
    }

    public void DeactivateIcon() {
        if(iconReference != null) iconReference.SetActive(false);
    }
}

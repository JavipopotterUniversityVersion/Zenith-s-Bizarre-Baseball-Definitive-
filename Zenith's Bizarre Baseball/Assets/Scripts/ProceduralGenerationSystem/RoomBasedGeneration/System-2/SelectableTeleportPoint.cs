using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SelectableTeleportPoint : MonoBehaviour
{
    Tilemap _map;
    [SerializeField] Bool _boolSignal;
    [SerializeField] Color _normalColor;
    [SerializeField] Color _overColor;
    [SerializeField] Transform _teleportPoint;
    [SerializeField] Identifiable _playerIdentifiable;
    Transform _player;

    private void Awake() {
        _map = GetComponent<Tilemap>();
    }

    private void Start() {
        StartCoroutine(findPlayer());
    }

    IEnumerator findPlayer() {
        yield return new WaitForSeconds(1);
        _player = SearchManager.Instance.GetClosestSearchable(transform.position, _playerIdentifiable).transform;
    }

    private void OnMouseEnter() {
        _map.color = _overColor;
    }

    private void OnMouseExit() {
        _map.color = _normalColor;
    }

    private void OnMouseDown() {
        _player.position = _teleportPoint.position;
        _boolSignal.SetValue(false);
    }
}

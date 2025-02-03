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
    [SerializeField] Color _insideColor;

    bool selected = false;

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

        if(_player.transform.parent == transform.parent) {
            selected = true;
            _map.color = _insideColor;
        }
    }

    private void OnMouseEnter() {
        _map.color = _overColor;
    }

    private void OnMouseExit() {
        _map.color = selected ? _insideColor : _normalColor;
    }

    private void OnMouseDown() {
        _player.position = _teleportPoint.position;
        _map.color = _insideColor;
        _boolSignal.SetValue(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.transform == _player) {
            _map.color = _insideColor;
            selected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.transform == _player) {
            _map.color = _normalColor;
            selected = false;
        }
    }
}

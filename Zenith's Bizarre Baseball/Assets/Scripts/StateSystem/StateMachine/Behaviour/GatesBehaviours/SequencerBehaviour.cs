using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] GameObject[] _behaviourContainers;
    IBehaviour[] _behaviours;
    int _currentBehaviour = 0;

    private void Awake()
    {
        _behaviours = new IBehaviour[_behaviourContainers.Length];
        for (int i = 0; i < _behaviourContainers.Length; i++)
        {
            _behaviours[i] = _behaviourContainers[i].GetComponent<IBehaviour>();
        }
    }

    public void ExecuteBehaviour()
    {
        _behaviours[_currentBehaviour].ExecuteBehaviour();
        _currentBehaviour++;

        if (_currentBehaviour >= _behaviours.Length) ResetIndex();
    }

    public void ResetIndex() => SetIndex(0);
    public void SetIndex(int index) => _currentBehaviour = index;

    private void OnValidate() {
        name = "SequencerBehaviour";
    }
}

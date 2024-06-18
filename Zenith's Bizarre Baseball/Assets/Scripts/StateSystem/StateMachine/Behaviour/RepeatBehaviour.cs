using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBehaviour : MonoBehaviour, IBehaviour, ICondition
{
    [SerializeField] private int _minNumberOfIterations;
    [SerializeField] private int _maxNumberOfIterations;
    private int _numberOfIterations;
    [SerializeField] BehaviourIteration[] _behavioursToRepeat;
    bool finished = false;

    public void ExecuteBehaviour()
    {
        StartCoroutine(Repeat());
    }

    public bool CheckCondition()
    {
        if(finished)
        {
            finished = false;
            return true;
        }

        return false;
    }

    private IEnumerator Repeat()
    {
        _numberOfIterations = Random.Range(_minNumberOfIterations, _maxNumberOfIterations + 1);
        for(int i = 0; i < _numberOfIterations; i++)
        {
            foreach (BehaviourIteration iteration in _behavioursToRepeat)
            {
                iteration.BehaviourContainer.GetComponent<IBehaviour>().ExecuteBehaviour();
                float _waitTime = Random.Range(iteration.MinTime, iteration.MaxTime);
                yield return new WaitForSeconds(_waitTime);
            }
        }

        finished = true;
    }
    private void OnValidate()
    {
        if(_behavioursToRepeat.Length > 0)
            name = "Repeat " + _behavioursToRepeat[0].BehaviourContainer.name 
            + (_minNumberOfIterations == _maxNumberOfIterations ? $" {_minNumberOfIterations} times" 
            : $" {_minNumberOfIterations}-{_maxNumberOfIterations} times");
    }
}
[System.Serializable]
public class BehaviourIteration
{
    [SerializeField] private GameObject _behaviourContainer;
    public GameObject BehaviourContainer => _behaviourContainer;
    
    [SerializeField] private float _mintime;
    public float MinTime => _mintime;

    [SerializeField] private float _maxtime;
    public float MaxTime => _maxtime;
}

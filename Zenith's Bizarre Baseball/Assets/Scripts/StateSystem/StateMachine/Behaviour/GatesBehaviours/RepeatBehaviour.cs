using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBehaviour : MonoBehaviour, IBehaviour, ICondition
{
    [SerializeField] ObjectProcessor _processor;
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
        int _numberOfIterations = _processor.ResultInt(1);
        for(int i = 0; i < _numberOfIterations; i++)
        {
            foreach (BehaviourIteration iteration in _behavioursToRepeat)
            {
                iteration.BehaviourContainer.GetComponent<IBehaviour>().ExecuteBehaviour();
                float _waitTime = iteration.waitTime;
                yield return new WaitForSeconds(_waitTime);
            }
        }

        finished = true;
    }
    private void OnValidate()
    {
        if(_behavioursToRepeat.Length > 0)
            name = "Repeat " + _behavioursToRepeat[0].BehaviourContainer.name + $" {_processor.ResultInt(1)}";
    }
}
[System.Serializable]
public class BehaviourIteration
{
    [SerializeField] private GameObject _behaviourContainer;
    public GameObject BehaviourContainer => _behaviourContainer;
    
    [SerializeField] ObjectProcessor _waitTime;
    public float waitTime => _waitTime.Result();
}

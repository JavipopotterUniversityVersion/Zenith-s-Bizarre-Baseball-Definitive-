using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DelayedBehaviour : MonoBehaviour, IBehaviour, ICondition
{
    [SerializeField] ObjectProcessor _delayTime;
    [SerializeField] string _behaviourName;

    [SerializeField] IRef<IBehaviour>[] behaviours;
    bool finished = false;

    public void ExecuteBehaviour() => StartCoroutine(DelayBehaviour());
    public bool CheckCondition()
    {
        if(finished)
        {
            finished = false;
            return true;
        }
        return false;
    }

    IEnumerator DelayBehaviour()
    {
        float delayTime = _delayTime.Result();
        yield return new WaitForSeconds(delayTime);
        behaviours.ToList().ForEach(x => x.I.ExecuteBehaviour());
        finished = true;
    }

    private void OnValidate() {
        name = $"Delay {_behaviourName} {_delayTime.Result()}s";
    }
}

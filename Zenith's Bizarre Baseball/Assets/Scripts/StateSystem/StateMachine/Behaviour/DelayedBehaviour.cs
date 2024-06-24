using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DelayedBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] ObjectProcessor _delayTime;

    [SerializeField] IRef<IBehaviour>[] behaviours;

    public void ExecuteBehaviour() => StartCoroutine(DelayBehaviour());
    IEnumerator DelayBehaviour()
    {
        float delayTime = _delayTime.Result();
        yield return new WaitForSeconds(delayTime);
        behaviours.ToList().ForEach(x => x.I.ExecuteBehaviour());
    }

    private void OnValidate() {
        name = $"Delay Behaviour {_delayTime.Result()}s";
    }
}

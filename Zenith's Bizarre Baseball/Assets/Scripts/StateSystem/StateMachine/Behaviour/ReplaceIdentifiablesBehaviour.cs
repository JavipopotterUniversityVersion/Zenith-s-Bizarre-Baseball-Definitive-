using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;

public class ReplaceIdentifiablesBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] Identifiable _targetType;
    [SerializeField] GameObject _replacementPrefab;

    public void ExecuteBehaviour()
    {
        Transform[] targets = SearchManager.Instance.GetAllSearchables<Transform>(_targetType);
        foreach (var target in targets)
        {
            target.gameObject.SetActive(false);
            Instantiate(_replacementPrefab, target.transform.position, Quaternion.identity, target.transform.parent);
        }

        GameObject[] targetsGameObjects = targets.Select(target => target.gameObject).ToArray();
        StartCoroutine(DestroyTargets(targetsGameObjects));
    }

    IEnumerator DestroyTargets(GameObject[] targets)
    {
        foreach (GameObject target in targets)
        {
            Destroy(target);
            yield return new WaitForEndOfFrame();
        }
    }
}

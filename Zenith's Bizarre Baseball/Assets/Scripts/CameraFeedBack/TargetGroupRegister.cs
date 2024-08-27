using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGroupRegister : MonoBehaviour
{
    [SerializeField] float _weight = 1;
    [SerializeField] float _radius = 15;

    private void OnEnable() 
    {
        StartCoroutine(AddMember());
    }

    IEnumerator AddMember()
    {
        yield return new WaitForEndOfFrame();
        MainVirtualCamera.Instance.TargetGroup.AddMember(this.transform, _weight, _radius);
    }

    private void OnDisable() 
    {
        MainVirtualCamera.Instance.TargetGroup.RemoveMember(this.transform);
    }
}

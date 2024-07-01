using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetScreenResolution : MonoBehaviour
{
    [SerializeField] Vector2Int _resolution;
    public void SetResolution() => Screen.SetResolution(_resolution.x, _resolution.y, Screen.fullScreen);
}

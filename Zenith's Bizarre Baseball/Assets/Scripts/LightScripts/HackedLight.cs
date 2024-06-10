using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
[RequireComponent(typeof(Light2D))]
public class HackedLight : MonoBehaviour {

    public Vector4 hackColor;
    public float multiplier = 1;

    void Update ()
    {
        var light = GetComponent<Light2D>();
        light.color = new Color(hackColor.x, hackColor.y, hackColor.z, hackColor.w) * multiplier;
    }
}
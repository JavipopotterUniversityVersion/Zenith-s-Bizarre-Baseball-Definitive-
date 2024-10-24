using UnityEngine;

public class FrequencyToFloat : FrequencyToMovement<float>
{
    protected override float Lerp(float t1, float t2, float ammount)
        => Mathf.Lerp(t1, t2, ammount);
}

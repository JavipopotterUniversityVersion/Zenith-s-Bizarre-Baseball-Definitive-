using UnityEngine;

public class FrequencyToQuaternion : FrequencyToMovement<Quaternion>
{
    protected override Quaternion Lerp(Quaternion t1, Quaternion t4, float ammount)
        => Quaternion.Lerp(t1, t4, ammount);
}
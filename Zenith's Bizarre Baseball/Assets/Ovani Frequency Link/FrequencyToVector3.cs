using UnityEngine;

public class FrequencyToVector3 : FrequencyToMovement<Vector3>
{
    protected override Vector3 Lerp(Vector3 t1, Vector3 t2, float ammount)
        => Vector3.Lerp(t1, t2, ammount);
}

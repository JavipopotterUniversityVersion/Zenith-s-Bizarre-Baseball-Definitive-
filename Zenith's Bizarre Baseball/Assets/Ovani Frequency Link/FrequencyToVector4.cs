using UnityEngine;

public class FrequencyToVector4 : FrequencyToMovement<Vector4>
{
    protected override Vector4 Lerp(Vector4 t1, Vector4 t4, float ammount)
        => Vector4.Lerp(t1, t4, ammount);
}

using UnityEngine;

public class FrequencyToVector2 : FrequencyToMovement<Vector2>
{
    protected override Vector2 Lerp(Vector2 t1, Vector2 t2, float ammount)
        => Vector2.Lerp(t1, t2, ammount);
}

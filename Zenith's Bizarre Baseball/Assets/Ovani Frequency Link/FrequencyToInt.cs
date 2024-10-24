using System;
using UnityEngine;

public class FrequencyToInt : FrequencyToMovement<int>
{
    protected override int Lerp(int t1, int t2, float ammount)
        => (int)Math.Round(Mathf.Lerp(t1, t2, ammount));
}

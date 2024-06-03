using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildrenIndexReadable : Readable
{
    public override float Read()
    {
        int i = 0;
        while (i < transform.parent.childCount && transform.parent.GetChild(i) != transform) i++;

        return i;
    }
}
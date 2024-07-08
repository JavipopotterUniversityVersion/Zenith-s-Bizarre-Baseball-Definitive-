using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildrenSorter : MonoBehaviour
{
    private void Awake() => SortChildren();

    [ContextMenu("Sort Children")]
    public void SortChildren()
    {
        SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();

        foreach(SpriteRenderer child in children)
        {
            child.sortingOrder = Mathf.RoundToInt(-transform.position.y);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

[CreateAssetMenu(fileName = "New Identifiable", menuName = "Identification/Identifiable")]
public class Identifiable : ScriptableObject
{
    [SerializeField] Identifiable[] ascendants;
    [SerializeField][DisplayInspector()] Identifiable[] descendants;

    public bool DerivesTo(Identifiable descendant)
    {
        if(this == descendant) return true;
        foreach (Identifiable identifiable in descendants)
        {
            if (identifiable == descendant || identifiable.DerivesTo(descendant)) return true;
        }
        return false;
    }

    public bool DerivesFrom(Identifiable ascendant)
    {
        if(this == ascendant) return true;
        foreach (Identifiable identifiable in ascendants)
        {
            if (identifiable == ascendant || identifiable.DerivesFrom(ascendant)) return true;
        }
        return false;
    }
}

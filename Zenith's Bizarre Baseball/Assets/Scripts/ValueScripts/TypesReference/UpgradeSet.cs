using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UpgradeSet", menuName = "TypesReference/UpgradeSet")]
public class UpgradeSet : ScriptableObject
{
    [SerializeField] Upgrade[] _upgrades;
    public Upgrade[] Upgrades => _upgrades;
}
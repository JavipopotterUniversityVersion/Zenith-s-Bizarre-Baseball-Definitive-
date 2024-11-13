using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;

public class Heart : MonoBehaviour
{
    [SerializeField] GameObject _background;
    [SerializeField] Animator[] _parts;

    public Animator[] GetParts() => _parts;
}

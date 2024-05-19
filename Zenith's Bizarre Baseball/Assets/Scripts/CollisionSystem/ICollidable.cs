using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICollidable : MonoBehaviour
{
    public virtual void OnCollide(Collider2D collider) { }
}

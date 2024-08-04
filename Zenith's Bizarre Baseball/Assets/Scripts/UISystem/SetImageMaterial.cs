using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetImageMaterial : MonoBehaviour
{
    [SerializeField] Image image;

    public void SetMaterial(Material material)
    {
        image.material = material;
    }
}

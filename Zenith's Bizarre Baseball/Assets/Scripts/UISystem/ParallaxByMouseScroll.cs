using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxByMouseScroll : MonoBehaviour
{
    Vector2 offset;
    Vector2 _initialPosition;
    [SerializeField] float parallaxFactor = 1;
    
    private void Awake() 
    {
        _initialPosition = transform.position;
        offset = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    private void Update() {
        transform.position = _initialPosition + ((Vector2)Input.mousePosition - offset) / 1000 * parallaxFactor;
    }
}

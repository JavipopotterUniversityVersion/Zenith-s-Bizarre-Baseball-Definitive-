using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public static Cursor Instance { get; private set; }
    [SerializeField] Transform _cursorTransform;
    [SerializeField] Canvas _canvas;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            StartCoroutine(FollowMouse());
        }
        else Destroy(gameObject);
    }

    IEnumerator FollowMouse()
    {
        while(true)
        {
            Vector2 pointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _cursorTransform.position = pointerPosition;
            yield return null;
        }
    }
}

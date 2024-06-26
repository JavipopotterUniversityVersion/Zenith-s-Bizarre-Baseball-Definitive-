using UnityEngine;

public class TranslateProcess : MonoBehaviour, IGameObjectProcessor
{
    [SerializeField] ObjectProcessor _x;
    [SerializeField] ObjectProcessor _y;

    public void Process(GameObject gameObject)
    {
        gameObject.transform.position += new Vector3(_x.Result(), _y.Result(), 0);
    }
}

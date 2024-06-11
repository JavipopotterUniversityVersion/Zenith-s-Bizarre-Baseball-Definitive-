using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectProcessorInstance : MonoBehaviour
{
    [SerializeField] IRef<IGameObjectProcessor>[] _objectProcessor;
    public void Process(GameObject gameObject) => _objectProcessor.ToList().ForEach(x => x.I.Process(gameObject));
}

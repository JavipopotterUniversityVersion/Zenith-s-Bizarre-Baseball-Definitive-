using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnregisterOnDestroy : MonoBehaviour
{
    GameObjectRegisterProcess _registerProcess;
    public void SetRegisterProcess(GameObjectRegisterProcess registerProcess) => _registerProcess = registerProcess;

    private void OnDestroy() => _registerProcess.Remove(gameObject);
}

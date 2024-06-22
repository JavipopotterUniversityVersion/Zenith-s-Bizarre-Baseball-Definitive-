using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class InstanceBlock : MonoBehaviour
{
    [SerializeField] InstanceUnit[] _instanceUnits;
    [SerializeField] InstanceType _instanceType;

    enum InstanceType
    {
        Random,
        Order
    }

    int _currentUnitIndex = 0;

    public void Instance(InstanceUnit[] extraUnits)
    {
        List<InstanceUnit> units = new List<InstanceUnit>();
        units.AddRange(_instanceUnits);
        units.AddRange(extraUnits);

        InstanceUnit selectedUnit = null;
        GameObject instance = null;

        switch(_instanceType)
        {
            case InstanceType.Random:
                selectedUnit = InstanceUnit.GetRandomUnit(units.ToArray());
                instance = Instantiate(selectedUnit.Instance, transform.position, Quaternion.identity);
                break;
            case InstanceType.Order:
                selectedUnit = InstanceUnit.GetFirstAvailableInstance(units.ToArray(), _currentUnitIndex);
                instance = Instantiate(selectedUnit.Instance, transform.position, Quaternion.identity);
                _currentUnitIndex++;
                break;
        }

        IGameObjectProcessor.Process(instance, selectedUnit.Processors);
    }
}

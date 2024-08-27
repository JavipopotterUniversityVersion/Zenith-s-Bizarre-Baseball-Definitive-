using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataSeter", menuName = "SaveSystem/DataSeter")]
public class DataSeter : ScriptableObject
{
    [SerializeField] FloatSet[] _floatSets;
    [SerializeField] IntSet[] _intSets;
    [SerializeField] BoolSet[] _boolSets;

    [ContextMenu("Set Data")]
    public void SetData()
    {
        foreach (var set in _floatSets) set.Set();
        foreach (var set in _intSets) set.Set();
        foreach (var set in _boolSets) set.Set();
    }

    [Serializable] class FloatSet : ISerializationCallbackReceiver
    {
        [SerializeField] string _name;
        [SerializeField] Float _float;
        [SerializeField] float _value;

        public void Set() => _float.SetRawValue(_value);

        public void OnBeforeSerialize() => _name = _float.name;
        public void OnAfterDeserialize() {}
    }
    [Serializable] class IntSet : ISerializationCallbackReceiver
    {
        [SerializeField] string _name;
        [SerializeField] Int _int;
        [SerializeField] int _value;

        public void Set() => _int.Value = _value;

        public void OnBeforeSerialize() => _name = _int.name;
        public void OnAfterDeserialize() {}
    }
    [Serializable] class BoolSet : ISerializationCallbackReceiver
    {
        [SerializeField] string _name;
        [SerializeField] Bool _bool;
        [SerializeField] bool _value;

        public void Set() => _bool.SetRawValue(_value);

        public void OnBeforeSerialize() => _name = _bool.name;
        public void OnAfterDeserialize() {}
    }
}
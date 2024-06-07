using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringReceiver : MonoBehaviour
{
    String _string;
    string _stringString;

    public void SetString(String value)
    {
        _string = value;
        _stringString = value.Value;
    }

    public void SetString(string value) => _stringString = value;

    public void CopyToString(String receiver) => receiver.SetString(_stringString);

    public String GetString() => _string;
    public string GetStringString() => _stringString;
}
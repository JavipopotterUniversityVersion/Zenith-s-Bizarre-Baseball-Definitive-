using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "StringFunction", menuName = "Value/StringFunction")]
public class StringFunction : ScriptableObject
{
    [SerializeField] string _function;
    [SerializeField] string[] _variables;
    public string Result(string input)
    {
        string[] inputs = input.Split(",").Select(x => x.Trim()).ToArray();
        string result = _function;

        for (int i = 0; i < _variables.Length; i++)
        {
            result = result.Replace($"{_variables[i]}", inputs[i]);
        }
        return result;
    }
}
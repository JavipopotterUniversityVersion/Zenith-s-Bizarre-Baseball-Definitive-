using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveSet", menuName = "SaveSystem/SaveSet")]
public class SaveSet : ScriptableObject
{
    [SerializeField] string _path;
    public string Path => Application.persistentDataPath + "/" + _path; 
    [SerializeField] SerializableDictionary<string, Float> _floatData;
    [SerializeField] SerializableDictionary<string, Int> _intData;
    [SerializeField] SerializableDictionary<string, Bool> _boolData;

    [ContextMenu("Write")]
    public void Write()
    {
        StreamWriter writer = new StreamWriter(Path);

        writer.WriteLine("Floats:");

        foreach (KeyValuePair<string, Float> pair in _floatData)
        {
            writer.WriteLine(pair.Key + " = " + pair.Value.GetRawValue());
        }

        writer.WriteLine("Ints:");

        foreach (KeyValuePair<string, Int> pair in _intData)
        {
            writer.WriteLine(pair.Key + " = " + pair.Value.Value);
        }

        writer.WriteLine("Bools:");

        foreach (KeyValuePair<string, Bool> pair in _boolData)
        {
            writer.WriteLine(pair.Key + " = " + pair.Value.Value);
        }

        writer.Close();
    }

    [ContextMenu("Read")]
    public void Read()
    {
         string path = Application.persistentDataPath + "/" + _path;
        StreamReader reader = new StreamReader(path);
        int readType = 0;

        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();

            if(line == "Floats:") { readType = 0; continue; }
            if(line == "Ints:") { readType = 1; continue; }
            if(line == "Bools:") { readType = 2; continue; }

            switch (readType)
            {
                case 0:
                    ReadFloat(line);
                    break;
                case 1:
                    ReadInt(line);
                    break;
                case 2:
                    ReadBool(line);
                    break;
            }
        }

        reader.Close();
    }

    void ReadFloat(string line)
    {
        string[] parts = line.Split('=');
        string key = parts[0].Trim();
        float value = float.Parse(parts[1].Trim());

        _floatData[key].SetRawValue(value);
    }

    void ReadInt(string line)
    {
        string[] parts = line.Split('=');
        string key = parts[0].Trim();
        int value = int.Parse(parts[1].Trim());

        _intData[key].Value = value;
    }

    void ReadBool(string line)
    {
        string[] parts = line.Split('=');
        string key = parts[0].Trim();
        bool value = bool.Parse(parts[1].Trim());

        _boolData[key].SetValue(value);
    }
}
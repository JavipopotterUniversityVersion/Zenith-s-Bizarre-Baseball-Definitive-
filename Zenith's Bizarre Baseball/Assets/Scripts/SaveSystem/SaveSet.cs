using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveSet", menuName = "SaveSystem/SaveSet")]
public class SaveSet : ScriptableObject
{

    [SerializeField] SerializableDictionary<string, IRef<ISaveable>> _saveables;

    public void SaveFloat(Float saveable) => Save(saveable);
    public void SaveInt(Int saveable) => Save(saveable);
    public void SaveBool(Bool saveable) => Save(saveable);

    public void Save(ISaveable saveable)
    {
        if(_saveables.ContainsKey(saveable.Key))
        {
            string persistentDataPath = Application.persistentDataPath + Path.DirectorySeparatorChar + saveable.Key + ".save";

            StreamWriter writer = new StreamWriter(persistentDataPath);
            writer.Write(saveable.SaveValue());
            writer.Close();
        }
        else
        {
            Debug.LogWarning("Saveable not found in SaveSet");
        }
    }

    [ContextMenu("Load")]
    public void Load()
    {
        foreach(KeyValuePair<string, IRef<ISaveable>> saveable in _saveables)
        {
            string persistentDataPath = Application.persistentDataPath + Path.DirectorySeparatorChar + saveable.Key + ".save";

            if(File.Exists(persistentDataPath))
            {
                StreamReader reader = new StreamReader(persistentDataPath);
                saveable.Value.I.LoadValue(float.Parse(reader.ReadToEnd()));
                reader.Close();
            }
        }
    }

    [SerializeField] IRef<ISaveable>[] _saveablesStack;


    [ContextMenu("SaveInDictionary")]
    public void SaveInDictionary()
    {
        foreach (IRef<ISaveable> saveable in _saveablesStack)
        {
            if(!_saveables.ContainsKey(saveable.I.Key)) _saveables.Add(saveable.I.Key, saveable);
            else Debug.LogWarning($"Saveable with key {saveable.I.Key} already exists in SaveSet");
        }

        _saveablesStack = new IRef<ISaveable>[0];
    }
}

public interface ISaveable
{
    string Key { get; }
    float SaveValue();
    void LoadValue(float value);
}
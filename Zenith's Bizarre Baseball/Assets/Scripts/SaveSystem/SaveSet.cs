using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyBox;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveSet", menuName = "SaveSystem/SaveSet")]
public class SaveSet : ScriptableObject
{
    [SerializeField] SerializableDictionary<string, SaveableData> _saveables;
    [SerializeField] DialogueSequence[] _dialogueSequence;

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

    [ContextMenu("ResetData")]
    public void ResetData()
    {
        _dialogueSequence.ForEach(x => x.ResetData());
        foreach(KeyValuePair<string, SaveableData> save in _saveables)
        {
            save.Value.data.I.LoadValue(save.Value.originalValue);
            Save(save.Value.data.I);
        }

        SaveDialogueSequences();
    }

    [ContextMenu("SaveDialogueSequences")]
    public void SaveDialogueSequences()
    {
        foreach (DialogueSequence sequence in _dialogueSequence)
        {
            string persistentDataPath = Application.persistentDataPath + Path.DirectorySeparatorChar + sequence.name + ".json";
            StreamWriter writer = new StreamWriter(persistentDataPath);
            writer.Write(JsonUtility.ToJson(sequence));
            writer.Close();
        }
    }

    [ContextMenu("Load")]
    public void Load()
    {
        foreach(KeyValuePair<string, SaveableData> saveable in _saveables)
        {
            string persistentDataPath = Application.persistentDataPath + Path.DirectorySeparatorChar + saveable.Key + ".save";

            if(File.Exists(persistentDataPath))
            {
                StreamReader reader = new StreamReader(persistentDataPath);
                saveable.Value.data.I.LoadValue(float.Parse(reader.ReadToEnd()));
                reader.Close();
            }
        }

        foreach (DialogueSequence sequence in _dialogueSequence)
        {
            string persistentDataPath = Application.persistentDataPath + Path.DirectorySeparatorChar + sequence.name + ".json";
            if (File.Exists(persistentDataPath)) JsonUtility.FromJsonOverwrite(File.ReadAllText(persistentDataPath), sequence);
        }
    }

    private void OnValidate() 
    {
        List<KeyValuePair<string, SaveableData>> dataToRemove = new List<KeyValuePair<string, SaveableData>>();
        List<KeyValuePair<string, SaveableData>> dataToAdd = new List<KeyValuePair<string, SaveableData>>();

        foreach(KeyValuePair<string, SaveableData> saveable in _saveables)
        {
            if(saveable.Value.data.I == null) return;

            string key = saveable.Value.data.I.Key;
            if(saveable.Key != key)
            {
                dataToRemove.Add(saveable);
                dataToAdd.Add(new KeyValuePair<string, SaveableData>(key, saveable.Value));
            }
        }

        dataToRemove.ForEach(x => _saveables.Remove(x));
        dataToAdd.ForEach(x => _saveables.Add(x));
    }

    [Serializable]
    struct SaveableData
    {
        public IRef<ISaveable> data;
        public float originalValue;

        public SaveableData(IRef<ISaveable> saveable, float originalValue = 0)
        {
            data = saveable;
            this.originalValue = originalValue;
        }
    }

    [SerializeField] IRef<ISaveable>[] dataToSave;

    [ContextMenu("Add Data Stack")]
    public void AddDataStack()
    {
        foreach(var data in dataToSave)
        {
            if(_saveables.ContainsKey(data.I.Key) == false)
            {
                _saveables.Add(data.I.Key, new SaveableData(data, data.I.SaveValue()));
            }
        }
    }
}

public interface ISaveable
{
    string Key { get; }
    float SaveValue();
    void LoadValue(float value);
}
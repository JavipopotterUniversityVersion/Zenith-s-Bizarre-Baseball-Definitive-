using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DialoguesExtractor : MonoBehaviour
{
    [SerializeField] TextAsset dialoguesFile;
    [SerializeField] TextAsset dialoguesFunctionsFile;

    public SerializableDictionary<string, string[]> dialogues = new SerializableDictionary<string, string[]>();
    public SerializableDictionary<string, string[]> functions = new SerializableDictionary<string, string[]>();

    private void Awake() {
        string[] dialogDivisions = dialoguesFile.text.Split("DialogName:");

        foreach(string line in dialogDivisions)
        {
            string[] a = line.Split("==>");
            dialogues.Add(a[0].Replace(" ", ""), a[1].Split($"\n"));
        }

        string[] functionsDivisions = dialoguesFunctionsFile.text.Split("FunctionName:");

        foreach(string line in functionsDivisions)
        {
            string[] a = line.Split("==>");
            functions.Add(a[0].Replace(" ", ""), a[1].Split($"\n"));
        }
    }
}
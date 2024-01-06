using System.Collections;
using UnityEngine;
public class DialoguesManager : MonoBehaviour
{
    SerializableDictionary<string, string[]> dialogues = new SerializableDictionary<string, string[]>();
    SerializableDictionary<string, string[]> functions = new SerializableDictionary<string, string[]>();
    DialoguesExtractor extractor;

    private void Start() {
        extractor = GetComponent<DialoguesExtractor>();
        dialogues = extractor.dialogues;
        functions = extractor.functions;
    }

    public void StartDialogue(string key, DialogueWriter selectedWriter)
    {
        GameManager.instance.ChangeGameState(GameState.Dialogue);
        StartCoroutine(_StartDialogue(dialogues[key], selectedWriter));
    }

    IEnumerator _StartDialogue(string[] key, DialogueWriter selectedWriter)
    {
        foreach (string line in key)
        {
            if(line.StartsWith("-"))
            {
                selectedWriter.Write(line.Substring(1));
            }
            else
            {
                if(line.StartsWith("name:"))
                {
                    selectedWriter.WriteTitle(line.Substring(6));
                }
                else if(line.StartsWith("delay:"))
                {
                    selectedWriter.delay = float.Parse(line.Substring(6));
                }
                else if(line.StartsWith("beepSound:"))
                {
                    selectedWriter.beepSound = line.Substring(10).Replace(" ", "");
                }

                continue;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
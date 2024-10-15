using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor.Callbacks;
#endif

[Serializable] [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue Container")]
public class DialogueContainer : ScriptableObject
{
    [SerializeField] UnityEvent onTriggerDialogue = new UnityEvent();
    public UnityEvent OnTriggerDialogue => onTriggerDialogue;
    
    public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
    [SerializeReference] public List<NodeData> DialogueNodeData = new List<NodeData>();
    public string Translation
    {
        get
        {
            LinkNodes();
            return DialogueNodeData[0].TranslateText();
        }
    }

    [SerializeField] [TextArea(1, 99999999)] string _translationPreview;

    [ContextMenu("Preview")]
    void Preview()
    {
        LinkNodes();
        _translationPreview = Translation;
    }

    public void LinkNodes()
    {
        DialogueNodeData.ForEach(x => x.LinkedNodes.Clear());
        foreach (var nodeLink in NodeLinks)
        {
            NodeData baseNodeData = DialogueNodeData.Find(x => x.GUID == nodeLink.BaseNodeGuid);
            NodeData targetNodeData = DialogueNodeData.Find(x => x.GUID == nodeLink.TargetNodeGuid);

            if (baseNodeData != null && targetNodeData != null)
            {
                baseNodeData.LinkedNodes.Add(targetNodeData);
            }
        }

        _translationPreview = DialogueNodeData[0].TranslateText();
    }

    public string Key => name;

#if UNITY_EDITOR
    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        DialogueContainer project = EditorUtility.InstanceIDToObject(instanceID) as DialogueContainer;
        if (project != null)
        {
            DialogueGraph.OpenAndLoadGraphWindow(project);
            return true;
        }
        return false;
    }
#endif
}
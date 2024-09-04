using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueContainer : ScriptableObject
{
    public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
    [SerializeReference] public List<NodeData> DialogueNodeData = new List<NodeData>();
    [SerializeField] [TextArea(1, 99999999)] string translation;

    public void LinkNodes()
    {
        foreach (var nodeLink in NodeLinks)
        {
            NodeData baseNodeData = DialogueNodeData.Find(x => x.GUID == nodeLink.BaseNodeGuid);
            NodeData targetNodeData = DialogueNodeData.Find(x => x.GUID == nodeLink.TargetNodeGuid);

            if (baseNodeData != null && targetNodeData != null)
            {
                baseNodeData.LinkedNodes.Add(targetNodeData);
            }
        }

        translation = DialogueNodeData[0].TranslateText();
        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.UI;

public class GraphSaveUtility
{
    private DialogueGraphView _targetGraphView;
    private DialogueContainer _containerCache;

    private List<Edge> Edges => _targetGraphView.edges.ToList();
    private List<DialogueNode> Nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();

    public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
    {
        return new GraphSaveUtility
        {
            _targetGraphView = targetGraphView
        };
    }

    public void SaveGraph(DialogueContainer container)
    {
        if(!Edges.Any()) return;

        var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();

        var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();

        for (int i = 0; i < connectedPorts.Length; i++)
        {
            var outputNode = connectedPorts[i].output.node as DialogueNode;
            var inputNode = connectedPorts[i].input.node as DialogueNode;

            dialogueContainer.NodeLinks.Add(new NodeLinkData
            {
                BaseNodeGuid = outputNode.GUID,
                BasePortIndex = outputNode.outputContainer.IndexOf(connectedPorts[i].output),
                PortName = connectedPorts[i].output.portName,
                TargetNodeGuid = inputNode.GUID
            });
        }

        DialogueNode entryNode = Nodes.Find(x => x.EntryPoint);
        dialogueContainer.DialogueNodeData.Add(new NodeData
        {
            GUID = entryNode.GUID,
            Position = entryNode.GetPosition().position
        });

        foreach (DialogueNode dialogueNode in Nodes.Where(node => !node.EntryPoint))
        {
            switch(dialogueNode.NodeType)
            {
                case NodeType.Choice:
                    List<ChoicePair> tempChoices = new List<ChoicePair>();
                    foreach(Port choicePort in dialogueNode.outputContainer.Children())
                    {
                        TextField choiceText = (TextField)choicePort.contentContainer[2];
                        TextField choiceValue = (TextField)choicePort.contentContainer[3];
                        tempChoices.Add(new ChoicePair { ChoiceText = choiceText.value, Value = choiceValue.value });
                    }

                    dialogueContainer.DialogueNodeData.Add(new ChoiceNodeData
                    {
                        GUID = dialogueNode.GUID,
                        Position = dialogueNode.GetPosition().position,
                        Choices = tempChoices,
                    });
                break;

                case NodeType.Dialogue:
                    dialogueContainer.DialogueNodeData.Add(new DialogueNodeData
                    {
                        GUID = dialogueNode.GUID,
                        DialogueLines = dialogueNode.mainContainer.Children().Where(x => x is TextField).Cast<TextField>().Select(x => x.value).ToArray(),
                        Speaker = dialogueNode.mainContainer.Q<ObjectField>().value as CharacterData,
                        Emotion = dialogueNode.mainContainer.Q<DropdownField>().value,
                        Position = dialogueNode.GetPosition().position,
                    });
                break;

                case NodeType.Label:
                    dialogueContainer.DialogueNodeData.Add(new LabelNodeData
                    {
                        GUID = dialogueNode.GUID,
                        Label = dialogueNode.mainContainer.Q<TextField>().value,
                        Position = dialogueNode.GetPosition().position,
                    });
                break;

                case NodeType.LabelJump:
                    dialogueContainer.DialogueNodeData.Add(new LabelJumpNodeData
                    {
                        GUID = dialogueNode.GUID,
                        Label = dialogueNode.inputContainer.Q<TextField>().value,
                        Position = dialogueNode.GetPosition().position,
                    });
                break;

                case NodeType.Background:
                    dialogueContainer.DialogueNodeData.Add(new BackgroundNodeData
                    {
                        GUID = dialogueNode.GUID,
                        Background = dialogueNode.mainContainer.Q<ObjectField>().value as Texture2D,
                        Position = dialogueNode.GetPosition().position,
                    });
                break;

                case NodeType.Conditional:
                    dialogueContainer.DialogueNodeData.Add(new ConditionalNodeData
                    {
                        GUID = dialogueNode.GUID,
                        Conditions = dialogueNode.outputContainer.Children().Select(x => x.Q<TextField>().value).ToArray(),
                        Position = dialogueNode.GetPosition().position,
                    });
                break;

                default:
                    dialogueContainer.DialogueNodeData.Add(new NodeData
                    {
                        GUID = dialogueNode.GUID,
                        Position = dialogueNode.GetPosition().position
                    });
                break;
            }
        }

        dialogueContainer.LinkNodes();
        string path = "New Dialogue";
        if(container != null) path = container.name;
        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Dialogue/Resources/{path}.asset");
        AssetDatabase.SaveAssets();
    }

    public void LoadGraph(DialogueContainer container)
    {
        _containerCache = Resources.Load<DialogueContainer>(container.name);

        if (_containerCache == null)
        {
            EditorUtility.DisplayDialog("File Not Found", "Target dialogue graph file does not exist!", "OK");
            return;
        }

        ClearGraph();
        CreateNodes();
        ConnectNodes();
    }

    private void ClearGraph()
    {
        Nodes.Find(x => x.EntryPoint).GUID = _containerCache.DialogueNodeData[0].GUID;

        foreach (var node in Nodes)
        {
            if (node.EntryPoint) continue;

            Edges.Where(x => x.input.node == node).ToList()
                .ForEach(edge => _targetGraphView.RemoveElement(edge));

            _targetGraphView.RemoveElement(node);
        }
    }

    private void CreateNodes()
    {
        foreach (var nodeData in _containerCache.DialogueNodeData)
        {
            if(nodeData.GUID == _containerCache.DialogueNodeData[0].GUID) continue;
            if(nodeData is ChoiceNodeData choiceData)
            {
                var generatedNode = _targetGraphView.GenerateChoiceNode("Choice", choiceData.Choices);
                generatedNode.GUID = nodeData.GUID;
                generatedNode.SetPosition(new Rect(nodeData.Position, _targetGraphView.DefaultNodeSize));
            }
            else if(nodeData is DialogueNodeData dialogueData)
            {
                var generatedNode = _targetGraphView.GenerateDialogueNode("Dialogue", 
                dialogueData.DialogueLines, dialogueData.Speaker, dialogueData.Emotion);
                generatedNode.GUID = nodeData.GUID;
                generatedNode.SetPosition(new Rect(nodeData.Position, _targetGraphView.DefaultNodeSize));
            }
            else if(nodeData is LabelNodeData labelData)
            {
                var generatedNode = _targetGraphView.GenerateLabelNode("Label", labelData.Label);
                generatedNode.GUID = nodeData.GUID;
                generatedNode.SetPosition(new Rect(nodeData.Position, _targetGraphView.DefaultNodeSize));
            }
            else if(nodeData is LabelJumpNodeData labelJumpData)
            {
                var generatedNode = _targetGraphView.GenerateLabelJumpNode("Goto", labelJumpData.Label);
                generatedNode.GUID = nodeData.GUID;
                generatedNode.SetPosition(new Rect(nodeData.Position, _targetGraphView.DefaultNodeSize));
            }
            else if(nodeData is BackgroundNodeData backgroundData)
            {
                var generatedNode = _targetGraphView.GenerateBackgroundNode("Background", backgroundData.Background);
                generatedNode.GUID = nodeData.GUID;
                generatedNode.SetPosition(new Rect(nodeData.Position, _targetGraphView.DefaultNodeSize));
            }
            else if(nodeData is ConditionalNodeData conditionalData)
            {
                var generatedNode = _targetGraphView.GenerateConditionalNode("Conditional", conditionalData.Conditions);
                generatedNode.GUID = nodeData.GUID;
                generatedNode.SetPosition(new Rect(nodeData.Position, _targetGraphView.DefaultNodeSize));
            }
        }
    }
    private void ConnectNodes()
    {
        foreach(NodeLinkData linkData in _containerCache.NodeLinks)
        {
            var baseNode = Nodes.First(x => x.GUID == linkData.BaseNodeGuid);
            var targetNode = Nodes.First(x => x.GUID == linkData.TargetNodeGuid);

            LinkNodes((Port)baseNode.outputContainer[linkData.BasePortIndex], (Port)targetNode.inputContainer[0]);
        }
    }
    private void LinkNodes(Port output, Port input)
    {
        var tempEdge = new Edge
        {
            output = output,
            input = input
        };

        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);
        _targetGraphView.Add(tempEdge);
    }
}
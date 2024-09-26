using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
{
    private EditorWindow _editorWindow;
    private DialogueGraphView _graphView;

    public void Init(EditorWindow editorWindow, DialogueGraphView graphView)
    {
        _editorWindow = editorWindow;
        _graphView = graphView;
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        var tree = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Create Elements"), 0),
            new SearchTreeEntry(new GUIContent("Dialogue Node"))
            {
                level = 1,
                userData = NodeType.Dialogue,
            },
            new SearchTreeEntry(new GUIContent("Choice Node"))
            {
                level = 1,
                userData = NodeType.Choice,
            },
            new SearchTreeEntry(new GUIContent("Label Node"))
            {
                level = 1,
                userData = NodeType.Label,
            },
            new SearchTreeEntry(new GUIContent("Label Jump Node"))
            {
                level = 1,
                userData = NodeType.LabelJump,
            },
            new SearchTreeEntry(new GUIContent("Conditional Node"))
            {
                level = 1,
                userData = NodeType.Conditional,
            },
            new SearchTreeEntry(new GUIContent("Background Node"))
            {
                level = 1,
                userData = NodeType.Background,
            },
        };

        return tree;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        var worldMousePosition = _editorWindow.rootVisualElement.ChangeCoordinatesTo(_editorWindow.rootVisualElement.parent, context.screenMousePosition - _editorWindow.position.position);
        var localMousePosition = _graphView.contentViewContainer.WorldToLocal(worldMousePosition);
        DialogueNode node;

        switch ((NodeType)SearchTreeEntry.userData)
        {
            case NodeType.Dialogue:
                node = _graphView.GenerateDialogueNode("Dialogue");
                break;
            case NodeType.Choice:
                node = _graphView.GenerateChoiceNode("Choice");
                break;
            case NodeType.Label:
                node = _graphView.GenerateLabelNode("Label");
                break;
            case NodeType.LabelJump:
                node = _graphView.GenerateLabelJumpNode("Goto");
                break;
            case NodeType.Conditional:
                node = _graphView.GenerateConditionalNode("Conditional");
                break;
            case NodeType.Background:
                node = _graphView.GenerateBackgroundNode("Background");
                break;
            default:
                node = _graphView.GenerateDialogueNode("Dialogue");
                break;
        }

        node.SetPosition(new Rect(localMousePosition, _graphView.DefaultNodeSize));
        return true;
    }
}

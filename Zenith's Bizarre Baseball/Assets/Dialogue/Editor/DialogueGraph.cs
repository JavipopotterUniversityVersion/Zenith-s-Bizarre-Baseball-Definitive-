using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView _graphView;
    private string _fileName = "New Narrative";

    [MenuItem("Graph/Dialogue Graph")]
    public static void OpenDialogueGraphWindow()
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Dialogue Graph");
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolBar();
        CreateMinimap();

        var window = GetWindow<DialogueGraph>();

        DragAndDrop.AddDropHandler(DropHandler);
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
        DragAndDrop.RemoveDropHandler(DropHandler);
    }

    private void ConstructGraphView()
    {
        _graphView = new DialogueGraphView()
        {
            name = "Dialogue Graph"
        };

        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }

    private void GenerateToolBar()
    {
        var toolbar = new UnityEditor.UIElements.Toolbar();

        var fileNameTextField = new TextField("File Name");
        fileNameTextField.SetValueWithoutNotify(_fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt => { _fileName = evt.newValue; });
        toolbar.Add(fileNameTextField);

        toolbar.Add(new Button(() => RequestDataOperation(true)) { text = "Save Data" });
        toolbar.Add(new Button(() => RequestDataOperation(false)) { text = "Load Data" });

        var nodeCreateButton = new Button(() => { _graphView.GenerateDialogueNode("Dialogue"); });
        nodeCreateButton.text = "Create Node";
        toolbar.Add(nodeCreateButton);

        var choiceCreateButton = new Button(() => { _graphView.GenerateChoiceNode("Choice"); });
        choiceCreateButton.text = "Create Choice";
        toolbar.Add(choiceCreateButton);

        var labelCreateButton = new Button(() => { _graphView.GenerateLabelNode("Label"); });
        labelCreateButton.text = "Create Label";
        toolbar.Add(labelCreateButton);

        var labelJumpCreateButton = new Button(() => { _graphView.GenerateLabelJumpNode("Goto"); });
        labelJumpCreateButton.text = "Create Label Jump";
        toolbar.Add(labelJumpCreateButton);

        var backgroundCreateButton = new Button(() => { _graphView.GenerateBackgroundNode("Background"); });
        backgroundCreateButton.text = "Create Background";
        toolbar.Add(backgroundCreateButton);

        var conditionalCreateButton = new Button(() => { _graphView.GenerateConditionalNode("Conditional"); });
        conditionalCreateButton.text = "Create Conditional";
        toolbar.Add(conditionalCreateButton);

        rootVisualElement.Add(toolbar);
    }

    private void CreateMinimap()
    {
        var miniMap = new MiniMap() { anchored = true };
        miniMap.SetPosition(new Rect(10, 30, 200, 140));
        _graphView.Add(miniMap);
    }

    private void RequestDataOperation(bool save)
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "OK");
            return;
        }

        var saveUtility = GraphSaveUtility.GetInstance(_graphView);

        if (save)
        {
            saveUtility.SaveGraph(_fileName);
        }
        else
        {
            saveUtility.LoadGraph(_fileName);
        }
    }

    DragAndDropVisualMode DropHandler(int id, string path, bool perform)
    {
        if (DragAndDrop.objectReferences.Length > 1) return DragAndDropVisualMode.Rejected;

        if (perform && DragAndDrop.objectReferences[0] is CharacterData)
        {
            _graphView.GenerateDialogueNode("Dialogue", null, (CharacterData) DragAndDrop.objectReferences[0]);
            return DragAndDropVisualMode.Link;
        }
        else if(!perform)
        {
            return DragAndDropVisualMode.Move;
        }

        return DragAndDropVisualMode.Rejected;
    }
}

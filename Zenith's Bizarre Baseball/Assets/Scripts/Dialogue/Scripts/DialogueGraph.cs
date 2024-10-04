using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView _graphView;
    public DialogueContainer container;

    [MenuItem("Graph/Dialogue Graph")]
    public static void OpenDialogueGraphWindow()
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Dialogue Graph");
    }

    public static void OpenAndLoadGraphWindow(DialogueContainer container)
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Dialogue Graph");
        window.container = container;

        window.RequestDataOperation(false);
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolBar();
        CreateMinimap();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }

    private void ConstructGraphView()
    {
        _graphView = new DialogueGraphView(this)
        {
            name = "Dialogue Graph"
        };

        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }

    private void GenerateToolBar()
    {
        var toolbar = new UnityEditor.UIElements.Toolbar();

        toolbar.Add(new Button(() => RequestDataOperation(true)) { text = "Save Data" });

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
        if (container == null)
        {
            EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "OK");
            return;
        }

        var saveUtility = GraphSaveUtility.GetInstance(_graphView);

        if (save)
        {
            saveUtility.SaveGraph(container);
        }
        else
        {
            saveUtility.LoadGraph(container);
        }
    }
}


#endif
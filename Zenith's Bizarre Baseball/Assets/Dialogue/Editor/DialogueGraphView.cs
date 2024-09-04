using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Linq;
using UnityEditor.UIElements;

public class DialogueGraphView : GraphView
{
    public readonly Vector2 DefaultNodeSize = new Vector2(150, 200);

    public DialogueGraphView()
    {
        styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));
        styleSheets.Add(Resources.Load<StyleSheet>("DialogueNode"));

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        AddElement(GenerateEntryPointNode());
    }

    private Port GeneratePort(Node node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
    }
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();
        ports.ForEach((port) =>
        {
            if (startPort != port && startPort.node != port.node)
            {
                compatiblePorts.Add(port);
            }
        });

        return compatiblePorts;
    } 

    public DialogueNode GenerateDialogueNode(string nodeName, string[] lines = null, CharacterData character = null, string expression = "")
    {
        DialogueNode node = CreateDialogueNode(nodeName, lines, character, expression);
        AddElement(node);

        return node;
    }
    public DialogueNode CreateDialogueNode(string nodeName, string[] lines = null, CharacterData character = null, string expression = "")
    {
        var dialogueNode = new DialogueNode()
        {
            title = nodeName,
            GUID = System.Guid.NewGuid().ToString(),
        };

        var characterField = new ObjectField()
        {
            objectType = typeof(CharacterData),
            allowSceneObjects = false
        };
        characterField.value = character;
        dialogueNode.mainContainer.Add(characterField);

        var expressionField = new DropdownField()
        {
            choices = characterField.value != null ? (characterField.value as CharacterData).ExpressionKeys.ToList() : new List<string>()
        };
        expressionField.value = expression;
        dialogueNode.mainContainer.Add(expressionField);

        characterField.RegisterValueChangedCallback(evt =>
        {
            expressionField.choices = (evt.newValue as CharacterData).ExpressionKeys.ToList();
        });

        var inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        dialogueNode.inputContainer.Add(inputPort);

        if (lines != null) foreach (var line in lines) AddDialogueLine(dialogueNode, line);
        else AddDialogueLine(dialogueNode);

        var button = new Button(() => { AddDialogueLine(dialogueNode); });
        button.text = "+";
        dialogueNode.titleButtonContainer.Add(button);

        var outputPort = GeneratePort(dialogueNode, Direction.Output);
        outputPort.portName = "Output";
        dialogueNode.outputContainer.Add(outputPort);

        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(Vector2.zero, DefaultNodeSize));

        return dialogueNode;
    }
    private void AddDialogueLine(DialogueNode node, string line = "")
    {
        var textField = new TextField(string.Empty);
        textField.value = line;
        node.mainContainer.Add(textField);
    }


    public DialogueNode GenerateChoiceNode(string nodeName)
    {
        List<ChoicePair> choicePairs = new List<ChoicePair>();
        choicePairs.Add(new ChoicePair());
        
        DialogueNode node = GenerateChoiceNode(nodeName, choicePairs);
        AddElement(node);

        return node;
    }
    public DialogueNode GenerateChoiceNode(string nodeName, List<ChoicePair> choicePairs)
    {
        DialogueNode node = CreateChoiceNode(nodeName, choicePairs);
        AddElement(node);

        return node;
    }
    public DialogueNode CreateChoiceNode(string nodeName, List<ChoicePair> choicePairs)
    {
        var choiceNode = new DialogueNode()
        {
            title = nodeName,
            GUID = System.Guid.NewGuid().ToString(),
            NodeType = NodeType.Choice
        };

        var InputPort = GeneratePort(choiceNode, Direction.Input, Port.Capacity.Single);
        InputPort.portName = "Input";
        choiceNode.inputContainer.Add(InputPort);

        foreach (var choice in choicePairs) AddChoicePort(choiceNode, choice);

        var button = new Button(() => { AddChoicePort(choiceNode); });
        button.text = "+";
        choiceNode.titleContainer.Add(button);

        return choiceNode;
    }

    private void AddChoicePort(DialogueNode node)
    {
        ChoicePair choice = new ChoicePair();
        AddChoicePort(node, choice);
    }
    private void AddChoicePort(DialogueNode node, ChoicePair choice)
    {
        var generatedPort = GeneratePort(node, Direction.Output);
        generatedPort.portName = $"out";

        var textField = new TextField(string.Empty);
        textField.value = choice.ChoiceText;
        generatedPort.contentContainer.Add(textField);
        textField.style.minWidth = 100;

        var valueField = new TextField(string.Empty);
        valueField.value = choice.Value;
        generatedPort.contentContainer.Add(valueField);
        valueField.style.minWidth = 100;

        var deleteButton = new Button(() => RemovePort(node, generatedPort)){ text = "X" };
        generatedPort.contentContainer.Add(deleteButton);

        node.outputContainer.Add(generatedPort);
        node.RefreshPorts();
        node.RefreshExpandedState();
    }
    private void RemovePort(DialogueNode node, Port port)
    {
        var targetEdge = edges.ToList().Where(x => x.output.portName == port.portName && x.output.node == port.node);
        if (targetEdge.Any())
        {
            var edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());
        }

        node.outputContainer.Remove(port);
        node.RefreshPorts();
        node.RefreshExpandedState();
    }

    private DialogueNode GenerateEntryPointNode()
    {
        var node = new DialogueNode()
        {
            title = "START",
            GUID = System.Guid.NewGuid().ToString(),
            EntryPoint = true
        };

        var generatedPort = GeneratePort(node, Direction.Output);
        generatedPort.portName = "Next";
        node.outputContainer.Add(generatedPort);

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(100, 200, 100, 150));
        return node;
    }


    public DialogueNode GenerateLabelNode(string nodeName, string label = "")
    {
        DialogueNode node = CreateLabelNode(nodeName, label);
        AddElement(node);

        return node;
    }
    public DialogueNode CreateLabelNode(string nodeName, string label)
    {
        var labelNode = new DialogueNode()
        {
            title = nodeName,
            GUID = System.Guid.NewGuid().ToString(),
            NodeType = NodeType.Label
        };

        var inputPort = GeneratePort(labelNode, Direction.Input, Port.Capacity.Single);
        inputPort.portName = "Input";
        labelNode.inputContainer.Add(inputPort);

        var textField = new TextField(string.Empty);
        textField.value = label;
        labelNode.mainContainer.Add(textField);

        var outputPort = GeneratePort(labelNode, Direction.Output);
        outputPort.portName = "Output";
        labelNode.outputContainer.Add(outputPort);

        labelNode.RefreshExpandedState();
        labelNode.RefreshPorts();
        labelNode.SetPosition(new Rect(Vector2.zero, DefaultNodeSize));

        return labelNode;
    }


    public DialogueNode GenerateBackgroundNode(string nodeName, Texture2D background = null)
    {
        DialogueNode node = CreateBackgroundNode(nodeName, background);
        AddElement(node);

        return node;
    }
    public DialogueNode CreateBackgroundNode(string nodeName, Texture2D background)
    {
        var backgroundNode = new DialogueNode()
        {
            title = nodeName,
            GUID = System.Guid.NewGuid().ToString(),
            NodeType = NodeType.Background
        };

        var inputPort = GeneratePort(backgroundNode, Direction.Input, Port.Capacity.Single);
        inputPort.portName = "Input";
        backgroundNode.inputContainer.Add(inputPort);

        var image = new ObjectField()
        {
            objectType = typeof(Texture2D),
            allowSceneObjects = false
        };

        var imagePreview = new Image();
        imagePreview.image = background;
        imagePreview.style.maxWidth = 500;
        imagePreview.style.maxHeight = 500;
        backgroundNode.mainContainer.Add(imagePreview);

        image.value = background;
        backgroundNode.titleContainer.Add(image);

        image.RegisterValueChangedCallback(evt =>
        {
            imagePreview.image = evt.newValue as Texture2D;
        });

        var outputPort = GeneratePort(backgroundNode, Direction.Output);
        outputPort.portName = "Output";
        backgroundNode.outputContainer.Add(outputPort);

        backgroundNode.RefreshExpandedState();
        backgroundNode.RefreshPorts();
        backgroundNode.SetPosition(new Rect(Vector2.zero, DefaultNodeSize));

        return backgroundNode;
    }


    public DialogueNode GenerateLabelJumpNode(string nodeName, string label = "")
    {
        DialogueNode node = CreateLabelJumpNode(nodeName, label);
        AddElement(node);

        return node;
    }
    public DialogueNode CreateLabelJumpNode(string nodeName, string label)
    {
        var labelJumpNode = new DialogueNode()
        {
            title = nodeName,
            GUID = System.Guid.NewGuid().ToString(),
            NodeType = NodeType.LabelJump
        };

        var inputPort = GeneratePort(labelJumpNode, Direction.Input, Port.Capacity.Single);
        inputPort.portName = "Input";
        labelJumpNode.inputContainer.Add(inputPort);

        var textField = new TextField(string.Empty);
        textField.value = label;
        labelJumpNode.inputContainer.Add(textField);

        labelJumpNode.RefreshExpandedState();
        labelJumpNode.RefreshPorts();
        labelJumpNode.SetPosition(new Rect(Vector2.zero, DefaultNodeSize));

        return labelJumpNode;
    }


    public DialogueNode GenerateConditionalNode(string nodeName, string[] conditions = null)
    {
        DialogueNode node = CreateConditionalNode(nodeName, conditions);
        AddElement(node);

        return node;
    }
    public DialogueNode CreateConditionalNode(string nodeName, string[] conditions)
    {
        var conditionalNode = new DialogueNode()
        {
            title = nodeName,
            GUID = System.Guid.NewGuid().ToString(),
            NodeType = NodeType.Conditional
        };

        var inputPort = GeneratePort(conditionalNode, Direction.Input, Port.Capacity.Single);
        inputPort.portName = "Input";
        conditionalNode.inputContainer.Add(inputPort);

        if (conditions != null) foreach (var condition in conditions) AddConditionalPort(conditionalNode, condition);
        else AddConditionalPort(conditionalNode);

        var button = new Button(() => { AddConditionalPort(conditionalNode); });
        button.text = "+";
        conditionalNode.titleContainer.Add(button);

        conditionalNode.RefreshExpandedState();
        conditionalNode.RefreshPorts();
        conditionalNode.SetPosition(new Rect(Vector2.zero, DefaultNodeSize));

        return conditionalNode;
    }
    private void AddConditionalPort(DialogueNode node, string condition = "")
    {
        var generatedPort = GeneratePort(node, Direction.Output);
        generatedPort.portName = $"out";

        var textField = new TextField(string.Empty);
        textField.value = condition;
        generatedPort.contentContainer.Add(textField);
        textField.style.minWidth = 100;

        var deleteButton = new Button(() => RemovePort(node, generatedPort)){ text = "X" };
        generatedPort.contentContainer.Add(deleteButton);

        node.outputContainer.Add(generatedPort);
        node.RefreshPorts();
        node.RefreshExpandedState();
    }
}

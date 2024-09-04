using System;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class NodeData
{
    public string GUID;
    public Vector2 Position;
    public List<NodeData> LinkedNodes = new List<NodeData>();
    public virtual string TranslateText() => "";
}

[Serializable]
public class DialogueNodeData : NodeData
{
    public CharacterData Speaker;
    public string Emotion;
    public string[] DialogueLines;
    public string[] lines;

    public override string TranslateText()
    {
        string value = $"<{Speaker.CharacterName}><{Emotion}> {DialogueLines.ForEach(x => x + "@line@")}";

        if(LinkedNodes.Count > 0) value += LinkedNodes[0].TranslateText();
        
        return value;
    }
}

[Serializable]
public class BackgroundNodeData : NodeData
{
    public Texture2D Background;

    public override string TranslateText()
    {
        string value = $"<background:{Background.name}>";

        if(LinkedNodes.Count > 0) value += LinkedNodes[0].TranslateText();
        
        return value;
    }
}

[Serializable]
public class LabelNodeData : NodeData
{
    public string Label;

    public override string TranslateText()
    {
        string value = $"<label:{Label}>";

        if(LinkedNodes.Count > 0) value += LinkedNodes[0].TranslateText();
        
        return value;
    }
}

[Serializable]
public class LabelJumpNodeData : NodeData
{
    public string Label;

    public override string TranslateText()
    {
        string value = $"<goto:{Label}>";

        if(LinkedNodes.Count > 0) value += LinkedNodes[0].TranslateText();
        
        return value;
    }
}

[Serializable]
public class ConditionalNodeData : NodeData
{
    public string[] Conditions;

    public override string TranslateText()
    {
        string value = "";

        for(int i = 0; i < Conditions.Length; i++) value += $"<if:{Conditions[i]}>" + LinkedNodes[i].TranslateText() + $"<endif>";

        return value;
    }
}

[Serializable]
public class ChoiceNodeData : NodeData
{
    public List<ChoicePair> Choices = new List<ChoicePair>();

    public override string TranslateText()
    {
        string value = $"<choice:{Choices.ForEach(x => x.ToString() + ",").Where(x => x != Choices.Last())}{Choices.Last()}>";

        for(int i = 0; i < LinkedNodes.Count; i++) value += $"<label:{Choices[i].ChoiceText}>" + LinkedNodes[i].TranslateText();

        return value;
    }
}

[Serializable]
public class ChoicePair
{
    public string ChoiceText = "";
    public string Value = "";

    public static bool EvaluateChoice(ChoicePair pair, Processor evaluator, out string choiceText)
    {
        choiceText = pair.ChoiceText;
        return evaluator.ResultOf(pair.Value, 1) == 1;
    }

    public override string ToString() => $"{ChoiceText}/{Value}";
}
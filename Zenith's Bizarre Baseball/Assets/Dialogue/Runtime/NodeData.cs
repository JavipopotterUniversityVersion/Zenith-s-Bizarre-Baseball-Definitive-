using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class NodeData
{
    public string GUID;
    public Vector2 Position;
    public List<NodeData> LinkedNodes = new List<NodeData>();
    public virtual string TranslateText() => LinkedNodes.Count > 0 ? LinkedNodes[0].TranslateText() : "";
}

[Serializable]
public class DialogueNodeData : NodeData
{
    public CharacterData Speaker;
    public string Emotion;
    public string[] DialogueLines;

    public override string TranslateText()
    {
        string value = $"<{Speaker.CharacterName}><{Emotion}>";

        for(int i = 0; i < DialogueLines.Length; i++)
        {
            value += "<an:Talk>" + DialogueLines[i] + "<an:Idle>" + "@line";
        }

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
        string value = $"<BACKGROUND:{Background.name}>";

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
        string value = $"<LABEL:{Label}>";

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
        string value = $"<GOTO:{Label}>";

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

        for(int i = 0; i < Conditions.Length; i++) value += $"<IF:{Conditions[i]}>" + LinkedNodes[i].TranslateText() + $"<ENDIF>";

        return value;
    }
}

[Serializable]
public class ChoiceNodeData : NodeData
{
    public List<ChoicePair> Choices = new List<ChoicePair>();

    public override string TranslateText()
    {
        string value = $"<CHOICE:";

        for(int i = 0; i < Choices.Count; i++) 
        {
            value += Choices[i].ToString() + (i < Choices.Count - 1 ? ",," : "");
        }

        value += ">";

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
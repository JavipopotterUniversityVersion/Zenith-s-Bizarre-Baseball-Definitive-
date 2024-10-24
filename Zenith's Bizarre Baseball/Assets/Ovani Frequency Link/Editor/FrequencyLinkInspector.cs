using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[CustomEditor(typeof(FrequencyLink))]
public class FrequencyLinkInspector : Editor
{
    public static List<FrequencyLinkInspector> FrequencyLinkInspectors = new List<FrequencyLinkInspector>();
    public FrequencyLink link;
    public VisualElement Graph;
    public VisualTreeAsset m_InspectorXML;
    public VisualElement[] elements = new VisualElement[FrequencyLink.SamplingRes];
    public MinMaxSlider slider;
    public MinMaxSlider volSlider;
    public override VisualElement CreateInspectorGUI()
    {
        link = (FrequencyLink)this.target;
        FrequencyLinkInspectors.Add(this);
        // Create a new VisualElement to be the root of our Inspector UI.
        VisualElement myInspector = new VisualElement();

        // Load from default reference.
        m_InspectorXML.CloneTree(myInspector);

        Graph = myInspector.Query<VisualElement>("Graph");

        slider = myInspector.Query<MinMaxSlider>("slid");
        slider.SetValueWithoutNotify(((FrequencyLink)target).MeasureRange);
        slider.highLimit = FrequencyLink.SamplingRes / FrequencyLink.ViewingDivisor;

        volSlider = myInspector.Query<MinMaxSlider>("vols");
        volSlider.SetValueWithoutNotify(((FrequencyLink)target).VolRange);

        for (int i = 0; i < FrequencyLink.SamplingRes / FrequencyLink.ViewingDivisor; i++)
        {
            VisualElement ve = new VisualElement();
            ve.style.width = 10000;
            ve.style.backgroundColor = Color.white;
            ve.style.height = new StyleLength(new Length(50, LengthUnit.Percent));
            Graph.Add(ve);
            elements[i] = ve;
        }
        
        // Return the finished Inspector UI.
        return myInspector;
    }
    public override bool RequiresConstantRepaint()
    {
        return true;
    }
    private void OnDestroy()
    {
        FrequencyLinkInspectors.Remove(this);
    }
}

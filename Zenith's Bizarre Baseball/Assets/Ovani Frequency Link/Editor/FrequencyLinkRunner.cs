using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.UIElements;

[InitializeOnLoadAttribute]
public static class FrequencyLinkRunner
{

    static FrequencyLinkRunner()
    {
        FrequencyToMovement.FreqMoves = Object.FindObjectsOfType<FrequencyToMovement>(false).ToList();
        EditorApplication.update += () =>
        {
            foreach (FrequencyToMovement ftm in FrequencyToMovement.FreqMoves)
                ftm.Move();
            foreach (var spec in FrequencyLinkInspector.FrequencyLinkInspectors)
            {

                spec.link.MeasureRange = spec.slider.value;
                spec.link.VolRange = spec.volSlider.value;

                float[] vals = spec.link.GetRange();
                for (int i = 0; i < FrequencyLink.SamplingRes / FrequencyLink.ViewingDivisor; i++) 
                {
                    spec.elements[i].style.height = new StyleLength(new Length((vals[i] * 99) + 1, LengthUnit.Percent));
                    spec.elements[i].style.minWidth = spec.Graph.resolvedStyle.width / spec.Graph.childCount;

                    spec.elements[i].style.backgroundColor = (i >= spec.link.MeasureFrom && i < spec.link.MeasureTo) ? Color.red : Color.white;

                }
                spec.Repaint();
            }
        };
    }
}

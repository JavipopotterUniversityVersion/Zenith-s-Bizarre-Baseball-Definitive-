using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenuAttribute()]
public class FrequencyLink : ScriptableObject
{
    public Vector2 MeasureRange;
    public Vector2 VolRange;
    public int MeasureFrom => (int)MeasureRange.x;
    public int MeasureTo => (int)MeasureRange.y;
    public float MinimumVolume => VolRange.x;
    public float MaximumVolume => VolRange.y;
    public float SmoothingUp = .232f;
    public float SmoothingDown = .232f;
    public float Scalar = 1;
    public FFTWindow SamplingMode;
    public List<LinkerTag> Sources = new List<LinkerTag>();

    public const int SamplingRes = 1024;
    public const int ViewingDivisor = 4;
    
    private void _getSourceStrength(AudioSource source, float[] o)
    {
        source.GetSpectrumData(o, 0, SamplingMode);
    }

    private float[] _floatsC = new float[SamplingRes];
    private float[] _floatsD = new float[SamplingRes];
    private float[] _lastRange = new float[SamplingRes];

    private float _lastMeasure; // time of last GetStrength call
    private float _lastValue; // SHOULD BE RESET IF _lastMeasure IS >1s OLD
    private float _curTime()
    {
#if UNITY_EDITOR
        var dt = DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime();
        return dt.Seconds + (dt.Milliseconds / 1000f);
#else
        return Time.realtimeSinceStartup;
#endif
    }
    private float delta => _curTime() - _lastMeasure;
    public float[] GetRange()
    {
        GetStrength();
        return _floatsD;
    }
    public float GetStrength()
    {
        if (_lastMeasure > _curTime())
        {
            _lastMeasure = 0;
        }
        if (delta < .01f)
        {
            return _lastValue;
        }


        SmoothingUp = Mathf.Clamp(SmoothingUp, 0.01f, .5f);
        SmoothingDown = Mathf.Clamp(SmoothingDown, 0.01f, .5f);

        if (MeasureFrom < 0 || MeasureTo >= SamplingRes || MeasureTo < MeasureFrom || MinimumVolume > MaximumVolume || Sources.Count == 0)
        {
            return 0.0f;
        }

        Sources.RemoveAll(l => l == null);
        Sources = Sources.Distinct().ToList();
        int sourceCount = Sources.Count;
        Array.Clear(_floatsD, 0, SamplingRes);
        foreach (var source in Sources)
        {
            if (source == null || source.AudioSource == null)
            {
                sourceCount -= 1;
                if (sourceCount == 0)
                {
                    return 0;
                }
                continue;
            }
            _getSourceStrength(source.AudioSource, _floatsC);
            for (int i = 0; i < SamplingRes; i++)
                _floatsD[i] += _floatsC[i];
        }
        if (sourceCount == 0)
        {
            return 0.0f;
        }
        for (int i = 0; i < SamplingRes; i++)
            _floatsD[i] = Mathf.Clamp01(Mathf.Sqrt(_floatsD[i]/sourceCount) * 1.5f * Scalar);

        if (delta < 1)
            for (int i = 0; i < SamplingRes; i++)
            {
                _floatsD[i] = Mathf.Lerp(_lastRange[i], _floatsD[i], .5f - (2 * (_floatsD[i] > _lastRange[i] ? SmoothingUp : SmoothingDown)));
            }


        // we gotta get the peak value from _floatsD
        _lastValue = 0.0f;
        for (int i = MeasureFrom; i < MeasureTo; i++)
            if (_floatsD[i] > _lastValue)
                _lastValue = _floatsD[i];


        _lastMeasure = _curTime();
        if (sourceCount != Sources.Count)
            Sources = Sources.Where(s => s != null).ToList();
        for (int i = 0; i < SamplingRes; i++)
            _lastRange[i] = _floatsD[i];

        _lastValue = Mathf.Clamp(_lastValue, MinimumVolume, MaximumVolume) - MinimumVolume;
        _lastValue /= MaximumVolume;
        return _lastValue;
    }
}

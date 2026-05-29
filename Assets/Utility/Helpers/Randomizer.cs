using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class Randomizer
{
    [SerializeField]
    private float decimalPlaces = 0;

    private List<ProbabilityData> probabilities;
    private float total = 0;

    public Randomizer(float _decimalPlaces, List<ProbabilityData> _probabilities)
    {
        decimalPlaces = _decimalPlaces;
        probabilities = _probabilities;
        foreach(ProbabilityData data in probabilities)
        {
            total += data.value;
        }
    }

    public Randomizer(List<ProbabilityData> _probabilities)
    {
        probabilities = _probabilities;
        foreach(ProbabilityData data in probabilities)
        {
            total += data.value;
        }
    }

    public object Run()
    {
        float randomValue = Random.Range(0, total + (1 / Mathf.Max(decimalPlaces / 10, 1)));
        float border = 0;
        foreach(ProbabilityData data in probabilities)
        {
            border += data.value;
            if(randomValue <= border)
            {
                return data.output;
            }
        }

        return probabilities[probabilities.Count - 1].output;
    }

    [Serializable]
    public struct ProbabilityData
    {
        public float value;
        public object output;

        public ProbabilityData(float _value, object _output)
        {
            value = _value;
            output = _output;
        }
    }
}

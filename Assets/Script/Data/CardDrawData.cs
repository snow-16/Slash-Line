using System;
using System.Collections.Generic;
using UnityEngine;

public class CardDrawData : MonoBehaviour
{
    [SerializeField]
    private List<DrawData> drawDatas;

    private Randomizer drawRandomizer;

    void Start()
    {
        List<Randomizer.ProbabilityData> inputDatas = new();
        foreach(DrawData data in drawDatas)
        {
            inputDatas.Add(new Randomizer.ProbabilityData(data.value, data.rarelity));
        }
        drawRandomizer = new(inputDatas);
    }

    public Randomizer DrawRandomizer
    {
        get{return drawRandomizer;}
    }

    [Serializable]
    public struct DrawData
    {
        public float value;
        public CrystalAndBlades.CAndBRarelity rarelity;
    }
}

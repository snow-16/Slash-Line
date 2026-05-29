using System;
using System.Collections.Generic;
using UnityEngine;

public class StageDatas : MonoBehaviour
{
    [SerializeField]
    private List<SpawnInStages> stageDatas;

    private Randomizer crystalRandomizer;
    private Randomizer enemyRandomizer;

    public int GenerateRandomizer(int stage)
    {
        List<Randomizer.ProbabilityData> inputDatas = new();
        foreach(CrystalSpawnData data in stageDatas[stage - 1].crystalDatas)
        {
            inputDatas.Add(new Randomizer.ProbabilityData(data.value, data.count));
        }
        crystalRandomizer = new(new(inputDatas));

        inputDatas.Clear();
        foreach(EnemySpawnData data in stageDatas[stage - 1].enemyDatas)
        {
            inputDatas.Add(new Randomizer.ProbabilityData(data.value, (data.count, data.hp)));
        }    
        enemyRandomizer = new(new(inputDatas));

        return stageDatas[stage - 1].requestWoods;
    }

    public Randomizer CrystalRandomizer
    {
        get{return crystalRandomizer;}
    }

    public Randomizer EnemyRandomizer
    {
        get{return enemyRandomizer;}
    }

    [Serializable]
    public struct SpawnInStages
    {
        public int requestWoods;
        public List<CrystalSpawnData> crystalDatas;
        public List<EnemySpawnData> enemyDatas;
    }

    [Serializable]
    public struct CrystalSpawnData
    {
        public float value;
        public int count;
    }

    [Serializable]
    public struct EnemySpawnData
    {
        public float value;
        public int count;
        public int hp;
    }
}

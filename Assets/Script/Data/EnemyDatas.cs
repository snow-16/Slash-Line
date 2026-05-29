using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDatas : MonoBehaviour
{
    [SerializeField]
    private List<EnemyData> datas;

    private Dictionary<int, EnemyData> enemyDataBase = new();

    void Start()
    {
        foreach(var data in datas)
        {
            enemyDataBase.Add(data.hp, data);
        }
    }

    public EnemyData FindData(int hp)
    {
        if(enemyDataBase.ContainsKey(hp))
        {
            return enemyDataBase[hp];
        }
        
        Debug.LogError($"HP:{hp}に対応するEnemyDatasのデータが見つかりませんでした。");
        return default;
    }

    [Serializable]
    public struct EnemyData
    {
        public int hp;
        public int haveWoods;
        public Sprite alive_sprite;
        public Sprite dead_sprite;
        public float surviveTimeMin;
        public float surviveTimeMax;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrystalAndBlades: MonoBehaviour
{
    [SerializeField]
    CAndBDatas pure;
    [SerializeField]
    List<CAndBDatas> commons = new();
    [SerializeField]
    List<CAndBDatas> un_commons = new();
    [SerializeField]
    List<CAndBDatas> rares = new();
    [SerializeField]
    List<CAndBDatas> catastrophes = new();

    private List<CAndBDatas>[] cAndBDatas = new List<CAndBDatas>[4];
    private List<CAndBDatas>[] drawableDatas = new List<CAndBDatas>[4];
    private Dictionary<CAndBType, CAndBDatas> cAndBDataBase = new();

    public enum CAndBType
    {
        //Base
        EMPTY,
        PURE,

        //Common
        GLASS,
        STONE,
        LEAD,

        //UnCommon
        OBSIDIAN,
        PAPER,
        AMETHYST,

        //Rare
        QUARTZ,
        FLOAT_STONE,
        ALEXANDRITE,

        //Catastrophe
        DIAMOND,
        OSMIUM,
        SOUL_STONE,
        CURSE_DIAMOND,
    }

    public enum CAndBRarelity
    {
        COMMON,
        UNCOMMON,
        RARE,
        CATASTROPHE
    }

    void Start()
    {
        cAndBDatas[0] = commons;
        cAndBDatas[1] = un_commons;
        cAndBDatas[2] = rares;
        cAndBDatas[3] = catastrophes;
        cAndBDatas[0].Insert(0, pure);
        ResetData();

        //アクセスしやすくするため cAndBDatas をDictionaryに纏め直す。
        for(int i = 0; i < 4; i++)
        {
            foreach(var data in cAndBDatas[i])
            {
                cAndBDataBase.Add(data.type, data);
            }
        }
    }

    public CAndBDatas FindData(CAndBType type)
    {
        if(cAndBDataBase.ContainsKey(type))
        {
            return cAndBDataBase[type];
        }
        
        Debug.LogError($"{type}に対応するCrystalAndBladesのデータが見つかりませんでした。");
        return default;
    }

    public void ResetData()
    {
        drawableDatas = cAndBDatas.Select(item => new List<CAndBDatas>(item)).ToArray();
        drawableDatas[0].RemoveAt(0);
    }

    public List<CAndBDatas>[] AllDatas
    {
        set{drawableDatas = value;}
        get{return drawableDatas;}
    }

    [Serializable]
    public struct CAndBDatas
    {
        public string name;
        public CAndBType type;
        public Color crystal_color;
        public float weight;
        public float damage;
        public int usableCount;
        public float collectRate;
        public string detail;
        public float surviveTimeMin;
        public float surviveTimeMax;
    }
}

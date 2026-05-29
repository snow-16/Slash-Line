using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FieldGenerator : ChildSystem
{
    [SerializeField]
    GameObject enemy;
    [SerializeField]
    GameObject crystal;

    [SerializeField]
    private int maxEnemyCount;
    [SerializeField]
    private int maxCeystalCount;
    [SerializeField]
    private float generateEnemyCooldown;
    [SerializeField]
    private float generateCrystalCooldown;

    private int enemyCount = 0;
    private int crystalCount = 0;
    private List<GameObject> onFieldObjects = new();

    private InstantGenerator crystalGenerator;
    private InstantGenerator enemyGenerator;

    private Coroutine[] generateCoroutines = new Coroutine[2];

    protected override void Start()
    {
        base.Start();
        crystalGenerator = gameObject.AddComponent<InstantGenerator>().Set(crystal);
        enemyGenerator = gameObject.AddComponent<InstantGenerator>().Set(enemy);
        onFieldObjects.Add(Mother.Slasher.gameObject);
    }

    public void StartPlay()
    {
        GenerateCrystal();
        GenerateEnemy();

        generateCoroutines[0] = StartCoroutine(LoopGeneCrystal());
        generateCoroutines[1] = StartCoroutine(LoopGeneEnemy());
    }

    IEnumerator LoopGeneCrystal()
    {
        while(true)
        {
            yield return new WaitForSeconds(generateCrystalCooldown);
            while(!Mother.CanMoveScene())
            {
                yield return null;
            }
            GenerateCrystal();
        }
    }
    private void GenerateCrystal()
    {
        crystalCount = Mother.CountContains($"{crystal.name}:");
        int count = Mathf.Min(Mother.RunCrystalRandomizer(), maxCeystalCount - crystalCount);
        for(int i = 0; i < count; i++)
        {
            GameObject obj = RunGenerator(crystalGenerator);
            onFieldObjects.Add(obj);

            CrystalCore crystalCore = obj.GetComponent<CrystalCore>();
            CrystalAndBlades.CAndBType[] containsTypes = Mother.Slasher.ContaintsTypes;
            CrystalAndBlades.CAndBType generateType = containsTypes[Random.Range(0, 4 - containsTypes.Count(item => item == CrystalAndBlades.CAndBType.EMPTY))];
            crystalCore.SetType(generateType);
        }
    }

    IEnumerator LoopGeneEnemy()
    {
        while(true)
        {
            yield return new WaitForSeconds(generateEnemyCooldown);
            while(!Mother.CanMoveScene())
            {
                yield return null;
            }
            GenerateEnemy();
        }
    }
    private void GenerateEnemy()
    {
        enemyCount = Mother.CountContains(enemy.name);
        var enemyType = Mother.RunEnemyRandomizer();
        int count = Mathf.Min(enemyType.Item1, maxEnemyCount - enemyCount);
        for(int i = 0; i < count; i++)
        {
            GameObject obj = RunGenerator(enemyGenerator);
            onFieldObjects.Add(obj);

            EnemyCore enemyCore = obj.GetComponent<EnemyCore>();
            EnemyDatas.EnemyData data = Mother.FindEnemyData(enemyType.Item2);
            enemyCore.SurviveTime = Random.Range(data.surviveTimeMin, data.surviveTimeMax + 1);
            enemyCore.HP = enemyType.Item2;
        }
    }

    //画面内の、他オブジェクトと被らないランダムな位置にオブジェクトを生成
    private GameObject RunGenerator(InstantGenerator generator)
    {
        Vector2? pos = null;
        float[] paddings = Mother.Slasher.Paddings;
        float addPadding = 50f;

        onFieldObjects.RemoveAll(item => item == null);
        while(pos == null)
        {
            pos = ScreenCore.RandomPositionInPaddingScreen(paddings[0] + addPadding, paddings[0] + addPadding, paddings[1] + addPadding, paddings[2] + addPadding);
            foreach(GameObject p in onFieldObjects)
            {
                if(Vector2.Distance(p.transform.position, pos.Value) < 2.5f)
                {
                    pos = null;
                    break;
                }
            }
        }

        GameObject obj = generator.Generate(pos.Value, generator.Identity());
        InstantObject obj_script = obj.GetComponent<InstantObject>();
        obj_script.LateSetMother(Mother);
        return obj;
    }

    public void Refresh()
    {
        StopCoroutine(generateCoroutines[0]);
        StopCoroutine(generateCoroutines[1]);
        enemyCount = 0;
        crystalCount = 0;
        ResetField();
    }

    public void ResetField()
    {
        foreach(GameObject obj in onFieldObjects)
        {
            if(obj != null && obj.tag != "Slasher")
            {
                Destroy(obj);
            }
        }
        onFieldObjects.RemoveAll(item => item == null);
    }

    public void StopField()
    {
        foreach(GameObject obj in onFieldObjects)
        {
            if(obj != null)
            {
                if(obj.tag == "Enemy")
                {
                    obj.GetComponent<EnemyCore>().Stop();
                }
                else if(obj.tag == "Crystal")
                {
                    obj.GetComponent<CrystalCore>().Stop();
                }
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-10)]
public class MotherSystem : MonoBehaviour
{
    public int maxLife;

    private static bool created = false;

    Dictionary<string, GameObject> existObjects = new();

    private bool isTitle = true;
    private bool isLoading = false;
    private bool isPosing = false;
    private bool isPlaying = false;
    private bool isWaitingEnd = false;

    private int stage = 1;

    private SlasherCore slasher;
    private FieldGenerator fieldGenerator;
    private UIHub uIHub;
    private TitleHub titleHub;
    private SceneWallUI sceneWall;

    private CrystalAndBlades cAndBs;
    private EnemyDatas enemyDatas;
    private StageDatas stageDatas;
    private CardDrawData cardDrawData;
    private SoundSystem soundSystem;

    void Start()
    {
        if(!created)
        {
            QualitySettings.vSyncCount = 0; 
            Application.targetFrameRate = 30;
            DontDestroyOnLoad(gameObject);

            cAndBs = GetComponent<CrystalAndBlades>();
            enemyDatas = GetComponent<EnemyDatas>();
            stageDatas = GetComponent<StageDatas>();
            soundSystem = GetComponent<SoundSystem>();
            cardDrawData = GetComponent<CardDrawData>();

            created = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InputObject(GameObject obj)
    {
        switch(obj.tag)
        {
            case "Slasher":
                slasher = obj.GetComponent<SlasherCore>();;
                break;
            case "FieldGenerator":
                fieldGenerator = obj.GetComponent<FieldGenerator>();
                break;
            case "UIHub":
                uIHub = obj.GetComponent<UIHub>();
                break;
            case "TitleHub":
                titleHub = obj.GetComponent<TitleHub>();
                break;
            case "SceneWall":
                sceneWall = obj.GetComponent<SceneWallUI>();
                break;
            case "DontAddExists":
                break;
            default:
                if(!existObjects.ContainsKey(obj.name))
                {
                    existObjects.Add(obj.name, obj);
                }
                else
                {
                    Debug.LogError($"{obj.name}はすでに登録済みです。");
                }
                break;
        }
    }

    public void RemoveObject(GameObject obj)
    {
        if(!existObjects.ContainsKey(obj.name) && !isLoading)
        {
            Debug.LogError($"{obj.name}はMotherSystemに登録されていないため削除できません。");
            return;
        }
        existObjects.Remove(obj.name);
    }

    public GameObject AccessObject(string name)
    {
        if(!existObjects.ContainsKey(name))
        {
            Debug.LogError($"{name}はMotherSystemに登録されていないためアクセスできません。");
            return null;
        }

        return existObjects[name];
    }

    public int CountContains(string name)
    {
        int count = 0;
        foreach(var check in existObjects)
        {
            if(check.Key.Contains(name))
            {
                count++;
            }
        }
        return count;
    }

    public SlasherCore Slasher
    {
        get {return slasher;}
    }

    public FieldGenerator FieldGenerator
    {
        get {return fieldGenerator;}
    }

    public bool IsTitle
    {
        get{ return isTitle; }
    }

    public bool IsPosing
    {
        get{ return isPosing; }
    }

    public bool IsPlaying
    {
        get{ return isPlaying; }
    }

    public bool IsWaitingEnd
    {
        get{ return isWaitingEnd; }
    }

    public bool IsLoading
    {
        get{ return isLoading; }
    }

    public int Stage
    {
        get{return stage;}
    }

    public SoundSystem Sound
    {
        get{ return soundSystem; }
    }

    public List<CrystalAndBlades.CAndBDatas>[] AllDatas
    {
        set{cAndBs.AllDatas = value;}
        get{return cAndBs.AllDatas;}
    }



    public CrystalAndBlades.CAndBDatas FindCAndBData(CrystalAndBlades.CAndBType type)
    {
        return cAndBs.FindData(type);
    }

    public EnemyDatas.EnemyData FindEnemyData(int hp)
    {
        return enemyDatas.FindData(hp);
    }

    public int RunCrystalRandomizer()
    {
        return (int)stageDatas.CrystalRandomizer.Run();
    }

    public (int, int) RunEnemyRandomizer()
    {
        return ((int, int))stageDatas.EnemyRandomizer.Run();
    }

    public CrystalAndBlades.CAndBRarelity RunDrawRandmizer()
    {
        return (CrystalAndBlades.CAndBRarelity)cardDrawData.DrawRandomizer.Run();
    }

    public void ReloadUI()
    {
        uIHub.ReloadUI();
    }

    public void LeaveTitle()
    {
        isTitle = false;
        titleHub.StartGame();
    }

    public void GoToField()
    {
        SceneManager.LoadScene("Field");
        isLoading = true;
    }

    public void ArrivedField()
    {
        isLoading = false;
        StartPlay();
        SetStage();
        sceneWall.OpenWall();
    }

    public void StartCountDown()
    {
        uIHub.StartCountDown();
    }

    public void CloseField()
    {
        uIHub.BlindMenu();
        uIHub.BlindPose();
        sceneWall.CloseWall();
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
        isLoading = true;
    }

    public void ArrivedTitle()
    {
        if(isLoading)
        {
            isLoading = false;
            isTitle = true;
            titleHub.BackTitle();
        }
    }

    public void SetStage()
    {
        int request = stageDatas.GenerateRandomizer(stage);
        slasher.RequestWoods = request;
    }

    public void StartPlay()
    {
        stage = 1;
        cAndBs.ResetData();
        slasher.RemoveContainers();
        slasher.ResetLife();
    }

    public void StartStage()
    {
        isPlaying = true;
        isPosing = false;

        uIHub.StartPlay();
        fieldGenerator.StartPlay();
    }

    public void PoseGame()
    {
        isPosing = true;
        uIHub.Pose();
    }

    public void ReStartGame()
    {
        isPosing = false;
        uIHub.Restart();
    }

    public void EndGame()
    {
        isPlaying = false;
        isWaitingEnd = true;
        fieldGenerator.StopField();
    }

    public void ResultGame(bool isClear, bool canRespawn)
    {
        isWaitingEnd = false;
        uIHub.ReloadUI();
        uIHub.ShowResult(isClear, canRespawn);
    }

    public void SelectCard()
    {
        isWaitingEnd = false;
        uIHub.DrawCard();
    }

    public void CollectCard(CrystalAndBlades.CAndBType type)
    {
        uIHub.HideCards();
        slasher.AddContainer(type);
        NextStage();
    }

    public void NextStage()
    {
        stage++;
        RefreshField();
        SetStage();
        uIHub.NextStage();
        StartCountDown();
    }

    public void RefreshField()
    {
        slasher.Refresh();
        fieldGenerator.Refresh();
        uIHub.BlindMenu();
        ReloadUI();
    }

    public bool CanMoveScene()
    {
        return isPlaying && !isPosing;
    }
}

using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-4)]
public class UIHub : ChildSystem
{
    [SerializeField]
    private SpriteRenderer backGround;
    [SerializeField]
    private Transform fieldMarker;

    private WeaponViewUI weapon;
    private CrystalGaugeUI[] crystals = new CrystalGaugeUI[4];
    private WeightViewUI weight;
    private StageLampUI _stageLamp;
    private WoodGaugeUI woodGauge;
    private PoseUI pose;
    private LifeViewUI lifeView;
    private Transform shadow;
    private CountDownUI countDown;
    private GameObject[] buttons = new GameObject[2];
    private GameObject[] resultMassages;
    private CardSelectUI cardSelect;
    
    private Transform[] fieldMarkers;

    private Vector2 saveScreenSize = new();

    

    protected override void Start()
    {
        base.Start();
        fieldMarkers = new Transform[]{fieldMarker.GetChild(0), fieldMarker.GetChild(1), fieldMarker.GetChild(2)};
        
        shadow = transform.GetChild(0).GetChild(0);

        Transform under = transform.GetChild(1);
        weapon = under.GetChild(1).GetComponent<WeaponViewUI>();
        for(int i = 0; i < 4; i++)
        {
            crystals[i] = under.GetChild(i + 2).GetComponent<CrystalGaugeUI>();
        }
        weight = under.GetChild(6).GetComponent<WeightViewUI>();
        _stageLamp = under.GetChild(7).GetComponent<StageLampUI>();

        Transform top = transform.GetChild(2);
        woodGauge = top.GetChild(1).GetComponent<WoodGaugeUI>();

        pose = transform.GetChild(3).GetComponent<PoseUI>();

        lifeView = transform.GetChild(4).GetComponent<LifeViewUI>();

        countDown = transform.GetChild(5).GetComponent<CountDownUI>();

        buttons[0] = transform.GetChild(6).GetChild(0).gameObject;
        buttons[1] = transform.GetChild(6).GetChild(1).gameObject;

        Transform massages = transform.GetChild(7);
        resultMassages = new GameObject[]{massages.GetChild(0).gameObject, massages.GetChild(1).gameObject};

        cardSelect = transform.GetChild(8).GetComponent<CardSelectUI>();

        ArrangeCameraRange();
    }

    void Update()
    {
        if(saveScreenSize.x != Screen.width || saveScreenSize.y != Screen.height)
        {
            ArrangeCameraRange();
        }
    }

    public void ReloadUI()
    {
        weapon.ReloadUI();
        foreach(CrystalGaugeUI crystalUi in crystals)
        {
            crystalUi.ReloadUI();
        }
        weight.ReloadUI();
        woodGauge.ReloadUI();
        lifeView.ReloadUI();
    }

    private void ArrangeCameraRange()
    {
        saveScreenSize = new Vector2(Screen.width, Screen.height);

        Mother.Slasher.Paddings = new float[]{
            Screen.width - ScreenCore.WorldToScreenPos(fieldMarkers[0].position).x, 
            Screen.height - ScreenCore.WorldToScreenPos(fieldMarkers[1].position).y, 
            ScreenCore.WorldToScreenPos(fieldMarkers[2].position).y
        };

        Vector2 CameraRange = ScreenCore.CameraRange();
        backGround.size = CameraRange * 2;
        Vector2 pos = backGround.transform.position;
        pos.y = ScreenCore.ScreenToWorldPosY(0);
        backGround.transform.position = pos;

        shadow.localScale = CameraRange;
    }

    public void StartCountDown()
    {
        pose.gameObject.SetActive(false);
        buttons[0].SetActive(false);
        buttons[1].SetActive(false);
        countDown.StartCountDown();
    }

    public void StartPlay()
    {
        shadow.gameObject.SetActive(false);
        pose.gameObject.SetActive(true);
    }

    public void Pose()
    {
        shadow.gameObject.SetActive(true);
        buttons[0].SetActive(true);
        buttons[1].SetActive(true);
    }

    public void Restart()
    {
        shadow.gameObject.SetActive(false);
        buttons[0].SetActive(false);
        buttons[1].SetActive(false);
    }

    public void ShowResult(bool isClear, bool canRespawn)
    {
        shadow.gameObject.SetActive(true);
        pose.gameObject.SetActive(false);

        buttons[0].SetActive(true);
        if(canRespawn)
        {
            buttons[1].SetActive(true);
        }

        if(isClear)
        {
            resultMassages[0].SetActive(true);
        }
        else
        {
            resultMassages[1].SetActive(true);
        }
    }

    public void BlindMenu()
    {
        buttons[0].SetActive(false);
        buttons[1].SetActive(false);
        resultMassages[0].SetActive(false);
        resultMassages[1].SetActive(false);
        pose.ExternalRestart();
    }

    public void BlindPose()
    {
        pose.gameObject.SetActive(false);
    }

    public void DrawCard()
    {
        shadow.gameObject.SetActive(true);
        pose.gameObject.SetActive(false);
        cardSelect.DrawCard();
    }

    public void NextStage()
    {
        _stageLamp.ChangeStage();
        ReloadUI();
    }

    public void HideCards()
    {
        cardSelect.HideCard();
    }
}

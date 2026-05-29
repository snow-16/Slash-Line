using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-5)]
public class SlasherCore : ChildSystem
{
    [SerializeField]
    private Vector2 startPos;
    [SerializeField]
    private Sprite safe;
    [SerializeField]
    private Sprite warning;
    [SerializeField]
    private Sprite danger;

    [SerializeField]
    private float initialRotVelocity;
    private float rotVelocity;
    [SerializeField]
    private float rotDamping;

    [SerializeField]
    private InputAction selectKey;

    public enum DangerLevel
    {
        SAFE,
        WARNING,
        DANGER
    }
    private DangerLevel level = DangerLevel.SAFE;

    private SlasherModule[] modules = new SlasherModule[4];
    private int bladeCount = 0;
    private float weight = 1;
    private GameObject selecter;
    private int selected = 0;
    private int maxLife;
    private int life;

    private CrystalContainer[] containers = new CrystalContainer[4];
    private CrystalAndBlades.CAndBType[] containtsTypes = new CrystalAndBlades.CAndBType[4];
    private int woodCount = 0;
    private int requestWoods = 10;

    private SlasherLineDrawer slasherLineDrawer;

    protected override void Start()
    {
        base.Start();

        transform.position = startPos;

        slasherLineDrawer = GetComponent<SlasherLineDrawer>();
        slasherLineDrawer.Core = this;

        for(int i = 0; i < 4; i++)
        {
            modules[i] = transform.GetChild(i).gameObject.GetComponent<SlasherModule>();
        }
        selecter = transform.GetChild(4).gameObject;
        selectKey.Enable();

        RemoveContainers();
        maxLife = Mother.maxLife;
        ResetLife();

        ResetRotVelocity();
    }

    void Update()
    {
        if(Mother.CanMoveScene())
        {
            CheckStopedTime();
            SelfRotation();

            if(selectKey.WasPressedThisFrame())
            {
                Select((int)selectKey.ReadValue<float>());
            }
        }
        else if(Mother.IsWaitingEnd)
        {
            if(rotVelocity > 0.01f)
            {
                SelfRotation();
            }
            else
            {
                rotVelocity = 0;
                if(woodCount != requestWoods)
                {
                    Death();
                    Mother.ResultGame(false, life > 0);
                }
                else if(Mother.Stage == 4)
                {
                    Mother.ResultGame(true, false);
                }
                else
                {
                    Mother.SelectCard();
                }
            }
        }
    }

    //時間経過でスプライトが変化し、最終的にゲームオーバー
    private void CheckStopedTime()
    {
        if(rotVelocity / initialRotVelocity <= 0.01)
        {
            rotVelocity = 0;
            Mother.EndGame();
        }
        else if(rotVelocity / initialRotVelocity <= 0.05)
        {
            if(level != DangerLevel.DANGER)
            {
                level = DangerLevel.DANGER;
                ChangeSafetyLevel();
            }
        }
        else if(rotVelocity / initialRotVelocity <= 0.2)
        {
            if(level != DangerLevel.WARNING)
            {
                level = DangerLevel.WARNING;
                ChangeSafetyLevel();
            }
        }
        else
        {
            if(level != DangerLevel.SAFE)
            {
                level = DangerLevel.SAFE;
                ChangeSafetyLevel();
            }
        }
    }

    private void SelfRotation()
    {
        transform.eulerAngles -= new Vector3(0,0,rotVelocity);
        rotVelocity /= rotDamping * (1f + (weight - 1) / 100);
    }

    public void Collect(CrystalAndBlades.CAndBType collectType)
    {
        for(int i = 0; i < 4; i++)
        {
            if(containers[i].Type == collectType)
            {
                if(containers[i].AddCount())
                {
                    CreateBlade(collectType);
                }
                break;
            }
        }

        Mother.ReloadUI();
    }

    private void CreateBlade(CrystalAndBlades.CAndBType collectType)
    {
        bladeCount++;
        if(bladeCount == 4)
        {
            ResetContainers();
        }

        foreach(SlasherModule module in modules)
        {
            if(!module.HaveBlade)
            {
                module.CreateBlade(collectType);
                break;
            }
        }
        CulculateWeight();

        Mother.ReloadUI();
    }

    public void UseBlade(int count)
    {
        for(int i = 0; i < count; i++)
        {
            if(!modules[selected].UseBlade())
            {
                bladeCount--;

                if(bladeCount == 0)
                {
                    Select(0);
                }
                else
                {
                    for(int j = 1; j <= 4; j++)
                    {
                        int lookAtIndex = (int)Mathf.Repeat(selected + j, 4);
                        if(modules[lookAtIndex].HaveBlade)
                        {
                            Select(lookAtIndex);
                            break;
                        }
                    }
                }
                CulculateWeight();
            }
        }

        Mother.ReloadUI();
    }

    public float UsingBlade(int count)
    {
        return GetBladeDamage((int)Mathf.Repeat(selected + count, 4));
    }

    public int GetCanUsingCount(int count)
    {
        return modules[(int)Mathf.Repeat(selected + count, 4)].UsableCount;
    }

    public List<float> BladeDamages()
    {
        List<float> total = new();
        for(int i = 0; i < 4; i++)
        {
            int index = (int)Mathf.Repeat(i + selected, 4);
            var module = modules[index];
            if(module.HaveBlade)
            {
                for(int j = 0; j < module.UsableCount; j++)
                {
                    total.Add(GetBladeDamage(index));
                }
            }
        }
        return total;
    }

    public float GetBladeDamage(int index)
    {
        if(GetBladeType(index) == CrystalAndBlades.CAndBType.OSMIUM)
        {
            return Mathf.Max(bladeCount - 1, 0) * 2;
        }
        else
        {
            return modules[index].Damage;
        }
    }

    public CrystalAndBlades.CAndBType GetBladeType(int index)
    {
        return modules[index].Type;
    }

    private void CulculateWeight()
    {
        weight = 1;
        foreach(SlasherModule module in modules)
        {
            if(module.HaveBlade)
            {
                weight += module.Weight;
            }
        }
    }

    private void RotSelecter()
    {
        selecter.transform.localEulerAngles = new Vector3(0,0,selected * -90);
    }

    public void ChangeSafetyLevel()
    {
        switch(level)
        {
            case DangerLevel.SAFE:
                foreach(SlasherModule module in modules)
                {
                    module.ChangeSafetyLevel(safe);
                }
                break;
            case DangerLevel.WARNING:
                foreach(SlasherModule module in modules)
                {
                    module.ChangeSafetyLevel(warning);
                }
                break;
            case DangerLevel.DANGER:
                foreach(SlasherModule module in modules)
                {
                    module.ChangeSafetyLevel(danger);
                }
                break;
        }

        slasherLineDrawer.ChangeSafety(level);
        Mother.ReloadUI();
    }

    public float GetBonusMultiplier()
    {
        switch(level)
        {
            case DangerLevel.SAFE:
                return 1f;
            case DangerLevel.WARNING:
                return 1.5f;
            case DangerLevel.DANGER:
                return 2f;
        }

        return 1;
    }

    public void ResetRotVelocity()
    {
        rotVelocity = initialRotVelocity;
    }

    public void Select(int index)
    {
        if(bladeCount > 0)
        {
            if(modules[index].HaveBlade)
            {
                selected = index;
                RotSelecter();
            }
        }
        else
        {
            selected = 0;
            RotSelecter();
        }

        Mother.ReloadUI();
    }

    public void AddWood(int value)
    {
        woodCount += value;
        if(woodCount >= requestWoods)
        {
            woodCount = requestWoods;
            EndRotation();
            Mother.EndGame();
        }

        Mother.ReloadUI();
    }

    public void EndRotation()
    {
        ResetRotVelocity();
        rotDamping = 1.5f;
        weight = 1;
    }

    public void Death()
    {
        life--;
    }

    public void Refresh()
    {
        ResetRotVelocity();
        level = DangerLevel.SAFE;
        ChangeSafetyLevel();

        transform.position = startPos;
        transform.eulerAngles = Vector2.zero;

        rotDamping = 1.03f;
        weight = 1;
        woodCount = 0;
        Select(0);

        ResetBlades();

        ResetContainers();
    }

    public void ResetBlades()
    {
        foreach(SlasherModule module in modules)
        {
            module.ResetBlade();
        }
        bladeCount = 0;
    }

    public void AddContainer(CrystalAndBlades.CAndBType type)
    {
        for(int i = 0; i < 4; i++)
        {
            if(containers[i].Type == CrystalAndBlades.CAndBType.EMPTY)
            {
                containers[i] = new CrystalContainer(type);
                break;
            }
        }
        CheckContaints();
    }

    public void ResetLife()
    {
        life = maxLife;
    }

    public void ResetContainers()
    {
        for(int i = 0; i < containers.Length; i++)
        {
            if(containers[i].Type != CrystalAndBlades.CAndBType.EMPTY)
            {
                containers[i].Reset();
            }
        }
    }

    public void RemoveContainers()
    {
        for(int i = 0; i < 4; i++)
        {
            containers[i].Reset();
            containers[i] = new CrystalContainer(CrystalAndBlades.CAndBType.EMPTY);
        }
        containers[0] = new CrystalContainer(CrystalAndBlades.CAndBType.PURE);
        CheckContaints();
    }

    private void CheckContaints()
    {
        for(int i = 0; i < 4; i++)
        {
            containtsTypes[i] = containers[i].Type;
        }
    }

    public float GetCollectRate()
    {
        return modules[selected].CollectRate;
    }

    public DangerLevel Level
    {
        get{return level;}
        set{level = value;}
    }

    public int BladeCount
    {
        get{return bladeCount;}
    }

    public float BladeDamage
    {
        get{return GetBladeDamage(selected);}
    }

    public CrystalAndBlades.CAndBType BladeType
    {
        get{return GetBladeType(selected);}
    }

    public float Weight
    {
        get{return weight;}
        set{weight = value;}
    }

    public int Selected
    {
        get{return selected;}
    }

    public int MaxLife
    {
        get{return maxLife;}
    }

    public int Life
    {
        get{return life;}
    }

    public int WoodCount
    {
        get{return woodCount;}
    }

    public int RequestWoods
    {
        get{return requestWoods;}
        set{requestWoods = value;}
    }

    public float[] Paddings
    {
        get{return slasherLineDrawer.Paddings;}
        set{slasherLineDrawer.Paddings = value;}
    }

    public SlasherModule[] Modules
    {
        get{return modules;}
    }

    public CrystalContainer[] Containers
    {
        get{return containers;}
    }

    public CrystalAndBlades.CAndBType[] ContaintsTypes
    {
        get{return containtsTypes;}
    }

    public struct CrystalContainer
    {
        private CrystalAndBlades.CAndBType type;
        private int count;

        public CrystalContainer(CrystalAndBlades.CAndBType selectedType)
        {
            type = selectedType;
            count = 0;
        }

        public bool AddCount()
        {
            count++;
            if(count == 4)
            {
                Reset();
                return true;
            }
            return false;
        }

        public void Reset()
        {
            count = 0;
        }

        public CrystalAndBlades.CAndBType Type
        {
            get{return type;}
        }

        public int Count
        {
            get{return count;}
        }
    }
}

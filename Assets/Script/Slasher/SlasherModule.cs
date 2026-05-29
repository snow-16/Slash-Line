using UnityEngine;

public class SlasherModule : ChildSystem
{
    private bool haveBlade = false;
    private CrystalAndBlades.CAndBType type;
    private float weight = 1;
    private int usableCount;
    private float damage;
    private float collectRate;

    private GameObject awake;
    private GameObject blade;
    private SpriteRenderer blade_sprite;

    private SpriteRenderer module_sprite;
    
    protected override void Start()
    {
        base.Start();
        awake = transform.GetChild(0).gameObject;
        blade = transform.GetChild(1).gameObject;
        blade_sprite = blade.GetComponent<SpriteRenderer>();

        module_sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    public void ChangeSafetyLevel(Sprite sprite)
    {
        module_sprite.sprite = sprite;
    }

    public void CreateBlade(CrystalAndBlades.CAndBType createType)
    {
        haveBlade = true;
        type = createType;
        CrystalAndBlades.CAndBDatas cAndBData = Mother.FindCAndBData(type);
        blade_sprite.color = cAndBData.crystal_color;
        weight = cAndBData.weight;
        usableCount = cAndBData.usableCount;
        damage = cAndBData.damage;
        collectRate = cAndBData.collectRate;
        awake.SetActive(true);
        blade.SetActive(true);
    }

    public bool UseBlade()
    {
        usableCount--;
        if(usableCount == 0)
        {
            ResetBlade();
        }
        return haveBlade;
    }

    public void ResetBlade()
    {
        haveBlade = false;
        blade.SetActive(false);
        awake.SetActive(false);
    }

    public bool HaveBlade
    {
        get{return haveBlade;}
    }

    public CrystalAndBlades.CAndBType Type
    {
        get{return type;}
    }

    public float Weight
    {
        get{return weight;}
        set{weight = value;}
    }

    public int UsableCount
    {
        get{return usableCount;}
    }

    public float Damage
    {
        get{return damage;}
    }

    public float CollectRate
    {
        get{return collectRate;}
    }

    public Color BladeColor
    {
        get{return blade_sprite.color;}
    }

    public Sprite ModuleSprite
    {
        get{return module_sprite.sprite;}
    }
}

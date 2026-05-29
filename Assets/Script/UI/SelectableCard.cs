using UnityEngine;

public class SelectableCard : ButtonBase
{
    private CrystalAndBlades.CAndBType type;

    protected override void OnClick()
    {
        Mother.CollectCard(type);
    }

    public CrystalAndBlades.CAndBType Type
    {
        set{type = value;}
    }
}

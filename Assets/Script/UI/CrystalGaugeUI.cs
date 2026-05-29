using UnityEngine;
using UnityEngine.UI;

public class CrystalGaugeUI : ChildSystem
{
    [SerializeField]
    private int index;

    SlasherCore.CrystalContainer container;

    private Image[] parts = new Image[4];
    TMPro.TextMeshProUGUI damageText;

    protected override void Start()
    {
        base.Start();
        
        for(int i = 0; i < 4; i++)
        {
            parts[i] = transform.GetChild(0).GetChild(i).GetComponent<Image>();
        }
        damageText = transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void ReloadUI()
    {
        container = Mother.Slasher.Containers[index];
        bool isEmpty = container.Type == CrystalAndBlades.CAndBType.EMPTY;
        CrystalAndBlades.CAndBDatas data;
        
        if(isEmpty)
        {
            damageText.gameObject.SetActive(false);
            ReloadParts(false);
        }
        else
        {
            data = Mother.FindCAndBData(container.Type);
            damageText.gameObject.SetActive(true);
            if(container.Type == CrystalAndBlades.CAndBType.OSMIUM)
            {
                damageText.text = (Mathf.Max(Mother.Slasher.BladeCount - 1, 0) * 2).ToString();
            }
            else
            {
                damageText.text = data.damage.ToString();
            }
            ReloadParts(true, data.crystal_color);
        }
    }

    private void ReloadParts(bool show)
    {
        ReloadParts(show, Color.gray);
    }

    private void ReloadParts(bool show, Color color)
    {
        for(int i = 0; i < 4; i++)
        {
            Image part = parts[i];
            part.gameObject.SetActive(show);
            if(show)
            {
                part.color = (container.Count > i) ? color : Color.gray;
            }
        }
    }
}

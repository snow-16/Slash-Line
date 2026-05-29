using UnityEngine.UI;

public class WoodGaugeUI : ChildSystem
{
    private TMPro.TextMeshProUGUI countText;
    private TMPro.TextMeshProUGUI requestText;
    private Image woodLine;

    protected override void Start()
    {
        base.Start();

        countText = transform.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>();
        requestText = transform.GetChild(5).GetComponent<TMPro.TextMeshProUGUI>();
        woodLine = transform.GetChild(1).GetComponent<Image>();
    }

    public void ReloadUI()
    {
        float count = Mother.Slasher.WoodCount;
        float request = Mother.Slasher.RequestWoods;

        countText.text = $"{count}";
        requestText.text = $"{request}";
        woodLine.fillAmount = count / request;
    }
}

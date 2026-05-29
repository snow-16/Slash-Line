public class WeightViewUI : ChildSystem
{
    private TMPro.TextMeshProUGUI weightText;

    protected override void Start()
    {
        base.Start();

        weightText = transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void ReloadUI()
    {
        weightText.text = $"x{Mother.Slasher.Weight}";
    }
}

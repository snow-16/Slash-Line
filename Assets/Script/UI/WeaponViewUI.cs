using UnityEngine;
using UnityEngine.UI;

public class WeaponViewUI : ChildSystem
{
    private Image[] modules = new Image[4];
    private Image[] blades = new Image[4];
    private GameObject[] awakes = new GameObject[4];
    private GameObject selecter;

    protected override void Start()
    {
        base.Start();
        
        for(int i = 0; i < 4; i++)
        {
            modules[i] = transform.GetChild(i).GetComponent<Image>();
            awakes[i] = transform.GetChild(i).GetChild(0).gameObject;
            blades[i] = transform.GetChild(i).GetChild(1).GetComponent<Image>();
        }
        selecter = transform.GetChild(4).gameObject;
    }

    public void ReloadUI()
    {
        for(int i = 0; i < 4; i++)
        {
            SlasherModule module = Mother.Slasher.Modules[i];
            modules[i].sprite = module.ModuleSprite;
            if(module.HaveBlade)
            {
                blades[i].color = module.BladeColor;
                blades[i].gameObject.SetActive(true);
                awakes[i].gameObject.SetActive(true);
            }
            else
            {
                blades[i].gameObject.SetActive(false);
                awakes[i].gameObject.SetActive(false);
            }
        }

        selecter.transform.localEulerAngles = new Vector3(0, 0, Mother.Slasher.Selected * -90);
    }
}

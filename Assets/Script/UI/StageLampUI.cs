using UnityEngine;
using UnityEngine.UI;

public class StageLampUI : ChildSystem
{
    [SerializeField]
    Sprite _lampIn;

    Image[] lampImages = new Image[4];

    protected override void Start()
    {
        base.Start();
        
        for(int i = 0; i < 4; i++)
        {
            lampImages[i] = transform.GetChild(i).GetComponent<Image>();
        }
    }

    public void ChangeStage()
    {
        for(int i = 0; i < 4; i++)
        {
            if(Mother.Stage > i)
            {
                lampImages[i].sprite = _lampIn;
            }
        }
    }
}

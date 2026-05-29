using UnityEngine;
using UnityEngine.UI;

public class LifeViewUI : ChildSystem
{
    [SerializeField]
    GameObject lifeContainer;
    [SerializeField]
    Sprite full;
    [SerializeField]
    Sprite empty;

    Image[] containerImages;

    protected override void Start()
    {
        base.Start();
        
        containerImages = new Image[Mother.Slasher.MaxLife];
        for(int i = 0; i < containerImages.Length; i++)
        {
            var newContainer = Instantiate(lifeContainer).transform;
            newContainer.SetParent(transform);
            newContainer.localPosition = new Vector2(100 * i, 0);
            newContainer.localScale = Vector3.one;
            containerImages[i] = newContainer.GetComponent<Image>();
        }
    }

    public void ReloadUI()
    {
        for(int i = 0; i < containerImages.Length; i++)
        {
            if(Mother.Slasher.Life < i + 1)
            {
                containerImages[i].sprite = empty;
            }
            else
            {
                containerImages[i].sprite = full;
            }
        }
    }
}

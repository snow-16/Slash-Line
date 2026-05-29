using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PoseUI : ChildSystem, IPointerClickHandler
{
    [SerializeField]
    private Sprite pose;
    [SerializeField]
    private Sprite reStart;

    private Image icon;

    protected override void Start()
    {
        base.Start();
        icon = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(Mother.IsPosing)
        {
            icon.sprite = pose;
            Mother.ReStartGame();
        }
        else
        {
            icon.sprite = reStart;
            Mother.PoseGame();
        }
    }

    public void ExternalRestart()
    {
        icon.sprite = pose;
    }
}

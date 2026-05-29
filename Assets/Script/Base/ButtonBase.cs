using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBase : ChildSystem, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected Image buttonImage;

    protected override void Start()
    {
        base.Start();

        buttonImage = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        buttonImage.color = Color.white;
        OnClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.color = Color.gray;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.color = Color.white;
    }

    protected virtual void OnClick()
    {
        
    }
}

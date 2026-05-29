using UnityEngine;
using UnityEngine.EventSystems;

[DefaultExecutionOrder(1)]
public class SelecterUI : ChildSystem, IPointerClickHandler
{
    [SerializeField]
    private int index;

    public void OnPointerClick(PointerEventData eventData)
    {
        Mother.Slasher.Select(index);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class DragableObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    protected bool have = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        have = true;
        OnDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        have = false;
        OnUp();
    }

    protected virtual void OnDown()
    {
        
    }

    protected virtual void OnUp()
    {
        
    }

    protected virtual void OnDrag()
    {
        
    }
}

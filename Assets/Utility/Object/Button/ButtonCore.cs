using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class ButtonCore : MonoBehaviour, 
IPointerClickHandler,
IPointerDownHandler,
IPointerUpHandler,
IPointerEnterHandler,
IPointerExitHandler
{
    [SerializeReference]
    protected List<GameObject> receivers = new List<GameObject>();

    [SerializeField]
    protected bool[] cans = new bool[5];

    public void OnPointerClick(PointerEventData eventData)
    {
        if (cans[0])
        {
            receivers.ForEach(receiver => receiver.GetComponent<IButtonReceiver>().OnClick(this));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (cans[1])
        {
            receivers.ForEach(item => 
            {
                IButtonReceiver receiver = item.GetComponent<IButtonReceiver>();
                receiver.OnDown(this);
                receiver.OnDownAndUp(this);
            });
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (cans[2])
        {
            receivers.ForEach(item => 
            {
                IButtonReceiver receiver = item.GetComponent<IButtonReceiver>();
                receiver.OnUp(this);
                receiver.OnDownAndUp(this);
            });
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cans[3])
        {
            receivers.ForEach(item => 
            {
                IButtonReceiver receiver = item.GetComponent<IButtonReceiver>();
                receiver.OnEnter(this);
                receiver.OnEnterAndExit(this);
            });
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cans[4])
        {
            receivers.ForEach(item => 
            {
                IButtonReceiver receiver = item.GetComponent<IButtonReceiver>();
                receiver.OnExit(this);
                receiver.OnEnterAndExit(this);
            });
        }
    }

    [Serializable]
    public abstract class DataStorage
    {
        
    }
}

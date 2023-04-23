using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public delegate void Delegate();
public class EventManager : MonoBehaviour
{
    public static event Delegate Event;
    public static int eventSign;
    public List<BaseEvent> baseEvents = new List<BaseEvent>();

    private void Awake()
    {
        eventSign = 1;
    }
    public void InvokeEvent()
    {
        if (Event == null)
            Debug.Log("Empty event");
        else
        {
            Event.Invoke();
            eventSign *= -1;
        }
    }
    public void ClearEvent()
    {
        Debug.Log("Clear");
        Event = null;
    }

    public static void Sub(BaseEvent baseEvent, bool subMessage)
    {
        if (baseEvent.sub)
        {
            if(subMessage)
                Debug.Log("Subed");
            Event += baseEvent.del;
        }
    }
    public void MultipleSub()
    {
        Debug.Log("MulltipleSub");
        foreach (BaseEvent baseEvent in baseEvents)
        {
            Sub(baseEvent, false);
        }
    }
    public void EventInfo()
    {
        string text;
        if (Event == null)
            text = "Empty";
        else
            text = "Last subscription: " + Event.Method;
        Debug.Log(text);
    }
}

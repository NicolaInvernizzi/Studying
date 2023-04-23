using UnityEngine;

public class Ball : BaseEvent
{
    private void Awake()
    {
        del = ChangeScale;
    }
    private void ChangeScale()
    {
        transform.localScale += Vector3.one * EventManager.eventSign;
    }
    public void UnityEvent_Test1(GameObject a)
    {
        Debug.Log("UnityEvent_Test1: " + a);
    }
    public void UnityEvent_Test2(int i, bool b)
    {
        Debug.Log("UnityEvent_Test1: " + i + b);
    }
}

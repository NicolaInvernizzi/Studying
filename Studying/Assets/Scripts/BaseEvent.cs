using UnityEngine;

public abstract class BaseEvent : MonoBehaviour
{
    public bool sub;
    [HideInInspector]public Delegate del;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SingleSub();
        if (Input.GetKeyDown(KeyCode.U))
            SingleUnSub();
        if (Input.GetKeyDown(KeyCode.A))
            Debug.Log(del.Target);
        if (Input.GetKeyDown(KeyCode.B))
            Debug.Log(del.Method);
    }
    public void SingleSub()
    {
        EventManager.Sub(this, true);
    }
    public void SingleUnSub()
    {
        Debug.Log("UnSubed");
        EventManager.Event -= del;
    }
}

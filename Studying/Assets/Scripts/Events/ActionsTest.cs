using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionsTest : MonoBehaviour
{
    // Action: return void, i can specify imput paramters.
    // Deleates ready to use.
    // I can create events based on Action delegate.
    // Can't see them in inspector.

    public BaseEvent baseEvent;
    public Action action;
    public Action<int> action_int;
    public Action<int, GameObject, List<int>> action_multy;
    public event Action eve;
    public delegate void ActionTest<T>(T t);
    void Start()
    {
        action = baseEvent.SingleSub;
        action_int = TestAction;
        eve += action;
        // !!!!!! eve += action_int;
        eve.Invoke();
        ActionTest<int> test = TestAction;
    }
    private void TestAction(int id)
    {
        // body
    }
}

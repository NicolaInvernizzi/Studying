using UnityEngine;
using UnityEngine.Events;

public class UnityEventTest : MonoBehaviour
{
    public event Delegate Event;
    public int test_int;
    public bool test_bool;
    public UnityEvent unityEvent;
    public UnityEvent unityEvent22;
    public DynamicTest dynamicTest;
    public DynamicTest2 dynamicTest2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            unityEvent.Invoke();

        if (Input.GetKeyDown(KeyCode.K))
            Dynamic_Invoke(test_int, test_bool);

        if (Input.GetKeyDown(KeyCode.J))
            Dynamic2_Invoke(test_bool);

        if (Input.GetKeyDown(KeyCode.Y))
            TestTest();
    }
    private void Dynamic_Invoke(int a, bool b)
    {
        dynamicTest.Invoke(a, b);
    }
    private void Dynamic2_Invoke(bool a)
    {
        dynamicTest2.Invoke(a);
    }
    private void Method_Test()
    {
        print("Method_Test");
    }
    private void TestTest()
    {
        if (unityEvent22.GetPersistentEventCount() == 0)
            print("unityEvent is null");
        unityEvent22.AddListener(Method_Test);
        unityEvent22.Invoke();
        unityEvent22.RemoveListener(Method_Test);
        if (unityEvent22 == null)
            print("unityEvent is null");
        unityEvent22.RemoveAllListeners();

        unityEvent22 = null;
        unityEvent22.AddListener(Method_Test);
        Event = null;
        Event += Method_Test;
        Event.Invoke();
    }
}

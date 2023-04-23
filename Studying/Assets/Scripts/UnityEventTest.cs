using UnityEngine;
using UnityEngine.Events;

public class UnityEventTest : MonoBehaviour
{
    public int test_int;
    public bool test_bool;
    public UnityEvent unityEvent;
    public DynamicTest dynamicTest;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            unityEvent.Invoke();

        if (Input.GetKeyDown(KeyCode.K))
            Dynamic_Invoke(test_int, test_bool);
    }
    private void Dynamic_Invoke(int a, bool b)
    {
        dynamicTest.Invoke(a, b);
    }
    private void Method_Test()
    {
        print("Method_Test");
    }
}

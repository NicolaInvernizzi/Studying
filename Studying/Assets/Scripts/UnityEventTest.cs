using UnityEngine;
using UnityEngine.Events;

public class UnityEventTest : MonoBehaviour
{
    public int test_int;
    public bool test_bool;
    public UnityEvent unityEvent;
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
}

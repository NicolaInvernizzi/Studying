using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventTest : MonoBehaviour
{
    UnityEvent unityEvent11;
    public UnityEvent unityEvent22;

    GameObject gameObjec11;
    public GameObject gameObjec22;
    public List<GameObject> list;

    public event Delegate Event;
    public int test_int;
    public bool test_bool;
    public DynamicTest dynamicTest;
    public DynamicTest2 dynamicTest2;

    private void Update()
    {
        if (unityEvent11 == null)
            print("unityEvent11 is null");
        if (unityEvent22== null)
            print("unityEvent22 is null");
        if (gameObjec22 == null)
            print("gameObjec22 is null");
        if (gameObjec11 == null)
            print("gameObjec11 is null");
        if (list == null)
            print("list is null");


        if (Input.GetKeyDown(KeyCode.Space))
            list = null;

        if (Input.GetKeyDown(KeyCode.K))
            Dynamic_Invoke(test_int, test_bool);

        if (Input.GetKeyDown(KeyCode.J))
            Dynamic2_Invoke(test_bool);

        if (Input.GetKeyDown(KeyCode.L))
            unityEvent11.Invoke();

        if (Input.GetKeyDown(KeyCode.Q))
            unityEvent22.Invoke(); //  invoke a null UnityEvent does nothing...
        if (Input.GetKeyDown(KeyCode.W))
        {
            /*unityEvent22 = null;*/ // if uncomment create error because can't call method froma null reference
            unityEvent22.AddListener(Method_Test); //  adding a listener to a null UnityEvent does nothing...
        }

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
            print("unityEvent is null"); // this will not print because

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

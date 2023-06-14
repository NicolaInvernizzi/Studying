using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestingNull : MonoBehaviour
{
    UnityEvent unityEvent11;
    public UnityEvent unityEvent22;

    GameObject gameObjec11;
    public GameObject gameObjec22;

    List<GameObject> list11;
    public List<GameObject> list22;

    int testCounter = 0;

    void Start()
    {
        Infos();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            unityEvent11 = null;
            unityEvent22 = null; 
            gameObjec11 = null; 
            gameObjec22 = null; 
            list11 = null; 
            list22 = null;
        }
        if(Input.GetKeyDown(KeyCode.O))
            Infos();
    }

    void Infos()
    {
        print($"----- Test {testCounter} -----");
        Debug.Log("unityEvent11:");
        Debug.Log(unityEvent11);
        Debug.Log("unityEvent22:");
        Debug.Log(unityEvent22);
        Debug.Log("gameObjec11:");
        Debug.Log(gameObjec11);
        Debug.Log("gameObjec22:");
        Debug.Log(gameObjec22);
        Debug.Log($"list11: {list11}");
        Debug.Log(list11);
        Debug.Log($"list22: {list22}");
        Debug.Log(list22);
        testCounter++;
    }
}

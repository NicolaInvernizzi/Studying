using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestingNull : MonoBehaviour
{
    [SerializeField] UnityEvent unityEvent11;
    public UnityEvent unityEvent22;

    GameObject gameObjec11;
    public GameObject gameObjec22;

    List<GameObject> list11;
    public List<GameObject> list22;

    int testCounter = 0;

    void Start() => Infos();

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
        print($"----- Test {testCounter} -----\n" +
            $"\nunityEvent11: - {unityEvent11} -\nunityEvent22: - {unityEvent22} -" +
            $"\ngameObjec11: - {gameObjec11} -\ngameObjec22: - {gameObjec22} -" +
            $"\nlist11: - {list11} -\nlist22: - {list22} -");
        testCounter++;
    }
}

using UnityEngine;

public class AwakeStartTest : MonoBehaviour
{
    private void Awake()
    {
        print(name + " Awake");
    }
    void Start()
    {
        print(name + " Start");
    }
}

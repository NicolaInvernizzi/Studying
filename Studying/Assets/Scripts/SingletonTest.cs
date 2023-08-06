using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonTest : MonoBehaviour
{
    public static SingletonTest instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        SceneManager.LoadScene("AI");
    //        Debug.Break();
    //    }
    //    if (Input.GetKeyDown(KeyCode.B))
    //    {
    //        SceneManager.LoadScene("FiniteStateMachine");
    //        Debug.Break();
    //    }
    //}
}

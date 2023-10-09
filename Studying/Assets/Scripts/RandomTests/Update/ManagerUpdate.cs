using UnityEngine;

public class ManagerUpdate : MonoBehaviour
{
    public TestUpdate testUpdate;
    public TestUpdate2 testUpdate2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print(testUpdate.a);
            print(testUpdate2.b);
        }
    }

}

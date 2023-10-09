using UnityEngine;

public class TestUpdate : MonoBehaviour
{
    public bool a;
    private void Update()
    {
        a = Input.GetKey(KeyCode.A);
    }
}

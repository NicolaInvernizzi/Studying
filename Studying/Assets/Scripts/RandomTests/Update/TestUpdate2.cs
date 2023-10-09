using UnityEngine;

public class TestUpdate2 : MonoBehaviour
{
    public bool b;
    private void Update()
    {
        b = Input.GetKey(KeyCode.B);
    }
}

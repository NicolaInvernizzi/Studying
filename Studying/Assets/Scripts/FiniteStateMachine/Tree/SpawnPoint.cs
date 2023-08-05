using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public void CreateFruit(Transform t)
    {
        Instantiate(t, transform);
        t.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}

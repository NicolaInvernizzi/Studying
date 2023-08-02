using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform carTransform;
    [SerializeField]
    float offset = 40f;

    private void LateUpdate()
    {
        transform.position = new Vector3(carTransform.position.x, carTransform.position.y + offset, carTransform.position.z);
    }
}

using UnityEngine;

public class BoidCreator : MonoBehaviour
{
    [SerializeField]
    GameObject boid;

    [SerializeField]
    bool randomStartRotation;

    [SerializeField, Range(0.0f, 360.0f)]
    float fixedStartRotation = 0.0f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                float rotY = 0.0f;
                if (randomStartRotation)
                {
                    rotY = Random.Range(0.0f, 360.0f);
                }
                else
                {
                    rotY = fixedStartRotation;
                }

                Vector3 spawnPosition = hitInfo.point;
                spawnPosition.y = 0;
                Instantiate(boid, spawnPosition, Quaternion.Euler(0.0f, rotY, 0.0f));
            }
        }
    }
}

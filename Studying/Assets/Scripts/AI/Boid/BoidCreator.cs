using UnityEngine;

public class BoidCreator : MonoBehaviour
{
    [SerializeField]
    GameObject boid;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Vector3 spawnPosition = hitInfo.point;
                spawnPosition.y = 0;
                Instantiate(boid, spawnPosition, Quaternion.identity);
            }
        }
    }
}

using System.Linq;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [SerializeField, Range(0.0f, 10.0f)]
    private float detectionRange = 2.0f;

    [SerializeField, Range(0.0f, 5.0f)]
    private float detectionDistance = 1.0f;

    [SerializeField, Range(0.0f, 10.0f)]
    private float minRotationSpeed = 1.0f;

    [SerializeField, Range(0.0f, 10.0f)]
    private float maxRotationSpeed = 5.0f;

    [SerializeField, Range(0.0f, 10.0f)]
    private float minMovementSpeed = 0.0f;

    [SerializeField, Range(0.0f, 10.0f)]
    private float maxMovementSpeed = 5.0f;

    [SerializeField]
    LayerMask detectablesMask;

    Collider[] colliders;
    float currentMovementSpeed;
    Color sphereColor;
    Collider thisCollider;
    Vector3 aiDirection = Vector3.zero;
    Vector3 closestPoint = Vector3.zero;

    private void Awake()
    {
        thisCollider = GetComponent<Collider>();
    }
    private void Start()
    {
        currentMovementSpeed = maxMovementSpeed;
    }
    private void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, detectionRange, detectablesMask);

        colliders = colliders.Where(col => col != thisCollider).ToArray();

        foreach (Collider col in colliders)
        {
            colliders = colliders.Where(col => Vector3.Angle(
                transform.forward, col.ClosestPoint(transform.position) - transform.position) <= 360.0f).ToArray();
        }

        if (colliders.Length > 0)
        {
            sphereColor = Color.green;
            float minDistance = Mathf.Infinity;
            Vector3 minPosition = Vector3.zero;
            Collider closestCol = colliders[0];

            foreach (Collider col in colliders)
            {
                float distance = Vector3.Distance(transform.position, col.ClosestPoint(transform.position));
                if (distance <= minDistance)
                {
                    minDistance = distance;
                    minPosition = col.transform.position;
                    closestCol = col;
                }
            }

            closestPoint = closestCol.ClosestPoint(transform.position);
            aiDirection = (transform.position - closestPoint).normalized;

            // modify movement speed
            currentMovementSpeed = Remap(0.0f, detectionRange, minMovementSpeed, maxMovementSpeed, minDistance);

            // modify rotation speed
            float currentRotationSpeed = Remap(0.0f, detectionRange, maxRotationSpeed, minRotationSpeed, minDistance);
            Quaternion finalRotation = Quaternion.LookRotation(aiDirection, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * currentRotationSpeed);

        }
        else
        {
            aiDirection = Vector3.zero;
            sphereColor = Color.red;
        }

        transform.position += transform.forward * currentMovementSpeed * Time.deltaTime;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = sphereColor;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, aiDirection * 10);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, closestPoint);
    }
    private void Cohesion()
    {

    }
    private void Alignment()
    {

    }
    private void Separation()
    {
        if (Physics.SphereCast(transform.position, detectionRange, transform.forward, out RaycastHit hitInfo, detectionRange, detectablesMask))
        {
            sphereColor = Color.green;
            aiDirection = transform.position - hitInfo.point;

            // modify rotation speed
            float currentRotationSpeed = Remap(0.0f, detectionRange, maxRotationSpeed, minRotationSpeed, aiDirection.magnitude);
            Quaternion finalRotation = Quaternion.LookRotation(aiDirection.normalized, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * currentRotationSpeed);
        }
        else
        {
            sphereColor = Color.red;
        }
    }

    private float Remap(float min_A, float max_A, float min_B, float max_B, float A)
    {
        float t = Mathf.InverseLerp(min_A, max_A, A);
        return Mathf.Lerp(min_B, max_B, t);
    }
    private Vector3 RemapSlerp(float min_A, float max_A, Vector3 min_B, Vector3 max_B, float A)
    {
        float t = Mathf.InverseLerp(min_A, max_A, A);
        return Vector3.Slerp(min_B, max_B, t);
    }
    private Quaternion RemapSlerp(float min_A, float max_A, Quaternion min_B, Quaternion max_B, float A)
    {
        float t = Mathf.InverseLerp(min_A, max_A, A);
        return Quaternion.Slerp(min_B, max_B, t);
    }
    private Vector3 RemapLerp(float min_A, float max_A, Vector3 min_B, Vector3 max_B, float A)
    {
        float t = Mathf.InverseLerp(min_A, max_A, A);
        return Vector3.Lerp(min_B, max_B, t);
    }
}

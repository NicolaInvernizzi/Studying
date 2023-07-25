using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class NewBoid : MonoBehaviour
{
    [SerializeField, Range(0.0f, 10.0f)]
    private float movementSpeed = 1.0f;
    [SerializeField, Range(0.0f, 100.0f)]
    private float globalMax_Separation = 50.0f;
    [SerializeField, Range(0.0f, 100.0f)]
    private float max_Separation = 5.0f;
    [SerializeField, Range(0.0f, 100.0f)]
    private float min_Separation = 0.0f;
    [SerializeField, Range(0.0f, 100.0f)]
    private float globalMax_Coesion = 50.0f;
    [SerializeField, Range(0.0f, 100.0f)]
    private float max_Coesion = 5.0f;
    [SerializeField, Range(0.0f, 100.0f)]
    private float min_Coesion = 0.0f;
    [SerializeField, Range(0.0f, 100.0f)]
    private float globalMax_Align = 50.0f;
    [SerializeField, Range(0.0f, 10.0f)]
    private float detectionRange = 2.0f;
    [SerializeField]
    private LayerMask detectablesMask;
    [SerializeField]
    private LayerMask obstacleMask;
    [SerializeField]
    private Color coesionColor;

    private Vector3 movement = new Vector3(1.0f, 0.0f, 0.0f);
    private Collider[] colliders;
    private Collider[] obstacles;
    private Collider thisCollider;
    private Color sphereColor;
    private void Awake()
    {
        thisCollider = GetComponent<Collider>();
    }
    private void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, detectionRange, detectablesMask);

        colliders = colliders.Where(col => col != thisCollider).ToArray();

        if (colliders.Length > 0)
        {
            movement += Alighn(colliders);
            movement += Separation(colliders);
            movement += Coesion(colliders);
            sphereColor = Color.green;
        }
        else
        {
            movement += Vector3.zero;
            sphereColor = Color.red;
        }

        movement += Obstacle();

        Move(movement);
    }

    private void Move(Vector3 velocity)
    {
        transform.position += velocity.normalized * Time.deltaTime;
        Quaternion rotation = Quaternion.LookRotation(velocity);
        transform.rotation = rotation;
    }

    private Vector3 Obstacle()
    {
        Vector3 repulsion = Vector3.zero;
        obstacles = Physics.OverlapSphere(transform.position, detectionRange, obstacleMask);

        if (obstacles.Length > 0)
        {
            foreach (Collider col in obstacles)
            {
                repulsion += (transform.position - col.ClosestPoint(transform.position)) *
                    Remap(0.0f, detectionRange, 1.0f, 0.0f, Vector3.Distance(transform.position, col.ClosestPoint(transform.position)));
            }
        }
        return repulsion;
    }

    private Vector3 Separation(Collider[] detected)
    {
        Vector3 separation = Vector3.zero;
        foreach (Collider col in detected)
        {
            separation += (transform.position - col.transform.position).normalized *
                Remap(0.0f, detectionRange, max_Separation, min_Separation, Vector3.Distance(transform.position, col.transform.position));
        }
        separation.y = 0.0f;
        return separation.normalized * Mathf.Min(separation.magnitude, this.globalMax_Separation);
    }

    private Vector3 Coesion(Collider[] detected)
    {
        Vector3 coesion = Vector3.zero;
        foreach (Collider col in detected)
        {
            Vector3 newDir = (col.transform.position - transform.position).normalized *
                    Remap(0.0f, detectionRange, min_Coesion, max_Coesion, Vector3.Distance(transform.position, col.transform.position));
            coesion += newDir;
            Debug.DrawRay(transform.position, newDir, coesionColor);
        }
        coesion.y = 0.0f;
        return coesion.normalized * Mathf.Min(coesion.magnitude, globalMax_Separation);
    }

    private Vector3 Alighn(Collider[] detected)
    {
        Vector3 align = Vector3.zero;
        foreach (Collider col in detected)
        {
            align += col.transform.forward *
                Remap(0.0f, detectionRange, 1.0f, 0.0f, Vector3.Distance(transform.position, col.transform.position));
        }
        align.y = 0.0f;
        return (align - movement).normalized * Mathf.Min((align - movement).magnitude, globalMax_Align);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = sphereColor;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 10);

        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, movement * 10);
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

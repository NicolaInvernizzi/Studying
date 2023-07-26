using System.Linq;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] float maxSpeed = 2f;
    [SerializeField] float speed = 2f;
    [SerializeField] float coesionRange = 2f;
    [SerializeField] float separationRange = 2f;
    [SerializeField] float obstacleRange = 2f;
    [SerializeField] LayerMask obstacleMask;
    public float repulsion = 30f;
    Vector3 direction;
    Vector3 velocity;

    void Start()
    {
        direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }
    void Update()
    {
        direction += Coesion();
        direction += Separation();
        direction += Align();
        direction += AvoidObstacles();

        velocity = direction.normalized * Mathf.Min(direction.magnitude, maxSpeed);
        transform.LookAt(transform.position + velocity);
        transform.position += velocity * Time.deltaTime;
    }

    Vector3 AvoidObstacles()
    {
        Vector3 totRepulsion = Vector3.zero;

        Collider[] obstacles = Physics.OverlapSphere(transform.position, obstacleRange, obstacleMask);
        if (obstacles.Length == 0f)
            return totRepulsion;

        foreach(Collider obstacle in obstacles)
        {
            totRepulsion += transform.position - obstacle.ClosestPoint(transform.position);
        }

        return totRepulsion.normalized * repulsion;
    }
    Vector3 Coesion()
    {
        Vector3 totCoesion = Vector3.zero;

        GameObject[] neighbors = GameObject.FindGameObjectsWithTag("Boid").Where(
            n => n != gameObject && Vector3.Distance(n.transform.position, transform.position) <= coesionRange).ToArray();

        if (neighbors.Length <= 0f)
            return totCoesion;

        foreach(GameObject neighbor in neighbors)
        {
            totCoesion += neighbor.transform.position;
        }
        totCoesion /= neighbors.Length;

        return totCoesion - transform.position;
    }
    Vector3 Separation()
    {
        Vector3 totSeparation = Vector3.zero;

        GameObject[] neighbors = GameObject.FindGameObjectsWithTag("Boid").Where(
            n => n != gameObject && Vector3.Distance(n.transform.position, transform.position) <= separationRange).ToArray();

        if (neighbors.Length == 0f)
            return totSeparation;

        //foreach (GameObject neighbor in neighbors)
        //{
        //    totSeparation += (transform.position - neighbor.transform.position);
        //}

        //direction += totSeparation;
        foreach (GameObject neighbor in neighbors)
        {
            totSeparation += neighbor.transform.position;
        }
        totSeparation /= neighbors.Length;

        return transform.position - totSeparation;
    }
    Vector3 Align()
    {
        Vector3 totAlign = Vector3.zero;

        GameObject[] neighbors = GameObject.FindGameObjectsWithTag("Boid").Where(
            n => n != gameObject && Vector3.Distance(n.transform.position, transform.position) <= separationRange).ToArray();

        if (neighbors.Length == 0f)
            return totAlign;

        foreach (GameObject neighbor in neighbors)
        {
            totAlign += neighbor.transform.forward;
        }
        totAlign /= neighbors.Length;

         return totAlign;
    }
    float Remap(float min_A, float max_A, float A, float min_B, float max_B)
    {
        float t = Mathf.InverseLerp(min_A, max_A, A);
        return Mathf.Lerp(min_B, max_B, t);
    }
}

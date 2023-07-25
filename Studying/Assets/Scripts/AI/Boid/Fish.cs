using System.Linq;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] Transform debug;
    [SerializeField] float speed = 2f;
    [SerializeField] float coesionRange = 2f;
    [SerializeField] float separationRange = 2f;
    [SerializeField] float obstacleRange = 2f;
    [SerializeField] float repulsion = 50f;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] bool d;
    Vector3 direction;
    Vector3 velocity;

    void Start()
    {
        direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }
    void Update()
    {
        Coesion();
        Separation();
        Align();
        AvoidObstacles();

        velocity = direction.normalized * speed * Time.deltaTime;
        transform.forward = velocity;

        transform.position += velocity;
    }

    void AvoidObstacles()
    {
        Collider[] obstacles = Physics.OverlapSphere(transform.position, obstacleRange, obstacleMask);
        if (obstacles.Length == 0f)
            return;

        Vector3 totRepulsion = Vector3.zero;
        foreach(Collider obstacle in obstacles)
        {
            totRepulsion += (transform.position - obstacle.ClosestPoint(transform.position)).normalized 
                * Remap(0f, obstacleRange, Vector3.Distance(transform.position, obstacle.ClosestPoint(transform.position)), 1f, 0f);
        }
        if (d)
        {
            Debug.DrawRay(new Vector3(debug.position.x, 0f, debug.position.z - 5), Vector3.right * repulsion, Color.blue);
        }
        direction += totRepulsion.normalized * repulsion;
    }
    void Coesion()
    {
        GameObject[] neighbors = GameObject.FindGameObjectsWithTag("Boid").Where(
            n => n != gameObject && Vector3.Distance(n.transform.position, transform.position) <= coesionRange).ToArray();

        if (neighbors.Length == 0f)
            return;

        Vector3 totCoesion = Vector3.zero;
        foreach(GameObject neighbor in neighbors)
        {
            totCoesion += neighbor.transform.position;
        }
        totCoesion /= neighbors.Length;

        if (d)
        {
            Debug.DrawRay(new Vector3(debug.position.x, 0f, debug.position.z - 6), Vector3.right * (totCoesion - transform.position).magnitude, Color.red);
        }
        direction += (totCoesion - transform.position);
    }
    void Separation()
    {
        GameObject[] neighbors = GameObject.FindGameObjectsWithTag("Boid").Where(
            n => n != gameObject && Vector3.Distance(n.transform.position, transform.position) <= separationRange).ToArray();

        if (neighbors.Length == 0f)
            return;

        Vector3 totSeparation = Vector3.zero;
        foreach (GameObject neighbor in neighbors)
        {
            totSeparation += (neighbor.transform.position - transform.position)
                 * Remap(0f, obstacleRange, Vector3.Distance(transform.position, neighbor.transform.position), 1f, 0f);
        }

        if (d)
        {
            Debug.DrawRay(new Vector3(debug.position.x, 0f, debug.position.z - 7), Vector3.right * totSeparation.magnitude, Color.green);
        }
        direction += totSeparation;
    }
    void Align()
    {
        GameObject[] neighbors = GameObject.FindGameObjectsWithTag("Boid").Where(
            n => n != gameObject && Vector3.Distance(n.transform.position, transform.position) <= separationRange).ToArray();

        if (neighbors.Length == 0f)
            return;

        Vector3 totAlign = Vector3.zero;
        foreach (GameObject neighbor in neighbors)
        {
            totAlign += neighbor.transform.forward 
                * Remap(0f, obstacleRange, Vector3.Distance(transform.position, neighbor.transform.position), 1f, 0f);
        }
        totAlign /= neighbors.Length;

        if (d)
        {
            Debug.DrawRay(new Vector3(debug.position.x, 0f, debug.position.z - 8), Vector3.right * totAlign.magnitude, Color.yellow);
        }
        direction += totAlign;
    }
    float Remap(float min_A, float max_A, float A, float min_B, float max_B)
    {
        float t = Mathf.InverseLerp(min_A, max_A, A);
        return Mathf.Lerp(min_B, max_B, t);
    }
}

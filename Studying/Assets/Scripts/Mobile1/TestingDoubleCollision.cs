using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TestingDoubleCollision : MonoBehaviour
{
    public float moveIntensity = 2f;
    public float radius = 2f;
    public float maxTime = 2f;
    public float knockbackIntenstiy = 2f;
    Collider[] colliders;
    Vector3 moveDirection;
    Coroutine coroutine;
    void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Obstacles"));

        if (coroutine == null)
            MovePlayer();

        if (colliders.Length > 0)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            StartCoroutine(Knockback(transform.position, colliders[0].ClosestPoint(transform.position)));
        }
    }
    IEnumerator Knockback(Vector3 startPos, Vector3 collisionPoint)
    {
        Vector3 endPos = transform.position + (transform.position - collisionPoint).normalized * knockbackIntenstiy;
        endPos.y = 1f;
        float timer = 0f;
        float percentage; 
        while (true)
        {
            timer += Time.deltaTime;
            percentage = timer / maxTime;
            transform.position = Vector3.Lerp(startPos, endPos, percentage);
            if (transform.position == endPos)
                break;
            yield return null;
        }
        coroutine = null;
    }
    void MovePlayer()
    {
        moveDirection.x = -Input.GetAxis("Horizontal");
        moveDirection.z = -Input.GetAxis("Vertical");
        transform.position += moveDirection.normalized * moveIntensity * Time.deltaTime;
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

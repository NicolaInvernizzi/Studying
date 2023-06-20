using System.Collections;
using UnityEngine;

public class TestingDoubleCollision : MonoBehaviour
{
    // to add: 1. Animation curve for knockback. 2. Lerp for resetting movement from end knockback.
    [SerializeField] float moveIntensity = 2f;
    [SerializeField] float rotationIntensity = 2f;
    [SerializeField] float sphereRadius = 2f;
    [SerializeField] float knockbackTime = 2f;
    [SerializeField] float knockbackIntenstiy = 2f;
    [SerializeField] bool pauseInCollision = false;
    Collider[] colliders;
    Coroutine coroutine;
    Vector3 startingPosition;
    Quaternion startingRotation;
    Vector3 startPosition_K;
    Vector3 endPosition_K;
    Color color_K;
    private void Awake()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }
    void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, sphereRadius, LayerMask.GetMask("Obstacles"));
        Rotate();
        if (coroutine == null && !Input.GetKey(KeyCode.Space))
            transform.position += transform.forward * moveIntensity * Time.deltaTime;

        if (colliders.Length > 0)
        {
            if (pauseInCollision)
                Debug.Break();

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            coroutine = StartCoroutine(Knockback(transform.position, colliders[0]));
        }
    }
    IEnumerator Knockback(Vector3 startPos, Collider knockbackCollider)
    {
        GetKnockbackColor(knockbackCollider);
        startPosition_K = startPos;
        Vector3 collisionPoint = knockbackCollider.ClosestPoint(transform.position);
        endPosition_K = transform.position + (transform.position - collisionPoint).normalized * knockbackIntenstiy;
        endPosition_K.y = 1f;

        float timer = 0f;
        float percentage; 
        while (true)
        {
            timer += Time.deltaTime;
            percentage = timer / knockbackTime;
            transform.position = Vector3.Lerp(startPos, endPosition_K, percentage);
            if (transform.position == endPosition_K)
                break;
            yield return null;
        }
        coroutine = null;
    }
    void GetKnockbackColor(Collider knockbackCollider)
    {
        KnockBackColor knockbackColor = knockbackCollider.GetComponent<KnockBackColor>();
        if (knockbackColor == null)
        {
            Debug.LogWarning("The obstacle hasn't knockback color");
            color_K = Color.black;
        }
        else
            color_K = knockbackColor.GetColor();
    }
    void Rotate()
    {
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.up * rotationIntensity * 10 * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.up * (- rotationIntensity) * 10 * Time.deltaTime);
    }
    void ResetTransform()
    {
        transform.position = startingPosition;
        transform.rotation = startingRotation;
    }
    void DrawKnockback()
    {
        if (coroutine != null)
        {
            Gizmos.color = color_K;
            Gizmos.DrawWireSphere(endPosition_K, sphereRadius);
            Gizmos.DrawLine(startPosition_K, endPosition_K);
        }           
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
        DrawKnockback();
    }
    void OnGUI()
    {
        if (GUILayout.Button("Reset player transform"))
            ResetTransform();
    }
}

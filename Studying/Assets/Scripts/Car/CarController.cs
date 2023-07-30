using UnityEngine;

/*
 * Movement in forword (F) costante.
 * Forza di derapata (D) aumenta all'aumentare di angolo tra F e D (a).
 * Sposto mouse su schermo -> macchina ruota verso il mouse con velocità di rotazione (R).
 * 
 * 
*/
public class CarController : MonoBehaviour
{
    [SerializeField, Range(0f, 50f)]
    float maxSpeed = 10f;
    [SerializeField, Range(0f, 50f)]
    float movementMagnitude = 0.5f;
    [SerializeField, Range(1f, 200f)]
    float rotationSpeed = 100f;
    [SerializeField, Range(0f, 200f)]
    float driftMagnitude = 2f;
    [SerializeField, Range(0f, 200f)]
    float sensibility = 0.5f;
    [SerializeField, Range(0f, 50f)]
    float debugLength = 10f;
    [SerializeField]
    bool visualizer = true;
    [SerializeField]
    AnimationCurve driftCurve;
    [SerializeField]
    AnimationCurve speedCurve;

    Vector3 movementVelocity;
    Vector3 driftVelocity;
    Vector3 mouseDirection;
    Ray mouseRay;

    private void Start()
    {
        driftVelocity = transform.forward * driftMagnitude;
    }
    private void Update()
    {
        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        mouseDirection = new Vector3(mouseRay.direction.x, 0f, mouseRay.direction.z).normalized;

        movementVelocity = transform.forward * movementMagnitude;
        movementVelocity += Drift();

        Rotate();
        Move(movementVelocity);
        Visualizer(visualizer);
    }

    private Vector3 Drift()
    {
        driftVelocity = Vector3.RotateTowards(
            driftVelocity, 
            transform.forward * movementMagnitude, 
            sensibility * Time.deltaTime, 
            1f).normalized * driftMagnitude;

        driftVelocity = driftVelocity * Remap(0f, 180f, Vector3.Angle(driftVelocity, transform.forward), 0f, 1f, driftCurve);

        return driftVelocity;
    }
    private void Move(Vector3 velocity)
    {
        Vector3 finalVelocity = velocity.normalized * Remap(0f, 180f, Vector3.Angle(velocity, transform.forward), 1f, 0.5f, speedCurve);
        transform.position += finalVelocity * maxSpeed * Time.deltaTime;
    }
    private void Rotate()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(mouseDirection), rotationSpeed * Time.deltaTime);
    }
    private void Visualizer(bool show)
    {
        if (!show)
            return;

        Debug.DrawRay(transform.position, transform.forward * movementMagnitude * debugLength, Color.blue);
        Debug.DrawRay(transform.position, mouseDirection * debugLength, Color.cyan);
        Debug.DrawRay(transform.position, driftVelocity * debugLength, Color.yellow);
        Debug.DrawRay(transform.position, movementVelocity * debugLength, Color.magenta);
        //Debug.DrawLine(transform.position + mouseDirection * debugLength, transform.position + transform.forward * movementMagnitude * debugLength, Color.white);
        //Debug.DrawLine(transform.position + transform.forward * movementMagnitude * debugLength, transform.position + movementVelocity * debugLength, Color.white);
        //Debug.DrawRay(Camera.main.transform.position, mouseRay.direction * 10, Color.red);
    }
    float Remap(float min_A, float max_A, float A, float min_B, float max_B, AnimationCurve curve)
    {
        float t = Mathf.InverseLerp(min_A, max_A, A);
        t = curve.Evaluate(t);
        return Mathf.Lerp(min_B, max_B, t);
    }         
}

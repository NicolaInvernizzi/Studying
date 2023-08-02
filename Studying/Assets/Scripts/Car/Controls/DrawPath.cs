using UnityEngine;

public class DrawPath : MonoBehaviour
{
    [Tooltip("Line drawing frequency ")]
    [SerializeField, Range(0.1f, 5f)] float frequency_path = 0.5f;

    [Tooltip("Line life time")]
    [SerializeField, Range(0.5f, 30f)] float lifeTime_path = 5f;
    [SerializeField] bool draw_path;

    float timer_path;
    Vector3 startPosition_path = Vector3.zero;
    void Update() => Draw(draw_path, frequency_path, lifeTime_path, transform);
    private void Draw(bool enable, float frequency, float lifeTime, Transform transform)
    {
        if (enable)
        {
            // set default start position only once
            if (startPosition_path == Vector3.zero)
                startPosition_path = transform.position;

            // draw temporary line (cyan)
            Debug.DrawLine(transform.position, startPosition_path, Color.cyan);
            timer_path += Time.deltaTime;

            // draw permanent line (white) with a lifeTime duration + update values
            if (timer_path >= frequency)
            {
                Debug.DrawLine(transform.position, startPosition_path, Color.white, lifeTime);
                startPosition_path = transform.position;
                timer_path = 0f;
            }
        }
        // reset values
        else if (!enable && startPosition_path != Vector3.zero)
        {
            startPosition_path = Vector3.zero;
            timer_path = 0f;
        }
    }
}

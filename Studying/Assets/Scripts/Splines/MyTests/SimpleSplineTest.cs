using UnityEngine;

public class SimpleSplineTest : MonoBehaviour
{
    public Transform[] points;
    public int steps;
    Vector3 start;
    Vector3 curvePoint;
    public bool red;
    public bool yellow;
    public bool gray;
    public bool cyan;
    public bool orange;
    public bool blue;
    public bool curve;
    private void OnDrawGizmos()
    {
        start = points[0].position;

        for (int i = 0; i <= steps; i++)
        {
            curvePoint = DrawBezier(points[0].position, points[1].position, points[2].position, 
                points[3].position, (float)i /steps);
            
            if (blue)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, curvePoint);
            }

            if (curve)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(start, curvePoint);
            }

            start = curvePoint;
        }
    }
    Vector3 DrawBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 val1 = Vector3.Lerp(p0, p1, t);
        Vector3 val2 = Vector3.Lerp(p1, p2, t);
        Vector3 val3 = Vector3.Lerp(p2, p3, t);

        if (cyan)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(val1, val2);
            Gizmos.DrawLine(val2, val3);
        }    

        if (red)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, val2);
        }

        if (yellow)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, val1);
        }

        if (gray)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(transform.position, val3);
        }

        Vector3 val12 = Vector3.Lerp(val1, val2, t);
        Vector3 val23 = Vector3.Lerp(val2, val3, t);

        if (orange)
        {
            Gizmos.color = new Color(255, 95, 31);
            Gizmos.DrawLine(val12, val23);
        }

        return Vector3.Lerp(val12, val23, t);
    }
}

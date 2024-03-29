using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Vector3[] points;
    private void Reset() // called when by the editor when the component is created or reset
    {
        points = new Vector3[]
        {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
        };
    }
    public Vector3 GetPoint (float t)
    {
        return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], t));
    }
}

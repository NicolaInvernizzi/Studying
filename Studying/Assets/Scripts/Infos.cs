using UnityEditor;
using UnityEngine;

public class Infos : MonoBehaviour
{
    public enum T
    {
        Position, 
        Rotation, 
        Scale, 
        None
    }
    public Vector3 position;
    public bool gameVisibility;
    public bool sceneVisibility;
    public bool positionVisibility;
    public bool rotationVisibility;
    public bool scaleVisibility;

    private string _transform_text;

    private void Start()
    {
        _transform_text = string.Empty;
    }
    private void Update()
    {
        _transform_text = 
            TransformInfos(positionVisibility, T.Position) +
            TransformInfos(rotationVisibility, T.Rotation) +
            TransformInfos(scaleVisibility, T.Scale);
    }

    public string TransformInfos(bool visibility, T operation)
    {
        string text = string.Empty;
        if(visibility)
        {
            switch (operation)
            {
                case T.Position:
                    text = "Position: " + transform.position.ToString();
                    break;
                case T.Rotation:
                    text = "Rotation: " + transform.eulerAngles.ToString();
                    break;
                case T.Scale:
                    text = "Scale: " + transform.localScale.ToString();
                    break;
            }
            text += "\n";
        }
        return text;
    }

    private void OnDrawGizmos()
    {
        if (sceneVisibility)
            Handles.Label(transform.position + position, _transform_text);
    }

    private void OnGUI()
    {
        if(gameVisibility)
            Handles.Label(transform.position + position, _transform_text);
    }
}

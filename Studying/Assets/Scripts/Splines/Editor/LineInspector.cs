using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Line))] // override default editor for Line class
public class LineInspector : Editor
{
    private void OnSceneGUI() // draw in scene view, called only if you move the mouse in scene view
    {
        Line line = target as Line; // target is the object been inspected (the Line class) -> in order to access the Line class variables you have to cast it.
        Transform handleTransform = line.transform;
        Quaternion handleRotation;

        if (Tools.pivotRotation == PivotRotation.Local)// the local/world button on top of the scene view
        {
            handleRotation = handleTransform.rotation;
        }   
        else
        {
            handleRotation = Quaternion.identity;
        }

        // p0 & p1 relative to the Line transform position (and rotation)
        Vector3 p0 = handleTransform.TransformPoint(line.p0);
        Vector3 p1 = handleTransform.TransformPoint(line.p1);

        // Handles set up:

        Handles.DrawLine(p0, p1);
        Handles.color = Color.white; // always draw line between them

        EditorGUI.BeginChangeCheck(); // check only p0: the method compare the last and current p0 value
        p0 = Handles.DoPositionHandle(p0, handleRotation); // the red, blue, green axis gizmo.
        // besically every time the method draw the gizmo in p0 and it return the new handle position if moved. So the nex time the handle will be 
        // drawn in the updated p0 position.

        if (EditorGUI.EndChangeCheck()) // if the last and current state are different -> true
        {
            Undo.RecordObject(line, "Move point"); // save line status with a name when mouse up
            EditorUtility.SetDirty(line); // set the script dirty, Unity will ask to save when quitting
            line.p0 = handleTransform.InverseTransformPoint(p0); // move line p0 position if you move the handle
        }

        EditorGUI.BeginChangeCheck();
        p1 = Handles.DoPositionHandle(p1, handleRotation); 
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(line, "Move point");
            EditorUtility.SetDirty(line);
            line.p1 = handleTransform.InverseTransformPoint(p1);
        }
    }
}

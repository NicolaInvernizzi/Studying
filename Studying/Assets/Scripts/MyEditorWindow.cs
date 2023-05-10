using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;

public class MyEditorWindow : EditorWindow
{
    private float[] values = new float[3] {100, 100, 100};
    private string[] strings = new string[3] {"Insert text", "Insert text", "Insert text"};
    [MenuItem("Window/DBGA/My Editor")]
    private static void OpenMyEditorWindow()
    {
        MyEditorWindow window = GetWindow<MyEditorWindow>("My Editor Window");
        window.minSize = new Vector2(200, 200);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Test", GUILayout.Width(200));
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        strings[0] = GUILayout.TextField(strings[0]);
        values[0] = EditorGUILayout.FloatField(values[0], GUILayout.Width(100));
        if (GUILayout.Button(strings[0], GUILayout.Height(values[0])))
            Debug.Log(strings[0]);
        strings[1] = GUILayout.TextField(strings[1]);
        values[1] = EditorGUILayout.FloatField(values[1], GUILayout.Width(100));
        if (GUILayout.Button(strings[1], GUILayout.Height(values[1])))
            Debug.Log(strings[1]);
        strings[2] = GUILayout.TextField(strings[2]);
        values[2] = EditorGUILayout.FloatField(values[2], GUILayout.Width(100));
        if (GUILayout.Button(strings[2], GUILayout.Height(values[2])))
            Debug.Log(strings[2]);
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Close"))
            Close();
    }
}

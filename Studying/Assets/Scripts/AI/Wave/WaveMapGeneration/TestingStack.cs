using UnityEngine;
using System.Collections.Generic;

public class TestingStack : MonoBehaviour
{
    public Stack<List<Test>> stack = new Stack<List<Test>>();
    public List<Test> vertices = new List<Test>();

    private void Start()
    {
        vertices.Add(new Test(0));
        vertices.Add(new Test(1));
        vertices.Add(new Test(2));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Test current = vertices.Find(v => v.id == 1);
            current.bol = true;
            stack.Push(vertices);
            vertices.Clear();
            vertices = stack.Pop();
            Debug.Log(vertices.Find(v => v.bol == true).id);
        }
    }
}
public class Test
{
    public int id;
    public bool bol;
    public Test(int id)
    {
        this.id = id;
    }
}

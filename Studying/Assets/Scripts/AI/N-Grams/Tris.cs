using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tris : MonoBehaviour
{
    Dictionary<string, int> aiStatus = new Dictionary<string, int>();
    public string[] possibleMoves = new string[3] { "R", "S", "P"};
    Queue<string> lastMoves = new Queue<string>();
    string botMove;

    void OnGUI()
    {
        foreach(string move in possibleMoves)
        {
            if (GUILayout.Button(move))
            {
                if (aiStatus.Keys.Count > 0)
                    AIMove();
                else
                    RandomMoveAI(possibleMoves);

                print($"AI move: {botMove}");
                print($"Player move: {move}");

                FixedSizeEnqueue<string>(lastMoves, move, 3);
                if (lastMoves.Count == 3)
                    UpdateDictionary();
            }
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Print aiStatus"))
            PrintDictionary(aiStatus);
        if (GUILayout.Button("Print lastMoves"))
            PrintQueue(lastMoves);
    }
    void MoveInfos()
    {
        switch (botMove)
        {
            case "R":
                botMove = "P";
                break;
            case "P":
                botMove = "S";
                break;
            default:
                botMove = "R";
                break;
        }
    }
    void AIMove()
    {
        Dictionary<string, int> possibleKeysValues = new Dictionary<string, int>();
        string refMoves = string.Join("", lastMoves.Skip(1));

        foreach (string key in aiStatus.Keys)
        {
            if (key.Contains(refMoves))
                possibleKeysValues.Add(key, aiStatus[key]);
        }

        if (possibleKeysValues.Keys.Count > 0)
        {
            List<string> highestKeys = new List<string>();
            foreach(string key in possibleKeysValues.Keys)
            {
                if (possibleKeysValues[key] == possibleKeysValues.Values.Max())
                    highestKeys.Add(key);
            }

            botMove = char.ToString(highestKeys[UnityEngine.Random.Range(0, highestKeys.Count)][2]);
            MoveInfos();
        }
        else
            RandomMoveAI(possibleMoves);
    }
    void RandomMoveAI(string[] array)
    {
        botMove = array[UnityEngine.Random.Range(0, array.Length)];
        botMove += " (Random)";
    }
    void UpdateDictionary()
    {
        string lastMovesString = string.Join("", lastMoves);

        try
        {
            aiStatus[lastMovesString] ++;
            print("Increase");
        }
        catch(KeyNotFoundException)
        {
            aiStatus.Add(lastMovesString, 1);
            print("Add");
        }
    }
    void FixedSizeEnqueue<T>(Queue<T> queue, T toAdd, int maxLength)
    {
        if (queue.Count == maxLength)
            queue.Dequeue();
        queue.Enqueue(toAdd);
    }
    void PrintQueue(Queue<string> queue)
    {
        int counter = 0;
        foreach (string value in queue)
        {
            print($"{counter}: " + value);
            counter++;
        }
    }
    void PrintDictionary(Dictionary<string, int> toPrint)
    {
        foreach (string key in toPrint.Keys)
        {
            print($"Key: {key}");
            print($"Value: {toPrint[key]}");
        }
    }
    void PrintPercentage()
    {

    }

    //[ContextMenu("Test")]
    //void Test()
    //{
    //    string[] st = new string[3] { "Rock", "Scissor", "Paper" };
    //    Dictionary<string[], int> test = new Dictionary<string[], int>()
    //    {
    //        { new string[3] { "Rock", "Scissor", "Paper" }, 1 },
    //        { new string[3] { "Scissor", "Scissor", "Rock" }, 2 },
    //        { new string[3] { "Paper", "Rock", "Scissor" }, 3 },
    //    };

    //    if (test.TryGetValue(st, out int value))
    //        print(value);
    //    else
    //        print("Not founded");

    //}
    //[ContextMenu("Tes3")]
    //void Tes3()
    //{
    //    string a = "RSP";
    //    print(a.Contains("RSP"));
    //}
}

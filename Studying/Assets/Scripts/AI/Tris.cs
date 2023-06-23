using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

// N grams
public class Tris : MonoBehaviour
{
    public Dictionary<string[], int> dictionary = new Dictionary<string[], int>();
    public string[] possibleMoves = new string[3] { "Rock", "Scissor", "Paper"};
    public Queue<string> lastMoves = new Queue<string>();
    public string botMove;

    void OnGUI()
    {
        foreach(string move in possibleMoves)
        {
            if (GUILayout.Button(move))
            {
                Game();
                print($"Player move: {move}");            
            }
        }
    }

    void Game()
    {
        if (lastMoves.Count == 3)
        {
            UpdateDictionary();
            AIMove();
            print($"AI move: {botMove}");
        }
        else
        {
            botMove = possibleMoves[Random.Range(0, possibleMoves.Length)];
            print($"AI random move: {botMove}");
        }      
    }
    string MoveInfos(string value)
    {
        switch (value)
        {
            case "Rock":
                botMove = "Paper";
                break;
            case "Paper":
                botMove = "Scissor";
                break;
            case "Scissor":
                botMove = "Rock";
                break;
        }

        return botMove;
    }
    void AIMove()
    {
        Dictionary<string[], int> tempDictionary = new Dictionary<string[], int>();   
        string[] lastMovesArray = lastMoves.Skip(1).ToArray();

        foreach (string[] key in dictionary.Keys)
        {
            if (key[0] == lastMovesArray[0] && key[1] == lastMovesArray[1])
                tempDictionary.Add(key, dictionary[key]);
        }

        string[] result = tempDictionary.FirstOrDefault(x => x.Value == tempDictionary.Values.Max()).Key;

        if (tempDictionary.Count == 0)
            botMove = possibleMoves[Random.Range(0, possibleMoves.Length)];
        else
        {
            botMove = result[2];
            botMove = MoveInfos(botMove);
        }
    }
    void UpdateDictionary()
    {
        if (dictionary.TryGetValue(lastMoves.ToArray(), out int value))
            dictionary[lastMoves.ToArray()] = ++value;
        else
            dictionary.Add(lastMoves.ToArray(), 1);
    }
    void FixedSizeEnqueue<T>(Queue<T> queue, T toAdd, int maxLength)
    {
        if (queue.Count == maxLength)
            queue.Dequeue();
        queue.Enqueue(toAdd);
    }
    void PrintQueue<T>(Queue<T> queue)
    {
        int counter = 0;
        foreach (T value in queue)
        {
            print($"{counter}: " + value);
            counter++;
        }
    }
}

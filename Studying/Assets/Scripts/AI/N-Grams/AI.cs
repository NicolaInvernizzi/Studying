using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AI : Player
{
    Queue<string> opponentLastMoves = new Queue<string>();
    Dictionary<string, string> moveInfos = new Dictionary<string, string>()
    {
        { "R", "P" },
        { "P", "S" },
        { "S", "R" }
    };
    Dictionary<string, int> aiStatus = new Dictionary<string, int>();
    Text opponentLastMoves_Text;
    Text aiInfos_Text;
    Text probability_Text;
    protected sealed override void Awake()
    {
        base.Awake();
        opponentLastMoves_Text = transform.Find("InfosOrganizer").Find("QueuePanel").GetComponentInChildren<Text>();
        aiInfos_Text = transform.Find("InfosOrganizer").Find("DictionaryPanel").GetComponentInChildren<Text>();
        probability_Text = transform.Find("InfosOrganizer").Find("ProbabilityPanel").GetComponentInChildren<Text>();
    }
    void AIMove()
    {
        Dictionary<string, int> possibleKeysValues = new Dictionary<string, int>();
        string refMoves = string.Join("", opponentLastMoves.Skip(1));

        foreach (string key in aiStatus.Keys)
        {
            if (key.Contains(refMoves))
                possibleKeysValues.Add(key, aiStatus[key]);
        }

        if (possibleKeysValues.Keys.Count > 0)
        {
            List<string> highestKeys = new List<string>();
            foreach (string key in possibleKeysValues.Keys)
            {
                if (possibleKeysValues[key] == possibleKeysValues.Values.Max())
                    highestKeys.Add(key);
            }

            move = char.ToString(highestKeys[Random.Range(0, highestKeys.Count)][2]);
            move = moveInfos[move];
        }
        else
            move = GameManager.instance.RandomMove();
        moveText.text = move;
    }
    void UpdateDictionary()
    {
        string lastMovesString = string.Join("", opponentLastMoves);

        try
        {
            aiStatus[lastMovesString]++;
            print("Increase");
        }
        catch (KeyNotFoundException)
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
        string text = "";
        int counter = 0;
        foreach (string value in queue)
        {
            text = $"{counter}: {value}\n";
            counter++;
        }
        opponentLastMoves_Text.text = text;
    }
    void PrintDictionary(Dictionary<string, int> toPrint)
    {
        string text = "";
        foreach (string key in toPrint.Keys)
            text = $"Key: {key}" + "  " + $"Value: {toPrint[key]}\n";
        aiInfos_Text.text = text;
    }
    void PrintProbability()
    {

    }
    public void RefreshData(string opponentMove)
    {
        FixedSizeEnqueue<string>(opponentLastMoves, opponentMove, 3);
        if (opponentLastMoves.Count == 3)
            UpdateDictionary();
        PrintQueue(opponentLastMoves);
        PrintDictionary(aiStatus);
        // PrintProbability();
    }
    IEnumerator Play()
    {
        yield return new WaitForSeconds(1);

        if (aiStatus.Keys.Count > 0)
            AIMove();
        else
            move = GameManager.instance.RandomMove();
    }
    public sealed override AI CheckAI() => this;
}

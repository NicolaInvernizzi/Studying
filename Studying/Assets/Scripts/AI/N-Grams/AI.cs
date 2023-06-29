using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AI : Player
{
    [Space(10), Header("AI settings")]
    [SerializeField] float simulationTime;
    [SerializeField] bool randomTime;
    [SerializeField] int nGram_Length;
    [SerializeField] int nGram_Window;
    public Text opponentLastMovesText, aiInfosText1, aiInfosText2, probabilityText;
    Queue<string> opponentLastMoves = new Queue<string>();
    Dictionary<string, string> moveInfos = new Dictionary<string, string>()
    {
        { "R", "P" },
        { "P", "S" },
        { "S", "R" }
    };
    Dictionary<string, int> aiStatus = new Dictionary<string, int>();

    //void Start() => NewProbability(string.Join("", opponentLastMoves.Skip(nGram_Window)));
    public void RandomMove()
    {
        SetMove(moveInfos.ElementAt(UnityEngine.Random.Range(0, moveInfos.Keys.Count)).Key);
    }
    void AIMove()
    {
        Dictionary<string, int> possibleKeysValues = new Dictionary<string, int>();

        foreach (string key in aiStatus.Keys)
        {
            if (FindPortionKey(key, string.Join("", opponentLastMoves.Skip(nGram_Window))))
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
            SetMove(moveInfos[char.ToString(highestKeys[UnityEngine.Random.Range(0, highestKeys.Count)][nGram_Length - nGram_Window])]);
        }
        else
            RandomMove();
    }
    bool FindPortionKey(string toCompare, string toFind)
    {
        for(int i = 0; i < toFind.Length; i++)
        {
            if (toCompare[i] != toFind[i])
                return false;
        }
        return true;
    }
    void UpdateDictionary()
    {
        string lastMovesString = string.Join("", opponentLastMoves);

        try
        {
            aiStatus[lastMovesString]++;
        }
        catch (KeyNotFoundException)
        {
            aiStatus.Add(lastMovesString, 1);
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
        string text = "";
        foreach (string value in queue)
        {
            text += $"[{counter}]{value}, ";
            counter++;
        }
        opponentLastMovesText.text = text;
    }
    void PrintDictionary()
    {
        int counter = 0;
        string text1 = "";
        string text2 = "";

        foreach (string key in aiStatus.Keys)
        {
            if (counter <= 17)
                text1 += $"[{counter}] Key: {key}" + "  " + $"Value: {aiStatus[key]}\n";
            else
                text2 += $"[{counter}] Key: {key}" + "  " + $"Value: {aiStatus[key]}\n";
            counter++;
        }
        aiInfosText1.text = text1;
        aiInfosText2.text = text2;
    }
    void PrintProbability(Dictionary<string, float> toPrint)
    {
        print("Probability printed");

        int counter = 0;
        string text = "";

        foreach (string key in toPrint.Keys)
        {
            text += $"[{counter}] Key: {key}" + "  " + $"Value: {toPrint[key]}%\n";
            counter++;
        }
        probabilityText.text = text;
    }
    void NewProbability(string toFind)
    {
        List<string> keys = new List<string>(aiStatus.Keys);
        Dictionary<string, float> probabilities = new Dictionary<string, float>()
        {
            { "R", 0 },
            { "P", 0 },
            { "S", 0 }
        };

        List<string> foundedKeys = new List<string>();
        foreach(string key in keys)
        {
            if (FindPortionKey(key, toFind))
                foundedKeys.Add(key);
        }

        foreach(string key in foundedKeys)
            probabilities[moveInfos[probabilities.Keys.ToList().Find(k => k == char.ToString(key[nGram_Length - nGram_Window]))]] = aiStatus[key];
        List<string> keysP = new List<string>(probabilities.Keys);
        foreach (string key in keysP)
            probabilities[key] /= probabilities.Values.Sum();

        PrintProbability(probabilities);
    }
    public void AIManagement(string opponentMove)
    {
        FixedSizeEnqueue<string>(opponentLastMoves, opponentMove, nGram_Length);
        if (opponentLastMoves.Count == nGram_Length)
            UpdateDictionary();
        PrintQueue(opponentLastMoves);
        PrintDictionary();

        //NewProbability(string.Join("", opponentLastMoves.Skip(nGram_Window)));
    }
    public IEnumerator Playing()
    {
        yield return new WaitForSeconds(randomTime ? UnityEngine.Random.Range(0.5f, simulationTime) : simulationTime);

        if (aiStatus.Keys.Count > 0)
            AIMove();
        else
            RandomMove();
        moveText.text = "Decided";
    }
    public sealed override void Play() 
    {
        base.Play();
        StartCoroutine(Playing());
    }
    public sealed override AI CheckAI() => this;
}

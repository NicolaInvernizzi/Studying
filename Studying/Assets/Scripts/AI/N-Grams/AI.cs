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

    void Start() => NewProbability(string.Join("", opponentLastMoves.Skip(nGram_Window)), true, false);
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
        int c = 0;
        string text;

        if (opponentLastMoves.Count < nGram_Length)
            text = $"Insufficient opponent last moves. The probabilies are:\n";
        else
            text = $"With {string.Join("", opponentLastMoves.Skip(nGram_Window))} the probabilies are:\n";
    
        foreach (string key in toPrint.Keys)
        {
            text += $"[{c}] Key: {key}" + "  " + $"Prob: {toPrint[key]}%\n";
            c++;
        }
        probabilityText.text = text;
    }

    Dictionary <string, float> NewProbability(string toFind, bool print, bool getProbability)
    {
        Dictionary<string, float> probabilities = new Dictionary<string, float>()
        {
            { "R", 0 },
            { "P", 0 },
            { "S", 0 }
        };
        List<string> statusKeys = new List<string>(aiStatus.Keys);
        List<string> probKeys = new List<string>(probabilities.Keys);
        List<float> probValues = new List<float>();
        List<string> foundedKeys = new List<string>();

        if (statusKeys.Exists(k => FindPortionKey(k, toFind)))
        {
            foreach (string key in statusKeys)
            {
                if (FindPortionKey(key, toFind))
                    foundedKeys.Add(key);
            }
            foreach (string key in foundedKeys)
                probabilities[moveInfos[probabilities.Keys.ToList().
                    Find(k => k == char.ToString(key[nGram_Length - nGram_Window]))]] = aiStatus[key];
            foreach (string key in probKeys)
                probValues.Add(Mathf.Floor((probabilities[key] / probabilities.Values.Sum()) * 100));
            int c = 0;
            foreach (string key in probKeys)
            {
                probabilities[key] = probValues[c];
                c++;
            }
        }
        else
            probKeys.ForEach(k => probabilities[k] = MathF.Floor(100/probKeys.Count));

        if (print)
            PrintProbability(probabilities);
        if (getProbability)
            return probabilities;
        else
            return null;
    }
    public void AIManagement(string opponentMove)
    {
        FixedSizeEnqueue<string>(opponentLastMoves, opponentMove, nGram_Length);
        if (opponentLastMoves.Count == nGram_Length)
            UpdateDictionary();
        PrintQueue(opponentLastMoves);
        PrintDictionary();

        NewProbability(string.Join("", opponentLastMoves.Skip(nGram_Window)), true, false);
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

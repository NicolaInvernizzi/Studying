using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    public float simulationTime = 1f;
    public Player[] players;
    public Text matchInfo;
    Coroutine coroutine;

    void Start() => StartGame();
    void Update()
    {
        if (coroutine == null && (players.All(p => p.move != "")))
            coroutine = StartCoroutine(Game(players[0].move, players[1].move));
    }
    IEnumerator Game(string leftMove, string rightMove)
    {
        matchInfo.text = "Showing move...";
        yield return new WaitForSeconds(simulationTime);
        Array.ForEach(players, p => p.ShowMove());
        yield return new WaitForSeconds(simulationTime);

        matchInfo.text = "Deciding winner...";
        yield return new WaitForSeconds(simulationTime);
        Score(leftMove, rightMove);
        yield return new WaitForSeconds(simulationTime);

        matchInfo.text = "Updating AI datas...";
        yield return new WaitForSeconds(simulationTime);

        for(int i = 0; i < players.Length; i++)
            players[i].CheckAI()?.AIManagement(players[players.Length - (1 + i)].move);
        yield return new WaitForSeconds(simulationTime);

        matchInfo.text = "Resetting...";
        yield return new WaitForSeconds(simulationTime);

        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetMove("");
            players[i].ShowMove();
        }
        yield return new WaitForSeconds(simulationTime);

        matchInfo.text = "New match...";
        yield return new WaitForSeconds(simulationTime);
        StartGame();
        coroutine = null;
    }
    void StartGame()
    {
        matchInfo.text = "Waiting for moves...";
        Array.ForEach(players, p => p.Play());
    }
    void Score(string leftMove, string rightMove)
    {
        string text1;
        string text2 = " wins";
        if (leftMove == rightMove)
        {
            text1 = "Draw";
            text2 = "";
        }
        else if ((rightMove == "R" && leftMove == "S") ||
        (rightMove == "S" && leftMove == "P") ||
            (rightMove == "P" && leftMove == "R"))
        {
            text1 = players[1].playerName;
            players[1].IncreaseScore();
        }
        else
        {
            text1 = players[0].playerName;
            players[0].IncreaseScore();
        }
        matchInfo.text = text1 + text2;

        int maxScore = players.Max(p => p.score);
        Player currentWinner = players.First(p => p.score == maxScore);
        Player[] losers = players.Where(p => p.score != maxScore).ToArray();
        foreach(Player p in losers)
        {
            if (p.scoreBar.activeSelf)
            {
                p.scoreBar.SetActive(false);
                break;
            }
        }
        currentWinner.ChangeScoreBar(Mathf.Abs(players[0].score - players[1].score));
    }
}

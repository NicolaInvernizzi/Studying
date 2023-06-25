using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public string[] possibleMoves = new string[3] { "R", "S", "P" };
    public Player playerLeft;
    public Player playerRight;
    Text matchInfo;
    public bool waitingForMoves = true;
    Coroutine coroutine;

    private void Awake()
    {
        instance = this;
        transform.Find("MatchInfo").GetComponent<Text>();
    }
    private void Update()
    {
        if (coroutine == null && (playerLeft.move != "" && playerRight.move != ""))
            coroutine = StartCoroutine(Game(playerLeft.move, playerRight.move));
    }
    public string RandomMove()
    {
        return possibleMoves[UnityEngine.Random.Range(0, possibleMoves.Length)];
    }
    IEnumerator Game(string leftMove, string rightMove)
    {
        waitingForMoves = false;

        matchInfo.text = "Deciding winner...";
        yield return new WaitForSeconds(1);
        Score(leftMove, rightMove);
        yield return new WaitForSeconds(2);
        matchInfo.text = "Resetting...";
        yield return new WaitForSeconds(1);
        matchInfo.text = "Waiting for moves...";

        waitingForMoves = true;

        playerLeft.CheckAI()?.RefreshData(rightMove);
        playerRight.CheckAI()?.RefreshData(leftMove);
    }
    void Score(string leftMove, string rightMove)
    {
        string text1;
        string text2 = "wins";
        if (leftMove == playerRight.move)
        {
            text1 = "Draw";
            text2 = "";
        }
        else if ((rightMove == "R" && leftMove == "S") ||
        (rightMove == "S" && leftMove == "P") ||
            (rightMove == "P" && leftMove == "R"))
        {
            text1 = playerRight.name;
            playerRight.IncreaseScore();
        }
        else
        {
            text1 = playerLeft.name;
            playerLeft.IncreaseScore();
        }
        matchInfo.text = text1 + text2;
    }
}

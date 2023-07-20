using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class GameLogic : MonoBehaviour
{
    Dictionary<string, int> scores = new Dictionary<string, int>()
    {
        {"X", 1},
        {"O", -1},
        {"D", 0},
    };
    public static List<ButtonPosition> buttons = new List<ButtonPosition>();

    [SerializeField]
    Text roundText;

    [SerializeField]
    string simbolA;

    [SerializeField]
    string simbolB;

    string[,] board = new string[3, 3];

    Round currentRound;
    string currentSimbol;
    bool cliked;
    Coroutine aiCoroutine;

    enum Round
    {
        A,
        B
    }

    void Start()
    {
        Restart();
    }
    public void Click()
    {
        if (!cliked)
        {
            ButtonPosition buttonScript = EventSystem.current.currentSelectedGameObject.gameObject.GetComponent<ButtonPosition>();
            buttonScript.SetSimbol(currentSimbol, false);
            board[buttonScript.raw, buttonScript.column] = currentSimbol;
            cliked = true;
            VerifyBoard();
        }
    }
    public void Restart()
    {
        if (aiCoroutine != null)
            StopCoroutine(aiCoroutine);

        foreach (ButtonPosition b in buttons)
        {
            b.SetSimbol("", true);
            b.Interactable(true);
        }

        board = new string[3, 3];
        currentRound = (Round)Random.Range(0, 2);
        ChangeRound();
    }
    void ChangeRound()
    {
        switch (currentRound)
        {
            case Round.A:
                currentRound = Round.B;
                currentSimbol = simbolB;
                roundText.text = $"{simbolB} is playing...";
                cliked = false;
                break;
            case Round.B:
                cliked = true;
                currentRound = Round.A;
                currentSimbol = simbolA;
                roundText.text = $"{simbolA} is playing...";
                aiCoroutine = StartCoroutine(AI());
                break;
        }
    }
    void VerifyBoard()
    {
        string status = CheckWinner(board);

        if (status == simbolA)
        {
            roundText.text = $"{status} wins";
            buttons.ForEach(b => b.Interactable(false));
            return;
        }

        else if (status == simbolB)
        {
            roundText.text = $"{status} wins";
            buttons.ForEach(b => b.Interactable(false));
            return;
        }

        else if (status == "D")
        {
            roundText.text = "Draw";
            return;
        }

        else
            ChangeRound();
    }

    IEnumerator AI()
    {
        yield return new WaitForSeconds(1);
        int bestScore = -100;
        int[] bestMove = new int[2];

        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                if (board[i, j] == null)
                {
                    board[i, j] = currentSimbol;
                    int score = MinMax(board, false);
                    board[i, j] = null;
                    
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = new int[] { i, j };
                    }
                }
            }
        }

        board[bestMove[0], bestMove[1]] = currentSimbol;
        
        foreach (ButtonPosition b in buttons)
        {
            if (b.CheckRawColumn(bestMove[0], bestMove[1]))
                b.SetSimbol(currentSimbol, false);
        }
        print($"Ai moved in: {bestMove[0]} {bestMove[1]}");
        VerifyBoard();
    }
    int MinMax(string[,] state, bool isMax)
    {
        string result = CheckWinner(state);
        print(result);
        if (result != "")
            return scores[result];

        if (isMax)
        {
            int bestScore = -100;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == null)
                    {
                        board[i, j] = simbolA;
                        int score = MinMax(board, false);
                        board[i, j] = null;
                        bestScore = Mathf.Max(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = 100;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == null)
                    {
                        board[i, j] = simbolB;
                        int score = MinMax(board, true);
                        board[i, j] = null;
                        bestScore = Mathf.Min(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
    }
    string CheckWinner(string[,] stateToCheck)
    {
        int c = 0;
        // check raw
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (stateToCheck[i, j] != null)
                {
                    if (stateToCheck[i, j] == stateToCheck[i, j + 1])
                        c++;
                    if (c == 2)
                        return stateToCheck[i, j];
                }
            }
            c = 0;
        }

        // check column
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (stateToCheck[j, i] != null)
                {
                    if (stateToCheck[j, i] == stateToCheck[j + 1, i])
                        c++;
                    if (c == 2)
                        return stateToCheck[j, i];
                }
            }
            c = 0;
        }

        // check diagonal1
        for (int i = 0; i < 2; i++)
        {
            if (stateToCheck[i, i] != null)
            {
                if (stateToCheck[i, i] == stateToCheck[i + 1, i + 1])
                    c++;
                if (c == 2)
                    return stateToCheck[i, i];
            }
        }
        c = 0;

        // check diagonal2
        int column = 0;
        for (int i = 2; i > 0; i--)
        {
            if (stateToCheck[i, column] != null)
            {
                if (stateToCheck[i, column] == stateToCheck[i - 1, column + 1])
                    c++;
                if (c == 2)
                    return stateToCheck[i, column];
                column++;
            }
        }

        // check draw
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == null)
                    return "";
            }
        }

        return "D";
    }
    [ContextMenu("Print")]
    public void PrintMatrix()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                print($"{i} {j} {board[i, j]}");
            }
        }
    }
}

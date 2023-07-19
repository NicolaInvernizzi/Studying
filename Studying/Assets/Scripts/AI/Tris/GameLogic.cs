using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class GameLogic : MonoBehaviour
{
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
    Coroutine ai = null;
    enum Round
    {
        A,
        B
    }

    void Start()
    {
        Restart();
    }
    private void Update()
    {
        print(ai == null);
    }
    public void Click()
    {
        ButtonPosition buttonScript = EventSystem.current.currentSelectedGameObject.gameObject.GetComponent<ButtonPosition>();
        buttonScript.SetSimbol(currentSimbol, false);
        board[buttonScript.raw, buttonScript.column] = currentSimbol;
        VerifyBoard();
    }
    public void Restart()
    {
        foreach(ButtonPosition b in buttons)
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
                break;
            case Round.B:
                currentRound = Round.A;
                currentSimbol = simbolA;
                roundText.text = $"{simbolA} is playing...";
                ai = StartCoroutine(AI());
                break;
        }
    }
    void VerifyBoard()
    {
        string status = Winner(board);

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

        else if (status == "Draw")
        {
            roundText.text = status;
            return;
        }

        else
            ChangeRound();
    }

    IEnumerator AI()
    {
        yield return new WaitForSeconds(1);
        int bestScore = -10;
        int[] bestMove = new int[2];

        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                if (board[i, j] == null)
                {
                    board[i, j] = currentSimbol;
                    int score = MinMax(board);
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
        ai = null;
    }

    // return state score
    int MinMax(string[,] state)
    {
        return 0;
    }

    string Winner(string[,] stateToCheck)
    {
        int c = 0;

        // check raw
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (stateToCheck[i, j] != null && stateToCheck[i, j] == stateToCheck[i, j + 1])
                    c++;
                if (c == 2)
                    return stateToCheck[i, j];
            }
            c = 0;
        }

        // check column
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (stateToCheck[j, i] != null && stateToCheck[j, i] == stateToCheck[j + 1, i])
                    c++;
                if (c == 2)
                    return stateToCheck[i, j];
            }
            c = 0;
        }

        // check diagonal1
        for (int i = 0; i < 2; i++)
        {
            if (stateToCheck[i, i] != null && stateToCheck[i, i] == stateToCheck[i + 1, i + 1])
                c++;
            if (c == 2)
                return stateToCheck[i, i];
        }

        c = 0;

        // check diagonal2
        int column = 0;
        for (int i = 2; i > 0; i--)
        {
            if (stateToCheck[i, column] != null && stateToCheck[i, column] == stateToCheck[i - 1, column + 1])
                c++;
            if (c == 2)
                return stateToCheck[i, i];
            column++;
        }

        // check draw
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == null)
                    return "Continue";
            }
        }

        return "Draw";
    }
}

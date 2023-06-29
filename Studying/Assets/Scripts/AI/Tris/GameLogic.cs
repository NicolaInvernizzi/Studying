using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class GameLogic : MonoBehaviour
{
    public static List<Button> buttonList = new List<Button>();
    public Text roundText;
    public string simbolA;
    public string simbolB; 
    Round currentRound;
    enum Round
    {
        A, 
        B
    }
    string currentSimbol;
    string[,] gameBoard = new string[3, 3];
    int winCounter = 0;
    bool gameOver;
    private enum Check
    {
        Raw,
        Column,
        DiagonalSx,
        DiagonalDx,
    }

    private void Start()
    {
        currentRound = (Round)Random.Range(0, 2);
        ChangeRound();
    }
    public void Click()
    {
        GameObject gameObject = EventSystem.current.currentSelectedGameObject.gameObject;
        gameObject.GetComponentInChildren<Text>().text = currentSimbol;
        ButtonPosition buttonScript = gameObject.GetComponent<ButtonPosition>();
        buttonScript.RemoveAndDisable();
        gameBoard[buttonScript.raw, buttonScript.column] = currentSimbol;
        GameOver();
    }
    void GameOver()
    {
        CheckRawColumn(Check.Raw);
        CheckRawColumn(Check.Column);
        CheckDiagonal(Check.DiagonalSx);
        CheckDiagonal(Check.DiagonalDx);

        if (!gameOver && buttonList.Count == 0)
        {
            roundText.text = "Draw";
            DisableGame();
        }

        if (!gameOver)
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
                break;
        }
    }
    void CheckRawColumn(Check check)
    {
        int dimension1 = 0;
        int dimension2 = 0;

        if (check == Check.Raw)
        {
            dimension1 = 0;
            dimension2 = 1;
        }
        else if (check == Check.Column)
        {
            dimension1 = 1;
            dimension2 = 0;
        }

        for (int value1 = 0; value1 < gameBoard.GetLength(dimension1); value1++)
        {
            for (int value2 = 0; value2 < gameBoard.GetLength(dimension2) - 1; value2++)
                CheckWin(check, value1, value2, 0);
            winCounter = 0;
        }
    }
    void CheckDiagonal(Check check)
    {
        int column = 0;
        int columnSign = 0;
        if (check == Check.DiagonalSx)
        {
            column = 0;
            columnSign = 1;
        }
        else if (check == Check.DiagonalDx)
        {
            column = 2;
            columnSign = -1;
        }

        for (int raw = 0; raw < 2; raw++)
        {
            CheckWin(check, raw, column, columnSign);
            column += columnSign;
        }
        winCounter = 0;
    }
    void CheckWin(Check check, int value1, int value2, int columnSign)
    {
        if (check == Check.Raw)
        {
            if (gameBoard[value1, value2] != null && gameBoard[value1, value2] == gameBoard[value1, value2 + 1])
                GameEnd(gameBoard[value1, value2]);
        }
        else if (check == Check.Column)
        {
            if (gameBoard[value2, value1] != null && gameBoard[value2, value1] == gameBoard[value2 + 1, value1])
                GameEnd(gameBoard[value2, value1]);
        }
        else if (check == Check.DiagonalSx || check == Check.DiagonalDx)
        {
            if (gameBoard[value1, value2] != null && gameBoard[value1, value2] == gameBoard[value1 + 1, value2 + columnSign])
                GameEnd(gameBoard[value1, value2]);
        }
    }
    void DisableGame()
    {
        buttonList.ForEach(b => b.GetComponent<ButtonPosition>().button.interactable = false);
        gameOver = true;
    }
    void GameEnd(string value)
    {
        winCounter++;
        if (winCounter == 2)
        {
            if (value == simbolA)
                roundText.text = $"{simbolA} wins";
            else
                roundText.text = $"{simbolB} wins";
            DisableGame();
        }
    }
}

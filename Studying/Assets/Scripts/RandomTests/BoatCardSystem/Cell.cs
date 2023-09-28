using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [HideInInspector]
    public BoatPanel boatPanel;

    private Cell prevCell;
    private Cell nextCell;
    private Coroutine delayCoroutine;
    private Image background;
    private CellState currentState;
    private enum CellState
    {
        Free, 
        Looked, 
        Occupied
    }
    private void Awake()
    {
        background = GetComponent<Image>();       
    }

    private void Update()
    {
        if (delayCoroutine != null)
        {
            background.color = Color.green;
            return;
        }
        background.color = Color.black;
    }
    public void SetLooked() => currentState = CellState.Looked;
    public void SetOccupied() => currentState = CellState.Occupied;
    public bool IsFree() => currentState == CellState.Free;
    public void SetPrevCell(Cell newPrev) => prevCell = newPrev;
    public void SetNextCell(Cell newNext) => nextCell = newNext;
    public bool InDely() => delayCoroutine != null;
    public void ClearCell()
    {
        if (nextCell == null)
        {
            if (boatPanel.HasOneLookingCard(this))
            {
                currentState = CellState.Free;
            }
            return;
        }
        if (nextCell.currentState != CellState.Occupied)
        {
            currentState = CellState.Free;
            return;
        }
        delayCoroutine = StartCoroutine(Delay());
    }
    public bool GetPrevCell(out Cell prevCell)
    {
        prevCell = null;

        if (this.prevCell == null)
        {
            return false;
        }
        if (this.prevCell.currentState == CellState.Free)
        {
            prevCell = this.prevCell;
            return true;
        }
        return false;
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(boatPanel.ShiftDelay);
        currentState = CellState.Free;
        delayCoroutine = null;
    }
}

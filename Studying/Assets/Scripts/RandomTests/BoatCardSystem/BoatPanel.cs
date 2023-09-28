using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoatPanel : MonoBehaviour
{
    public float ShiftDelay { get { return shiftDelay; } }    
    public float BoatCardSpeed { get { return boatCardSpeed; } }
    public float BoatCardDuration { get { return boatCardDuration; } }
    public List<BoatCard> BoatCards { get { return boatCards; } }

    [SerializeField]
    private GameObject boatCard;
    [SerializeField, Range(100.0f, 200.0f)]
    private float spawnOffset = 100.0f;
    [SerializeField, Range(0.0f, 10f)]
    private float shiftDelay = 0.1f;
    [SerializeField, Range(0.01f, 500.0f)]
    private float boatCardSpeed = 150.0f;
    [SerializeField, Range(5.0f, 300.0f)]
    private float boatCardDuration;

    public Transform CellsContainer { get; private set; }
    private Cell[] cells;
    private List<BoatCard> boatCards = new List<BoatCard>();
    private float randomTime;
    private float timer;

    private void Awake()
    {
        CellsContainer = transform.Find("CellsContainer");
        cells = CellsSetup();
        randomTime = UnityEngine.Random.Range(1.0f, 3.0f);
    }
    private void Update()
    {
        TestCycle();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnBoatCard();
        }
        for(int i = 0; i < boatCards.Count; i++)
        {
            boatCards[i].Behaviour();
        }
    }

    private void TestCycle()
    {
        timer += Time.deltaTime;
        if (timer >= randomTime)
        {
            randomTime = UnityEngine.Random.Range(1.0f, 3.0f);
            timer = 0.0f;
            SpawnBoatCard();
        }
    }
    public float GetOutY() => CellsContainer.position.y + spawnOffset;
    public BoatCard SpawnBoatCard()
    {
        Cell insertCell = FindInsertPoint();
        BoatCard boatCard = Instantiate(this.boatCard, new Vector3(insertCell.transform.position.x, GetOutY()), 
            Quaternion.identity, transform).GetComponent<BoatCard>();
        boatCard.boatPanel = this;
        boatCard.SetTargetCell(insertCell);
        boatCards.Add(boatCard);
        boatCard.duration = UnityEngine.Random.Range(10.0f, 20.0f);
        return boatCard;
    }
    public bool HasOneLookingCard(Cell cell)
    {
        return boatCards.Count(b => b.TargetCell == cell) == 1;
    }
    private Cell FindInsertPoint()
    {
        for (int i = cells.Length - 1; i >= 0; i--)
        {
            if (!cells[i].IsFree())
            {
                if (i == cells.Length - 1)
                {
                    Debug.LogWarning("There are no more free cells.");
                    return cells[i];
                }
                return cells[i + 1];
            }
        }
        return cells[0];
    }
    private Cell[] CellsSetup()
    {
        Cell[] cells = CellsContainer.GetComponentsInChildren<Cell>();

        for (int i = 0; i < cells.Length; i++)
        {
            if (i < cells.Length - 1)
            {
                cells[i].SetNextCell(cells[i + 1]);
            }
            if (i > 0)
            {
                cells[i].SetPrevCell(cells[i - 1]);
            }
            cells[i].boatPanel = this;
        }

        return cells;
    }
}

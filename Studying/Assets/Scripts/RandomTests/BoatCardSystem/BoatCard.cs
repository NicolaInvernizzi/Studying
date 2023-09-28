using UnityEngine;
using UnityEngine.UI;

public class BoatCard : MonoBehaviour
{
    [HideInInspector]
    public BoatPanel boatPanel;
    [HideInInspector]
    public float duration;
    public Cell TargetCell { get { return targetCell; } }   

    [SerializeField]
    private Slider waitingSlider;
    private Cell targetCell;
    private float timer;
    [SerializeField]
    private bool testExpire;

    private void Update()
    {
        if (testExpire)
        {
            Expire();
            testExpire = false;
        }
    }
    public void SetTargetCell(Cell cell)
    {
        targetCell = cell;
        targetCell?.SetLooked();
    }
    public void Behaviour()
    {
        if (targetCell == null)
        {
            Move(new Vector3(transform.position.x, boatPanel.GetOutY()), true);
            return;
        }
        if (targetCell.GetPrevCell(out Cell prevCell))
        {
            targetCell.ClearCell();
            SetTargetCell(prevCell);
        }
        Move(targetCell.transform.position, false);
        timer += Time.deltaTime;
        UpdateWaitingTime(timer / duration);
    }
    public void UpdateWaitingTime(float value)
    {
        waitingSlider.value = 1 - value;
        if (1 - value <= 0.0f)
        {
            Expire();
        }
    }
    public void Expire()
    {
        targetCell.ClearCell();
        SetTargetCell(null);
    }
    public void Move(Vector3 stopPosition, bool destroy)
    {
        if (destroy && Mathf.Abs(transform.position.magnitude - stopPosition.magnitude) <= 1.0f)
        {
            boatPanel.BoatCards.Remove(this);
            Destroy(gameObject);
            return;
        }
        if (transform.position == stopPosition)
        {
            targetCell.SetOccupied();
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, stopPosition, Time.deltaTime * boatPanel.BoatCardSpeed);
    }
}

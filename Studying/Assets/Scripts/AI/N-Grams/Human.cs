using UnityEngine.UI;

public class Human : Player
{
    Button rockButton;
    Button paperButton;
    Button scissorButton;
    protected sealed override void Awake()
    {
        base.Awake();
        rockButton = transform.Find("Organizer").Find("Rock").GetComponent<Button>();
        scissorButton = transform.Find("Organizer").Find("Scissor").GetComponent<Button>();
        paperButton = transform.Find("Organizer").Find("Paper").GetComponent<Button>();
    }
    void Update()
    {
        if (GameManager.instance.waitingForMoves)
        {
            if (isPlaying && move != "")
                CanPlay(false);
        }
        else
        {
            move = "";
            CanPlay(true);
        }
    }
    public void CanPlay(bool value)
    {
        rockButton.interactable = value;
        paperButton.interactable = value;
        scissorButton.interactable = value;
        isPlaying = value;
    }
    bool isPlaying = true;
    public void Rock()
    {
        move = GameManager.instance.possibleMoves[0];
        moveText.text = move;
    }
    public void Scissor()
    {
        move = GameManager.instance.possibleMoves[1];
        moveText.text = move;
    }
    public void Paper()
    {
        move = GameManager.instance.possibleMoves[2];
        moveText.text = move;
    } 
}

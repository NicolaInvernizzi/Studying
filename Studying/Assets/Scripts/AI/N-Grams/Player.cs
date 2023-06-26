using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public string playerName = "";
    public string move { get; protected set; } = "";
    Text scoreText;
    protected Text moveText;
    int score = 0;

    protected virtual void Awake()
    {
        scoreText = transform.Find("Score").GetComponentInChildren<Text>();
        moveText = transform.Find("Organizer").Find("Move").GetComponentInChildren<Text>();
    } 
    void Start() => transform.Find("Organizer").Find("Name").GetComponentInChildren<Text>().text = playerName;
    public virtual AI CheckAI() => null;
    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
}

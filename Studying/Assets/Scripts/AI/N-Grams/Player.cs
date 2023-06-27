using UnityEngine;
using UnityEngine.UI;

public abstract class Player : MonoBehaviour
{
    [Header("Player settings")]
    public string playerName;
    public string move { get; private set; } = "";
    public Text moveText, nameText, scoreText;
    int score = 0;

    protected virtual void Awake() => nameText.text = playerName;
    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
    public void SetMove(string move) => this.move = move;
    public void ShowMove() => moveText.text = move;
    public virtual AI CheckAI() => null;
    public virtual Human CheckHuman() => null;
    public virtual void Play() => moveText.text = "Deciding move...";
}

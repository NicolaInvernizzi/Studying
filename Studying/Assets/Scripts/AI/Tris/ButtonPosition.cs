using UnityEngine;
using UnityEngine.UI;

public class ButtonPosition : MonoBehaviour
{
    public int raw;
    public int column;
    [HideInInspector] public Button button;
    [HideInInspector] Text text;

    private void Awake()
    {
        button = transform.GetComponent<Button>();
        text = button.GetComponentInChildren<Text>();
    }
    private void Start()
    {
        GameLogic.buttons.Add(this);
    }
    public void Interactable(bool value)
    {
        button.interactable = value;
    }
    public void SetSimbol(string simbol, bool interactable)
    {
        text.text = simbol;
        Interactable(interactable);
    }
    public bool CheckRawColumn(int raw, int column)
    {
        return this.raw == raw && this.column == column;
    }
}

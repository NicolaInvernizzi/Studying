using UnityEngine;
using UnityEngine.UI;

public class ButtonPosition : MonoBehaviour
{
    public int raw;
    public int column;
    [HideInInspector] public Button button;

    private void Awake()
    {
        button = transform.GetComponent<Button>();
    }
    private void Start()
    {
        GameLogic.buttonList.Add(button);
    }
    public void RemoveAndDisable()
    {
        button.interactable = false;
        GameLogic.buttonList.Remove(button);
    }
}

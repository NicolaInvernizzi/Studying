using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine;

public class Human : Player
{
    [Space(10), Header("Human settings")]
    public Button[] buttons;
    public override void Play()
    {
        base.Play();
        buttons.All(i => i.interactable = true);
    }
    public void Button(string move)
    {
        base.SetMove(move);
        Array.ForEach(buttons, b => b.interactable = false);
        moveText.text = "Decided";
    }
    public sealed override Human CheckHuman() => this;
}

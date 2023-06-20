using UnityEngine;

public class KnockBackColor : MonoBehaviour
{
    [SerializeField] Color color;
    public Color GetColor()
    {
        if (color.a == 0)
        {
            Debug.LogWarning($"The color of {this.gameObject.name} ha alpha channel == 0");
            color.a = 255;
        }
        return color;
    }
}

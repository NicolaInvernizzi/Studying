using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float health = 1f;
    public void AddHealth()
    {
        health++;
    }
    public void DecreaseHealth()
    {
        health--;
    }

    private void Update()
    {
        transform.position += (transform.forward * Input.GetAxis("Vertical") + 
            transform.right * Input.GetAxis("Horizontal")) * 2 * Time.deltaTime;
    }
}

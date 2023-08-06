using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float health = 1f;

    public void HealthModifier(float quantity, bool increase)
    {
        if (increase)
            health += quantity;
        else 
            health -= quantity;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += (transform.forward * Input.GetAxis("Vertical") + 
            transform.right * Input.GetAxis("Horizontal")) * 2 * Time.deltaTime;
    }
}

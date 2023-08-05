using UnityEngine;

public class FruitRottenState : FruitBaseState
{
    float destroyTime = 10f;

    public override void EnterState(FruitStateManager fruitContext) { }
    public override void UpdateState(FruitStateManager fruitContext)
    {
        destroyTime -= Time.deltaTime;

        if (destroyTime <= 0f)
        {
            Object.Destroy(fruitContext.gameObject);
        }
    }
    public override void OnCollisionEnter(FruitStateManager fruitContext, Collision collision)
    {
        GameObject other = collision.gameObject;

        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().DecreaseHealth();
            fruitContext.SwitchState(FruitStateManager.States.Chewed, FruitStateManager.PrefabStates.RottenChewed);
        }
    }
}

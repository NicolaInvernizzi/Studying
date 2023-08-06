using UnityEngine;

public class FruitChewedState : FruitBaseState
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
    public override void CollisionEnter(FruitStateManager fruitContext) { }
}

using UnityEngine;

public abstract class FruitBaseState
{
    public abstract void EnterState(FruitStateManager fruitContext);
    public abstract void UpdateState(FruitStateManager fruitContext);
    public abstract void CollisionEnter(FruitStateManager fruitContext);
}

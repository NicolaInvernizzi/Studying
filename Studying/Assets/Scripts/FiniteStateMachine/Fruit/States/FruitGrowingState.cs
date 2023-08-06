using UnityEngine;

public class FruitGrowingState : FruitBaseState
{
    Vector3 startSize = new Vector3(0.1f, 0.1f, 0.1f);
    Vector3 growScaler = new Vector3(0.01f, 0.01f, 0.01f);
    Vector3 stopSize = new Vector3(0.25f, 0.25f, 0.25f);

    public override void EnterState(FruitStateManager fruitContext)
    {
        fruitContext.currentPrefabState.transform.localScale = startSize;
    }
    public override void UpdateState(FruitStateManager fruitContext)
    {
        if (fruitContext.currentPrefabState.transform.localScale.x < stopSize.x)
        {
            fruitContext.currentPrefabState.transform.localScale += growScaler * Time.deltaTime;
            return;
        }
        fruitContext.SwitchState(FruitStateManager.States.Whole);
    }
    public override void CollisionEnter(FruitStateManager fruitContext) { }
}

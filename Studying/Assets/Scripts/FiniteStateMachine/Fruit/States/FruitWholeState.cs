using UnityEngine;

public class FruitWholeState : FruitBaseState
{
    float rottenTime = 10f;

    public override void EnterState(FruitStateManager fruitContext)
    {
        Rigidbody rb = fruitContext.gameObject.AddComponent<Rigidbody>();
        rb.drag = 2f;
        rb.angularDrag = 2f;
        fruitContext.transform.SetParent(null);
    }
    public override void UpdateState(FruitStateManager fruitContext)
    {
        rottenTime -= Time.deltaTime;
        if (rottenTime <= 0f)
        {
            fruitContext.SwitchState(FruitStateManager.States.Rotten, FruitStateManager.PrefabStates.Rotten);
        }
    }
    public override void CollisionEnter(FruitStateManager fruitContext) 
    {
        if (fruitContext.CollisionDetection())
        {
            fruitContext.player.GetComponent<PlayerController>().HealthModifier(1f, true);
            fruitContext.SwitchState(FruitStateManager.States.Chewed, FruitStateManager.PrefabStates.WholeChewed);
        }
    }
}

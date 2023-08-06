using UnityEngine;

public class FruitRottenState : FruitBaseState
{
    float destroyTime = 10f;
    Vector3 startSize = new Vector3(0.25f, 0.25f, 0.25f);
    Vector3 rottenScaler = new Vector3(0.01f, 0.01f, 0.01f);
    Vector3 stopSize = new Vector3(0.1f, 0.1f, 0.1f);
    public override void EnterState(FruitStateManager fruitContext)
    {
        fruitContext.currentPrefabState.transform.localScale = startSize;
    }
    public override void UpdateState(FruitStateManager fruitContext)
    {
        if (fruitContext.currentPrefabState.transform.localScale.x > stopSize.x)
        {
            fruitContext.currentPrefabState.transform.localScale -= rottenScaler * Time.deltaTime;
            return;
        }

        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0f)
        {
            Object.Destroy(fruitContext.gameObject);
        }
    }

    public override void CollisionEnter(FruitStateManager fruitContext)
    { 
        if (fruitContext.CollisionDetection())
        {
            fruitContext.player.GetComponent<PlayerController>().HealthModifier(1f, false);
            fruitContext.SwitchState(FruitStateManager.States.Chewed, FruitStateManager.PrefabStates.RottenChewed);
        }
    }
}

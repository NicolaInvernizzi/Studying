using UnityEngine;

public class FruitWholeState : FruitBaseState
{
    float rottenTime = 50f;

    public override void EnterState(FruitStateManager fruitContext)
    {
        fruitContext.GetComponent<Rigidbody>().useGravity = true;
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
    public override void OnCollisionEnter(FruitStateManager fruitContext, Collision collision)
    {
        GameObject other = collision.gameObject;
        Debug.Log(other.name);
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().AddHealth();
            fruitContext.SwitchState(FruitStateManager.States.Chewed, FruitStateManager.PrefabStates.WholeChewed);
        }
    }
}

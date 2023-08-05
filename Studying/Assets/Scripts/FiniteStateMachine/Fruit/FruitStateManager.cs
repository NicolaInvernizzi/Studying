using System;
using System.Collections.Generic;
using UnityEngine;

public class FruitStateManager : MonoBehaviour
{
    [SerializeField]
    List<StatePrefabKeyValue> statePrefab = new List<StatePrefabKeyValue>();
    FruitBaseState currentState;
    FruitGrowingState growingState = new FruitGrowingState();
    FruitWholeState wholeState = new FruitWholeState();
    FruitRottenState rottenState = new FruitRottenState();
    FruitChewedState chewedState = new FruitChewedState();
    Dictionary<States, FruitBaseState> states = new Dictionary<States, FruitBaseState>();
    [HideInInspector] public GameObject currentPrefabState = null;

    public enum States
    {
        Growing, 
        Whole, 
        Rotten, 
        Chewed
    }
    public enum PrefabStates
    {
        None,
        Growing,
        Rotten,
        RottenChewed,
        WholeChewed
    }

    private void Awake()
    {
        states.Add(States.Growing, growingState);
        states.Add(States.Whole, wholeState);
        states.Add(States.Rotten, rottenState);
        states.Add(States.Chewed, chewedState);
    }
    private void Start()
    {
        SwitchState(States.Growing, PrefabStates.Growing);
    }
    private void Update()
    {
        currentState.UpdateState(this);
    }
    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }
    public void SwitchState(States state, PrefabStates prefabState = PrefabStates.None)
    {
        if (prefabState != PrefabStates.None)
        {
            foreach (StatePrefabKeyValue sp in statePrefab)
            {
                GameObject newPrefab = sp.ActivatePrefab(prefabState);
                if (newPrefab != null)
                {
                    if (currentPrefabState != null)
                        currentPrefabState.SetActive(false);

                    currentPrefabState = newPrefab;
                    break;
                }
            }
        }

        currentState = states[state];
        currentState.EnterState(this);
    }

    [Serializable]
    public class StatePrefabKeyValue
    {
        [SerializeField] PrefabStates state;
        [SerializeField] GameObject prefab;

        public GameObject ActivatePrefab(PrefabStates toFind)
        {
            if (state == toFind)
            {
                prefab.SetActive(true);
                return prefab;
            }
            return null;
        }
    }
}

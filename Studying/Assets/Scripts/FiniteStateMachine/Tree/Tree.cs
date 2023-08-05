using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField]
    float spawnTime = 10f;
    [SerializeField]
    FruitStateManager fruit;

    SpawnPoint[] spawnPoints;
    Coroutine spawnCoroutine = null;

    private void Start()
    {
        spawnPoints = transform.GetComponentsInChildren<SpawnPoint>();
    }

    private void Update()
    {
        if (spawnCoroutine == null && Array.Exists(spawnPoints, s => s.transform.childCount == 0))
        {
            spawnCoroutine = StartCoroutine(Decision());
        }
    }

    IEnumerator Decision()
    {
        yield return new WaitForSeconds(spawnTime);
        SpawnPoint[] freeSpawnPoints = spawnPoints.Where(s => s.transform.childCount == 0).ToArray();
        freeSpawnPoints[UnityEngine.Random.Range(0, freeSpawnPoints.Length)].CreateFruit(fruit.transform);
        spawnCoroutine = null;
    }
}

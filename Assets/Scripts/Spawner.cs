using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _secondsBetweenSpawns = 5.0f;
    
    private Boundary _spawnBoundary = null;
    private float _additionalYHeight = 0f;
    // Start is called before the first frame update
    void Start()
    {
        _spawnBoundary = GetComponentInChildren<Boundary>();
        GameManager.OnStartGame += StartSpawning;
        GameManager.OnEndGame += StopSpawning;
        GameManager.OnAddToStack += AddToHeight;
    }

    private void StartSpawning() {
        _additionalYHeight = 0;
        StartCoroutine(nameof(SpawnItems));
    }

    private void StopSpawning() {
        StopAllCoroutines();
    }

    private void AddToHeight(StackableItem stackableItem) {
        _additionalYHeight += stackableItem.Height;
    }

    IEnumerator SpawnItems() {
        while (true) {
            PoolId idToSpawn = Random.Range(0, 2) == 1 ? PoolId.Pizza : PoolId.Bomb;
            Vector3 spawnPos = _spawnBoundary.RandomPosInBound;
            spawnPos.y += _additionalYHeight;
            ObjectPooler.instance.SpawnFromPool(idToSpawn, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(_secondsBetweenSpawns);
        }
    }
}

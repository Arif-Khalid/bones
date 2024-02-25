using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _initialSecondsBetweenSpawns = 5.0f;
    [SerializeField] private float _initialBombChance = 0.1f;
    [SerializeField] private float _maxBombChance = 0.5f;
    [SerializeField] private float _minSecondsBetweenSpawns = 1.0f;
    [SerializeField] private float _bombChanceIncreaseVal = 0.01f;
    [SerializeField] private float _secondsDecreaseVal = 0.01f;
    
    private float _secondsBetweenSpawns = 0f;
    private float _bombChance = 0f;
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
        _secondsBetweenSpawns = _initialSecondsBetweenSpawns;
        _bombChance = _initialBombChance;
        StartCoroutine(nameof(SpawnItems));
        StartCoroutine(nameof(RampDifficulty));
    }

    private void StopSpawning() {
        StopAllCoroutines();
    }

    private void AddToHeight(StackableItem stackableItem) {
        _additionalYHeight += stackableItem.Height;
    }

    IEnumerator SpawnItems() {
        while (true) {
            PoolId idToSpawn = Random.Range(0f, 1f) < _bombChance ? PoolId.Bomb : PoolId.Pizza;
            Vector3 spawnPos = _spawnBoundary.RandomPosInBound;
            spawnPos.y += _additionalYHeight;
            GameObject spawnedObject = ObjectPooler.instance.SpawnFromPool(idToSpawn, spawnPos, Quaternion.Euler(Vector3.zero));
            yield return new WaitForSeconds(_secondsBetweenSpawns);
        }
    }

    IEnumerator RampDifficulty() {
        while (true) {
            _bombChance = Mathf.Min(_bombChance + _bombChanceIncreaseVal * Time.deltaTime, _maxBombChance);
            _secondsBetweenSpawns = Mathf.Max(_secondsBetweenSpawns - _secondsDecreaseVal * Time.deltaTime, _minSecondsBetweenSpawns);
            yield return null;
        }
    }

}

using System.Collections;
using UnityEngine;

/**
 * Responsible for spawning falling items
 * Including difficulty progression as time passes
 */
public class Spawner : MonoBehaviour
{
    [SerializeField] private float _initialSecondsBetweenSpawns = 5.0f;     // The starting amount of seconds between item spawns
    [SerializeField] private float _initialBombChance = 0.1f;               // The starting chance to spawn a bomb when spawning an item
    [SerializeField] private float _maxBombChance = 0.5f;
    [SerializeField] private float _minSecondsBetweenSpawns = 1.0f;
    [SerializeField] private float _bombChanceIncreaseVal = 0.01f;          // Speed of bomb chance increasing as time passes
    [SerializeField] private float _secondsDecreaseVal = 0.01f;             // Speed of seconds between spawns decreasing as time passes

    private float _secondsBetweenSpawns = 0f;                               // The current seconds between spawns
    private float _bombChance = 0f;                                         // The current bomb chance from 0-1
    private float _additionalYHeight = 0f;                                  // The additional height to spawn above boundary height

    private Boundary _spawnBoundary = null;
    // Start is called before the first frame update
    void Start() {
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

    private void OnDestroy() {
        GameManager.OnStartGame -= StartSpawning;
        GameManager.OnEndGame -= StopSpawning;
        GameManager.OnAddToStack -= AddToHeight;
    }

}

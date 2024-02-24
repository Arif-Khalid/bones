using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static UnityAction<StackableItem> OnAddToStack;
    public static UnityAction OnStartGame;
    public static UnityAction OnEndGame;

    [SerializeField] private float _secondsBetweenSpawns = 5.0f;
    private static bool _isGameStarted = false;

    private Boundary _spawnBoundary = null;
    private void Start() {
        _spawnBoundary = GetComponentInChildren<Boundary>();
        OnStartGame += StartGame;
        OnEndGame += EndGame;
    }

    private void EndGame() {
        _isGameStarted = false;
    }

    private void StartGame() {
        ObjectPooler.instance.ResetPools();
        _isGameStarted = true;
        StartCoroutine(nameof(SpawnItems));
    }

    IEnumerator SpawnItems() {
        while (_isGameStarted) {
            PoolId idToSpawn = Random.Range(0, 2) == 1 ? PoolId.Pizza : PoolId.Bomb;
            ObjectPooler.instance.SpawnFromPool(idToSpawn, _spawnBoundary.RandomPosInBound, Quaternion.identity);
            yield return new WaitForSeconds(_secondsBetweenSpawns);
        }
    }

    public static void TriggerOnAddToStack(StackableItem stackableItem) {
        if(OnAddToStack != null) {
            OnAddToStack(stackableItem);
        }
    }

    public static void TriggerOnStartGame() {
        if(OnStartGame != null && !_isGameStarted) {
            OnStartGame();
        }
    }

    public static void TriggerOnEndGame() {
        if(OnEndGame != null && _isGameStarted) {
            OnEndGame();
        }
    }
}

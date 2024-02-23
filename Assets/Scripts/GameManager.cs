using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static UnityAction<StackableItem> OnAddToStack;

    [SerializeField] private GameObject _pizzaPrefab = null;
    [SerializeField] private float _secondsBetweenSpawns = 5.0f;

    private Boundary _spawnBoundary = null;
    private void Start() {
        _spawnBoundary = GetComponentInChildren<Boundary>();
        StartCoroutine(nameof(SpawnItems));
    }

    IEnumerator SpawnItems() {
        while (true) {
            GameObject pizza = Instantiate(_pizzaPrefab);
            pizza.transform.position = _spawnBoundary.RandomPosInBound;
            yield return new WaitForSeconds(_secondsBetweenSpawns);
        }
    }
}

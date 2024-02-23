using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static UnityAction<FallingItem> OnAddToStack;

    [SerializeField] private GameObject _pizzaPrefab = null;
    [SerializeField] private float _secondsBetweenSpawns = 5.0f;
    [SerializeField] private float _fadeSpeed = 1.0f;
    private Queue<GameObject> _objectsToFade = new();
    private Boundary _spawnBoundary = null;
    private void Start() {
        _spawnBoundary = GetComponentInChildren<Boundary>();
        StartCoroutine(nameof(SpawnItems));
    }

    private void Update() {
        FadeObjects();
    }

    private void FadeObjects() {
        int size = _objectsToFade.Count;
        while(size > 0) {
            GameObject obj = _objectsToFade.Dequeue();
            float currentOpacity = obj.GetComponent<Renderer>().material.GetFloat("_Opacity");
            float newOpacity = currentOpacity - _fadeSpeed * Time.deltaTime;
            if (newOpacity > 0) {
                obj.GetComponent<Renderer>().material.SetFloat("_Opacity", newOpacity);
                _objectsToFade.Enqueue(obj);
            }
            else {
                Destroy(obj);
            }
            size -= 1;
        }
    }

    private void OnFallingItemCollide(Collision collision, GameObject fallingItemGameObject) {
        if (collision.gameObject.tag == "StackTop") {
            OnAddToStack(fallingItemGameObject.GetComponent<FallingItem>());
            return;
        }
        _objectsToFade.Enqueue(fallingItemGameObject);
    }

    IEnumerator SpawnItems() {
        while (true) {
            GameObject pizza = Instantiate(_pizzaPrefab);
            pizza.transform.position = _spawnBoundary.RandomPosInBound;
            pizza.GetComponent<FallingItem>().OnCollide = OnFallingItemCollide;
            yield return new WaitForSeconds(_secondsBetweenSpawns);
        }
    }
}

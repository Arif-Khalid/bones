using System.Collections.Generic;
using UnityEngine;

/**
 * Responsible for registering and handling behaviours of multiple scripts
 * that require multi-frame behaviour
 */
public class RunService : MonoBehaviour
{
    public static RunService instance;
    private Queue<IRunServiceable> _runServiceQueue = new();

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }

    private void Start() {
        GameManager.OnStartGame += Reset;
    }

    // Run all the registered scripts
    private void Update() {
        int size = _runServiceQueue.Count;
        while (size > 0) {
            IRunServiceable runServiceable = _runServiceQueue.Dequeue();
            // Remove script that has completed its behaviour
            if (!runServiceable.Run()) {
                _runServiceQueue.Enqueue(runServiceable);
            }
            size -= 1;
        }
    }

    private void Reset() {
        _runServiceQueue.Clear();
    }

    public void AddRunServiceable(IRunServiceable runServiceable) {
        _runServiceQueue.Enqueue(runServiceable);
    }

    private void OnDestroy() {
        GameManager.OnStartGame -= Reset;
    }
}

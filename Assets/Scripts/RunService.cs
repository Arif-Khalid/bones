using System.Collections.Generic;
using UnityEngine;

public class RunService : MonoBehaviour
{
    public static RunService instance;
    private Queue<IRunServiceable> _runServiceQueue = new();

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }

    private void Start() {
        GameManager.OnStartGame += Reset;
    }

    private void Update() {
        int size = _runServiceQueue.Count;
        while(size > 0) {
            IRunServiceable runServiceable = _runServiceQueue.Dequeue();
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
}

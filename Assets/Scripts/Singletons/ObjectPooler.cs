using System.Collections.Generic;
using UnityEngine;

// Represents an item that can be spawned from the pool
public enum PoolId
{
    Pizza,
    Bomb,
    Explosion,
    StackableItemUI,
}

/**
 * Responsible for spawning objects efficiently
 * Alternative to Instantiate and Destroy
 */
public class ObjectPooler : MonoBehaviour
{
    // Represents the pool from which items are spawned
    [System.Serializable]
    public class Pool
    {
        public PoolId Id;
        public GameObject Prefab;
        public int size;
    }

    [SerializeField] private PoolsScriptableObject _poolsScriptableObject;                                                  // Contains the pools to draw from
    private Dictionary<PoolId, Queue<GameObject>> _pools = new Dictionary<PoolId, Queue<GameObject>>();
    // Keep track of removed objects is necessary as some objects cannot be re-used until explicitly reset
    private Dictionary<PoolId, HashSet<GameObject>> _removedObjectsPools = new Dictionary<PoolId, HashSet<GameObject>>();

    public static ObjectPooler instance;
    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        }
        instance = this;

        // Create an object pool for each pool stored in scriptable object
        foreach (Pool pool in _poolsScriptableObject.Pools) {
            if (_pools.ContainsKey(pool.Id)) {
                Debug.LogWarning("There are multiple existence of the pool id " + pool.Id + " in pool scriptable object");
                continue;
            }
            _pools.Add(pool.Id, new Queue<GameObject>());
            _removedObjectsPools.Add(pool.Id, new HashSet<GameObject>());
            for (int i = 0; i < pool.size; i++) {
                GameObject spawnedObject = Instantiate(pool.Prefab, Vector3.zero, Quaternion.identity);
                spawnedObject.SetActive(false);
                _pools[pool.Id].Enqueue(spawnedObject);
            }
        }
    }

    public GameObject SpawnFromPool(PoolId poolId, Vector3 position, Quaternion rotation) {
        if (!_pools.ContainsKey(poolId) || _pools[poolId].Count == 0) {
            Debug.LogWarning("Pool with the id " + poolId + " does not exist");
            return null;
        }
        GameObject spawnedObject = _pools[poolId].Dequeue();
        while (_pools[poolId].Count > 0 && _removedObjectsPools[poolId].Contains(spawnedObject)) {
            spawnedObject = _pools[poolId].Dequeue();
        }
        if (_removedObjectsPools[poolId].Contains(spawnedObject)) {
            Debug.LogWarning("Pool with the id " + poolId + " has run out of items");
            return null;
        }
        spawnedObject.SetActive(true);
        IPooledObject pooledObject = spawnedObject.GetComponent<IPooledObject>();
        if (pooledObject != null) {
            pooledObject.OnObjectSpawn();
        }
        spawnedObject.transform.position = position;
        spawnedObject.transform.rotation = rotation;

        _pools[poolId].Enqueue(spawnedObject);
        return spawnedObject;
    }

    // Stops the object from being re-used by pooler until reset
    public void RemoveFromPool(PoolId poolId, GameObject gameObjectToRemove) {
        _removedObjectsPools[poolId].Add(gameObjectToRemove);
    }

    public void ResetPools() {
        foreach ((PoolId poolId, Queue<GameObject> queue) in _pools) {
            int size = queue.Count;
            for (int i = 0; i < size; ++i) {
                GameObject gameObject = queue.Dequeue();
                gameObject.SetActive(false);
                gameObject.transform.SetParent(null, false);
                queue.Enqueue(gameObject);
            }
            HashSet<GameObject> removedObjects = _removedObjectsPools[poolId];
            foreach (GameObject removedObject in removedObjects) {
                queue.Enqueue(removedObject);
                removedObject.transform.SetParent(null, false);
                removedObject.SetActive(false);
            }
        }
        _removedObjectsPools.Clear();
        foreach (PoolId poolId in _pools.Keys) {
            _removedObjectsPools.Add(poolId, new HashSet<GameObject>());
        }
    }
}

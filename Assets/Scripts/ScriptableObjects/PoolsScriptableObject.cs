using System.Collections.Generic;
using UnityEngine;

/**
 * Responsible for storing pools
 * Pools are used in ObjectPooler
 */
[CreateAssetMenu(fileName = "Pools", menuName = "PoolsScriptableObject", order = 1)]
public class PoolsScriptableObject : ScriptableObject
{
    public List<ObjectPooler.Pool> Pools = new List<ObjectPooler.Pool>();
}

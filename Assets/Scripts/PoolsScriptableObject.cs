using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pools", menuName = "PoolsScriptableObject", order = 1)]
public class PoolsScriptableObject : ScriptableObject
{
    public List<ObjectPooler.Pool> Pools = new List<ObjectPooler.Pool>();
}

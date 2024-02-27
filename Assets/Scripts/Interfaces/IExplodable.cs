using UnityEngine;

/**
 * Implemented by objects that can be exploded
 */
public interface IExplodable
{
    // Explosion force already accounts for distance from explosion
    abstract void OnExplode(Vector3 explosionForce);
}

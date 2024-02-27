using UnityEngine;

/**
 * Responsible for handling collision detection
 * Only triggers overridable function on first collision
 */
public abstract class CollisionDetector : MonoBehaviour
{
    private bool _hasCollided = false;
    private void OnCollisionEnter(Collision collision) {
        if (_hasCollided) {
            return;
        }
        OnFirstCollision(collision);
        _hasCollided = true;
    }

    protected abstract void OnFirstCollision(Collision collision);

    protected void ResetCollisionStatus() {
        _hasCollided = false;
    }
}

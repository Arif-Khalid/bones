using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Responsible for logic of player kicking bombs
 */
[RequireComponent(typeof(PlayerInput))]
public class PlayerKick : MonoBehaviour
{
    [SerializeField] private Transform _kickCenterTransform;        // The center of the kick from which the bounds of the kick size stem
    [SerializeField] private float _kickSize = 1.0f;                // The length of one side of the cuboid area where the player kick occurs 
    [SerializeField] private float _pushForce = 1.0f;               // The amount of force extruded by the kick

    // Called by player input component
    private void OnKick() {
        Collider[] colliders = Physics.OverlapBox(_kickCenterTransform.position,
            new Vector3(_kickSize / 2, _kickSize / 2, _kickSize / 2));
        bool hasKickedBomb = false;
        for (int i = 0; i < colliders.Length; i++) {
            Rigidbody rigidbody = colliders[i].GetComponent<Rigidbody>();
            if (rigidbody != null) {
                Vector3 direction = Vector3.Normalize(colliders[i].transform.position - transform.position);
                rigidbody.AddForce(direction * _pushForce, ForceMode.Impulse);
            }
            if (!hasKickedBomb && colliders[i].GetComponent<Bomb>()) {
                hasKickedBomb = true;
            }
        }
        if (hasKickedBomb) {
            AudioManager.instance.Play(AudioID.BombKick);
        }
    }

    // Helper function to visualise kick area
    private void OnDrawGizmosSelected() {
        if (_kickCenterTransform == null) {
            Debug.LogWarning("Kick center transform not set in PlayerCollision");
            return;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_kickCenterTransform.position, new Vector3(_kickSize, _kickSize, _kickSize));
    }
}

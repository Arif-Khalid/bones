using UnityEngine;

public class PlayerKick : MonoBehaviour
{
    [SerializeField] private Transform _kickCenterTransform;
    [SerializeField] private float _kickSize = 1.0f;
    [SerializeField] private float _pushForce = 1.0f;
    private void OnKick() {
        Collider[] colliders = Physics.OverlapBox(_kickCenterTransform.position,
            new Vector3(_kickSize / 2, _kickSize / 2, _kickSize / 2));

        for (int i = 0; i < colliders.Length; i++) {
            Rigidbody rigidbody = colliders[i].GetComponent<Rigidbody>();
            if (rigidbody != null) {
                Vector3 direction = Vector3.Normalize(colliders[i].transform.position - transform.position);
                rigidbody.AddForce(direction * _pushForce, ForceMode.Impulse);
            }
        }
    }

    private void OnDrawGizmosSelected() {
        if (_kickCenterTransform == null) {
            Debug.LogWarning("Kick center transform not set in PlayerCollision");
            return;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_kickCenterTransform.position, new Vector3(_kickSize, _kickSize, _kickSize));
    }
}

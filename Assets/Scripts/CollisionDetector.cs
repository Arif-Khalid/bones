using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollisionDetector : MonoBehaviour
{
    public delegate void CollisionDelegate(Collision collision, GameObject detectorGameObject);
    public CollisionDelegate OnCollide;

    private bool _hasCollided = false;

    protected virtual void OnCollisionEnter(Collision collision) {
        if (OnCollide != null && !_hasCollided) {
            OnCollide(collision, gameObject);  
        }
        _hasCollided = true;
    }

    public void ResetCollisionStatus() {
        _hasCollided = false;
    }
}

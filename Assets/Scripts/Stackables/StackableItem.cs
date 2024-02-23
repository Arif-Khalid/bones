using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class StackableItem : MonoBehaviour
{
    private bool _hasCollided = false;
    // This is necessary because getting bounds during motion is inaccurate
    [HideInInspector] public float Height = 0;
    void Start() {
        Height = GetComponent<Collider>().bounds.size.y;
    }

    
    private void OnCollisionEnter(Collision collision) {
        if (_hasCollided) {
            return;
        }
        _hasCollided = true;
        if(collision.gameObject.tag == "StackTop") {
            GameManager.OnAddToStack(this);
        }
        else {
            Fader.instance.AddObject(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Renderer))]
public abstract class StackableItem : MonoBehaviour, IRunServiceable
{
    [SerializeField] private float _fadeSpeed = 1.0f;
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
            RunService.instance.AddRunServiceable(this);
        }
    }

    public bool Run() {
        Renderer renderer = GetComponent<Renderer>();
        float currentOpacity = renderer.material.GetFloat("_Opacity");
        float newOpacity = currentOpacity - _fadeSpeed * Time.deltaTime;
        if (newOpacity > 0) {
            renderer.material.SetFloat("_Opacity", newOpacity);
            return false;
        }
        else {
            Destroy(gameObject);
            return true;
        }
    }
}

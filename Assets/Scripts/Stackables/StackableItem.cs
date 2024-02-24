using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Renderer))]
public abstract class StackableItem : MonoBehaviour, IRunServiceable, IExplodable, IPooledObject
{
    [SerializeField] private float _fadeSpeed = 1.0f;
    public PoolId PrefabPoolId;
    [HideInInspector] public Tray tray = null;
    private bool _hasCollided = false;
    // This is necessary because getting bounds during motion is inaccurate
    [HideInInspector] public float Height = 0;
    protected Renderer _renderer = null;
    void Awake() {
        Height = GetComponent<Collider>().bounds.size.y;
        _renderer = GetComponent<Renderer>();
    }


    private void OnCollisionEnter(Collision collision) {
        if (_hasCollided) {
            return;
        }
        _hasCollided = true;
        if (collision.gameObject.tag == "StackTop") {
            GameManager.TriggerOnAddToStack(this);
        }
        else {
            RunService.instance.AddRunServiceable(this);
        }
    }

    public virtual bool Run() {
        float currentOpacity = _renderer.material.GetFloat("_Opacity");
        float newOpacity = currentOpacity - _fadeSpeed * Time.deltaTime;
        if (newOpacity > 0) {
            _renderer.material.SetFloat("_Opacity", newOpacity);
            return false;
        }
        else {
            gameObject.SetActive(false);
            return true;
        }
    }

    public virtual void OnExplode(Vector3 explosionForce) {
        if (tray != null) {
            tray.RagdollStack(); // Adds rigidbody to entire stack
        }
        GetComponent<Rigidbody>().AddForce(explosionForce, ForceMode.Impulse);
    }

    public virtual void OnObjectSpawn() {
        _renderer.material.SetFloat("_Opacity", 1);
        _hasCollided = false;
    }
}

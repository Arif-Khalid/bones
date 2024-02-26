using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Renderer))]
public abstract class StackableItem : FallingItem, IRunServiceable, IExplodable
{
    [SerializeField] private float _fadeSpeed = 1.0f;
    public PoolId PrefabPoolId;
    [HideInInspector] public Tray tray = null;
    // This is necessary because getting bounds during motion is inaccurate
    [HideInInspector] public float Height = 0;
    protected Renderer _renderer = null;
    private Rigidbody _rigidbody = null;
    protected override void Awake() {
        base.Awake();
        Height = GetComponent<Collider>().bounds.size.y;
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }


    protected override void OnFirstCollision(Collision collision) {
        base.OnFirstCollision(collision);
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
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(explosionForce, ForceMode.Impulse);
    }

    public override void OnObjectSpawn() {
        base.OnObjectSpawn();
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _renderer.material.SetFloat("_Opacity", 1);
    }
}

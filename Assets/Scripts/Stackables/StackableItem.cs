using UnityEngine;

/**
 * Responsible for handling logic of a stackable item
 */
[RequireComponent(typeof(Collider))]
public abstract class StackableItem : FallingItem, IRunServiceable, IExplodable
{
    public PoolId PrefabPoolId;                                 // The pool ID of this stackable item
    [SerializeField] private float _fadeSpeed = 1.0f;           // How fast the item fades upon hitting the ground
    [HideInInspector] public Tray tray = null;                  // Reference to the tray this item is stacked on

    [HideInInspector] public float Height = 0;                  // height of this item, necessary as getting height while item is falling is inaccurate
    protected Renderer[] _renderers = null;                     // The renderers that make up this item. All renderers should have _Opacity attribute
    private Rigidbody _rigidbody = null;
    protected override void Awake() {
        base.Awake();
        Height = GetComponent<Collider>().bounds.size.y;
        _renderers = GetComponentsInChildren<Renderer>();
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

    // Fade the item, handled by run service
    public virtual bool Run() {
        float currentOpacity = _renderers[0].material.GetFloat("_Opacity");
        float newOpacity = currentOpacity - _fadeSpeed * Time.deltaTime;
        foreach (Renderer renderer in _renderers) {
            if (newOpacity > 0) {
                renderer.material.SetFloat("_Opacity", newOpacity);
            }
        }
        if (newOpacity <= 0) {
            gameObject.SetActive(false);
            return true;
        }
        return false;
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
        foreach (Renderer renderer in _renderers) {
            renderer.material.SetFloat("_Opacity", 1);
        }
    }
}

using UnityEngine;

/**
 * Responsible for handling Bomb item logic
 */
public class Bomb : FallingItem, IRunServiceable
{
    // Customization functions
    [SerializeField] private LayerMask _explodingObjects;       // The objects that can be exploded
    [SerializeField] private float _explosionRadius = 5.0f;
    [SerializeField] private float _explosionForce = 10.0f;
    [SerializeField] private float _timeBeforeExplode = 10.0f;  // Seconds before exploding after spawning
    [SerializeField] private float _timeForColorChange = 2.0f;  // Seconds till the first color change
    [SerializeField] private float _percentageSpeedUp = 0.9f;   // Percentage faster the bomb beeps after each period
    [SerializeField] private Color _redColor = Color.red;       // The second color the bomb lerps to while beeping
    [SerializeField] private Color _blackColor = Color.black;   // The first color the bomb lerps to while beeping

    // Reference to the beep audio source
    [SerializeField] private AudioSource _beepAudioSource = null;

    private bool _turningRed = true;
    private float _currentTimeForColorChange = 2.0f;
    private float _currentExplodeTime = 0;
    private float _currentColorChangeTime = 0;

    private Renderer _renderer;
    protected override void Awake() {
        base.Awake();
        _renderer = GetComponentInChildren<Renderer>();
    }

    public override void OnObjectSpawn() {
        base.OnObjectSpawn();
        _currentTimeForColorChange = _timeForColorChange;
        _currentExplodeTime = 0;
        _currentColorChangeTime = 0;
        _turningRed = true;
        RunService.instance.AddRunServiceable(this);
    }

    // Control beeping behaviour through RunService
    public bool Run() {
        _currentExplodeTime += Time.deltaTime;
        _currentColorChangeTime += Time.deltaTime;
        if (_currentExplodeTime > _timeBeforeExplode) {
            Explode();
            return true;
        }
        if (_currentColorChangeTime > _currentTimeForColorChange) {
            _currentTimeForColorChange = _percentageSpeedUp * _currentTimeForColorChange;
            if (_turningRed) {
                _beepAudioSource.Play();
            }
            _turningRed = !_turningRed;
            _currentColorChangeTime = 0;
        }
        Color baseColor = _turningRed ? _blackColor : _redColor;
        Color destColor = _turningRed ? _redColor : _blackColor;
        _renderer.material.color = Color.Lerp(baseColor, destColor, _currentColorChangeTime / _currentTimeForColorChange);
        return false;
    }

    private void Explode() {
        ObjectPooler.instance.SpawnFromPool(PoolId.Explosion, transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius, _explodingObjects);
        for (int i = 0; i < colliders.Length; i++) {
            IExplodable explodable = colliders[i].GetComponent<IExplodable>();
            if (explodable != null) {
                Vector3 direction = Vector3.Normalize(colliders[i].transform.position - transform.position);
                float forceMagnitude = Vector3.Distance(transform.position, colliders[i].transform.position) / _explosionRadius * _explosionForce;
                Vector3 currentExplosionForce = direction * forceMagnitude;
                explodable.OnExplode(currentExplosionForce);
            }
        }
        gameObject.SetActive(false);
    }

    // Helper function to visualise explosion radius in editor
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}

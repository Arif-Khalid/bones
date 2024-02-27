using UnityEngine;

/**
 * Responsible for playing customizable audio clips while an object falling and landing
 * Object should be spawned in a suspended state
 */
public abstract class FallingItem : CollisionDetector, IPooledObject
{
    [SerializeField] private AudioClip _fallingAudioClip;
    [SerializeField] private AudioClip _landingAudioClip;

    [Tooltip("Set only if you have another audio source you are using elsewhere on this object")]
    [SerializeField] private AudioSource _fallingAudioSource = null;
    protected virtual void Awake() {
        if (_fallingAudioSource == null) {
            _fallingAudioSource = GetComponent<AudioSource>();
            if (_fallingAudioSource == null) {
                _fallingAudioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    protected override void OnFirstCollision(Collision collision) {
        _fallingAudioSource.clip = _landingAudioClip;
        _fallingAudioSource.loop = false;
        _fallingAudioSource.Play();
    }

    public virtual void OnObjectSpawn() {
        ResetCollisionStatus();
        _fallingAudioSource.clip = _fallingAudioClip;
        _fallingAudioSource.loop = true;
        _fallingAudioSource.Play();
        GameManager.OnPause += PauseFallingAudio;
        GameManager.OnResume += ResumeFallingAudio;
    }

    private void PauseFallingAudio() {
        if (_fallingAudioSource.clip == _fallingAudioClip) {
            _fallingAudioSource.Stop();
        }
    }

    private void ResumeFallingAudio() {
        if (_fallingAudioSource.clip == _fallingAudioClip) {
            _fallingAudioSource.Play();
        }
    }

    protected virtual void OnDisable() {
        GameManager.OnPause -= PauseFallingAudio;
        GameManager.OnResume -= ResumeFallingAudio;
    }
}

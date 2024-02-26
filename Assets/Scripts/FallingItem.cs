using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingItem : CollisionDetector, IPooledObject
{
    [SerializeField] private AudioClip _fallingAudioClip;
    [SerializeField] private AudioClip _landingAudioClip;

    private AudioSource _audioSource = null;
    protected virtual void Awake() {
        _audioSource = GetComponent<AudioSource>();
        if( _audioSource == null) {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    protected override void OnFirstCollision(Collision collision) {
        _audioSource.clip = _landingAudioClip;
        _audioSource.loop = false;
        _audioSource.Play();
    }

    public virtual void OnObjectSpawn() {
        ResetCollisionStatus();
        _audioSource.clip = _fallingAudioClip;
        _audioSource.loop = true;
        _audioSource.Play();
        GameManager.OnPause += PauseFallingAudio;
        GameManager.OnResume += ResumeFallingAudio;
    }

    private void PauseFallingAudio() {
        if(_audioSource.clip == _fallingAudioClip) {
            _audioSource.Stop();
        }
    }

    private void ResumeFallingAudio() {
        if(_audioSource.clip == _fallingAudioClip) {
            _audioSource.Play();
        }
    }

    protected virtual void OnDisable() {
        GameManager.OnPause -= PauseFallingAudio;
        GameManager.OnResume -= ResumeFallingAudio;
    }
}

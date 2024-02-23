using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IRunServiceable
{
    [SerializeField] private GameObject _explosionPrefab = null;
    [SerializeField] private float _timeBeforeExplode = 10.0f;
    [SerializeField] private float _timeForColorChange = 2.0f;
    [SerializeField] private float _percentageSpeedUp = 0.9f;
    [SerializeField] private Color _redColor = Color.red;
    [SerializeField] private Color _blackColor = Color.black;
    private bool _turningRed = true;
    private float _currentExplodeTime = 0;
    private float _currentColorChangeTime = 0;


    private void Start() {
        RunService.instance.AddRunServiceable(this);
    }

    public bool Run() {
        _currentExplodeTime += Time.deltaTime;
        _currentColorChangeTime += Time.deltaTime;
        if(_currentExplodeTime > _timeBeforeExplode) {
            Explode();
            return true;
        }
        if(_currentColorChangeTime > _timeForColorChange) {
            _timeForColorChange = _percentageSpeedUp * _timeForColorChange;
            _turningRed = !_turningRed;
            _currentColorChangeTime = 0;
        }
        Renderer renderer = GetComponent<Renderer>();
        Color baseColor = _turningRed ? _blackColor : _redColor;
        Color destColor = _turningRed ? _redColor : _blackColor;
        renderer.material.color = Color.Lerp(baseColor, destColor, _currentColorChangeTime / _timeForColorChange);
        return false;
    }

    private void Explode() {
        Destroy(gameObject);
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
    }
}

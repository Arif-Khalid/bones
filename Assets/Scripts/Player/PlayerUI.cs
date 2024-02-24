using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Canvas _generalUICanvas = null;
    [SerializeField] private Canvas _escapeMenuCanvas = null;
    [SerializeField] private Canvas _deathCanvas = null;

    private PlayerInput _playerInput = null;
    private void Start() {
        GameManager.OnEndGame += OnDeath;
    }

    private void Awake() {
        _playerInput = GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnPause() {
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.None;
        _playerInput.SwitchCurrentActionMap("PauseUI");
        _escapeMenuCanvas.enabled = true;
    }

    public void OnResume() {
        Time.timeScale = 1.0f;
        _playerInput.SwitchCurrentActionMap("Gameplay");
        DefaultGameplayUI();
    }

    public void OnStart() {
        GameManager.TriggerOnStartGame();
        DefaultGameplayUI();
    }

    private void DefaultGameplayUI() {
        _playerInput.SwitchCurrentActionMap("Gameplay");
        _generalUICanvas.enabled = false;
        _escapeMenuCanvas.enabled = false;
        _deathCanvas.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDeath() {
        _playerInput.SwitchCurrentActionMap("DeathUI");
        Cursor.lockState = CursorLockMode.None;
        _deathCanvas.enabled = true;
    }

    public void OnQuit() {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject _generalUICanvas = null;
    [SerializeField] private GameObject _escapeMenuCanvas = null;
    [SerializeField] private GameObject _deathCanvas = null;

    private PlayerInput _playerInput = null;
    private void Start() {
        _generalUICanvas.SetActive(true);
        _escapeMenuCanvas.SetActive(false);
        _deathCanvas.SetActive(false);
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
        _escapeMenuCanvas.SetActive(true);
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
        _generalUICanvas.SetActive(false);
        _escapeMenuCanvas.SetActive(false);
        _deathCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDeath() {
        _playerInput.SwitchCurrentActionMap("DeathUI");
        Cursor.lockState = CursorLockMode.None;
        _deathCanvas.SetActive(true);
    }

    public void OnQuit() {
        Application.Quit();
    }
}

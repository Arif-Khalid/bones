using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/**
 * Responsible for handling player UI logic
 */
[RequireComponent(typeof(PlayerInput))]
public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject _generalUICanvas = null;                                    // Where general other UI is
    [SerializeField] private GameObject _escapeMenuCanvas = null;                                   // What is shown when game is paused
    [SerializeField] private Slider _volumeSlider = null;

    // Death canvas
    [SerializeField] private GameObject _deathCanvas = null;                                        // What is shown when player is dead
    [SerializeField] private RectTransform _scrollViewContent = null;                               // The content of the scroll view located on the death canvas
    [SerializeField] private PoolId _stackabeItemUIId;                                              // Pool id of stackable item UI 
    [SerializeField] private StackableItemScriptableObject _stackableItemScriptableObject = null;   // Scriptable object linking Pool ID of stackable item to its
                                                                                                    // UI display properties

    private PlayerInput _playerInput = null;
    private void Start() {
        _generalUICanvas.SetActive(true);
        _escapeMenuCanvas.SetActive(false);
        _deathCanvas.SetActive(false);
        _volumeSlider.value = AudioListener.volume * _volumeSlider.maxValue;
        GameManager.OnEndGame += OnDeath;
    }

    private void Awake() {
        _playerInput = GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Functions called by Player Input component
    #region Player input
    private void OnPause() {
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.None;
        _playerInput.SwitchCurrentActionMap("PauseUI");
        _escapeMenuCanvas.SetActive(true);
        GameManager.TriggerOnPause();
    }

    public void OnResume() {
        Time.timeScale = 1.0f;
        _playerInput.SwitchCurrentActionMap("Gameplay");
        DefaultGameplayUI();
        GameManager.TriggerOnResume();
    }

    public void OnStart() {
        GameManager.TriggerOnStartGame();
        DefaultGameplayUI();
    }
    #endregion

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

    public void PopulateDeathCanvas(List<StackableItem> stackedItems) {
        // Make the correct number of UI in the scroll view content
        float contentHeight = 0f;
        while (_scrollViewContent.transform.childCount < stackedItems.Count) {
            GameObject stackableItemUI = ObjectPooler.instance.SpawnFromPool(_stackabeItemUIId, Vector3.zero, Quaternion.identity);
            stackableItemUI.transform.SetParent(_scrollViewContent.transform, false);
            contentHeight += stackableItemUI.GetComponent<RectTransform>().rect.height;
        }

        // Correct the size of content
        _scrollViewContent.sizeDelta = new Vector2(_scrollViewContent.sizeDelta.x, contentHeight);

        // Populate the scroll view content UIs with the correct names and images
        for (int i = 0; i < stackedItems.Count; i++) {
            Transform currentChild = _scrollViewContent.transform.GetChild(i);
            currentChild.GetComponentInChildren<TextMeshProUGUI>().text = _stackableItemScriptableObject.getNameFromId(stackedItems[i].PrefabPoolId);
            currentChild.GetComponentInChildren<Image>().sprite = _stackableItemScriptableObject.getImageFromId(stackedItems[i].PrefabPoolId);
        }
    }

    public void OnQuit() {
        Application.Quit();
    }

    public void OnVolumeChange() {
        AudioListener.volume = _volumeSlider.value / _volumeSlider.maxValue;
    }

    public void PlayButtonSound() {
        AudioManager.instance.Play(AudioID.Button);
    }

    private void OnDestroy() {
        GameManager.OnEndGame -= OnDeath;
    }
}

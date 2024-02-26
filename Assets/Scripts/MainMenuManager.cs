using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private Slider _volumeSlider;

    private void Start() {
        ShowDefaultUI();
    }

    public void EnterGameScene() {
        SceneManager.LoadScene(sceneName: "GameScene");
    }

    public void Quit() {
        Application.Quit();
    }

    public void EnterOptionsMenu() {
        _mainMenu.SetActive(false);
        _optionsMenu.SetActive(true);
    }

    // Works because I only have one other menu besides main menu
    private void OnBack() {
        ShowDefaultUI();
    }

    private void ShowDefaultUI() {
        _mainMenu.SetActive(true);
        _optionsMenu.SetActive(false);
    }

    public void OnVolumeChange() {
        AudioListener.volume = _volumeSlider.value / _volumeSlider.maxValue;
    }
}

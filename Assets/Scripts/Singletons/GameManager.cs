using UnityEngine;
using UnityEngine.Events;

/**
 * Responsible for centralising global game events
 */
public class GameManager : MonoBehaviour
{
    public static UnityAction<StackableItem> OnAddToStack;
    public static UnityAction OnStartGame;
    public static UnityAction OnEndGame;
    public static UnityAction OnPause;
    public static UnityAction OnResume;


    private static bool _isGameRunning = false;
    private static bool _isGamePaused = false;

    private void Start() {
        OnStartGame += StartGame;
        OnEndGame += EndGame;
        OnPause += PauseGame;
        OnResume += ResumeGame;
    }

    private void EndGame() {
        _isGameRunning = false;
    }

    private void PauseGame() {
        _isGamePaused = true;
    }

    private void ResumeGame() {
        _isGamePaused = false;
    }

    private void StartGame() {
        ObjectPooler.instance.ResetPools();
        _isGameRunning = true;
    }

    // Trigger functions called by other scripts
    // Handles null reference checks and multiple triggers
    #region Trigger functions
    public static void TriggerOnAddToStack(StackableItem stackableItem) {
        if (OnAddToStack != null) {
            OnAddToStack(stackableItem);
        }
    }

    public static void TriggerOnStartGame() {
        if (OnStartGame != null && !_isGameRunning) {
            OnStartGame();
        }
    }

    public static void TriggerOnEndGame() {
        if (OnEndGame != null && _isGameRunning) {
            OnEndGame();
        }
    }

    public static void TriggerOnPause() {
        if (OnPause != null && !_isGamePaused) {
            OnPause();
        }
    }

    public static void TriggerOnResume() {
        if (OnResume != null && _isGamePaused) {
            OnResume();
        }
    }
    #endregion

    private void OnDestroy() {
        OnStartGame -= StartGame;
        OnEndGame -= EndGame;
        OnPause -= PauseGame;
        OnResume -= ResumeGame;
    }
}

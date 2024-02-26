using UnityEngine;
using UnityEngine.Events;

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
}

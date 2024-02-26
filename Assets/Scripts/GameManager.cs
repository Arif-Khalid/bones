using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static UnityAction<StackableItem> OnAddToStack;
    public static UnityAction OnStartGame;
    public static UnityAction OnEndGame;


    private static bool _isGameRunning = false;

    private void Start() {

        OnStartGame += StartGame;
        OnEndGame += EndGame;
    }

    private void EndGame() {
        _isGameRunning = false;
    }

    private void StartGame() {
        ObjectPooler.instance.ResetPools();
        _isGameRunning = true;
    }

    public static void TriggerOnAddToStack(StackableItem stackableItem) {
        if(OnAddToStack != null) {
            OnAddToStack(stackableItem);
        }
    }

    public static void TriggerOnStartGame() {
        if(OnStartGame != null && !_isGameRunning) {
            OnStartGame();
        }
    }

    public static void TriggerOnEndGame() {
        if(OnEndGame != null && _isGameRunning) {
            OnEndGame();
        }
    }
}

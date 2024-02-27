using System.Collections.Generic;
using UnityEngine;

/**
 * Responsible for the logic behind the tray where items are stacked
 * Tray catches stackable items as they come into contact with the stack top
 */
public class Tray : MonoBehaviour
{
    [SerializeField] private float _stackRigidity = 1.0f;                   // How much should the stack move when the player moves
                                                                            // Higher values mean stack is more stable
    [SerializeField] private float _trayYOffset = 0.0f;                     // Used to offset game object origin to match tray mesh origin when aligning stack

    private StackableItem _currentStackTop = null;                          // The current top most stackable item
    private List<StackableItem> _stackedItems = new List<StackableItem>();
    private bool _isStacked = true;                                         // Remains true while the stack has not been toppled

    private PlayerUI _playerUI = null;
    private void Start() {
        _playerUI = GetComponentInParent<PlayerUI>();
        GameManager.OnAddToStack += AddToStack;
        GameManager.OnStartGame += ResetStack;
    }

    private void ResetStack() {
        _stackedItems.Clear();
        gameObject.tag = "StackTop";
        _isStacked = true;
    }

    private void AddToStack(StackableItem stackableItem) {
        float currentHeight;
        Vector3 currentPos;
        if(_currentStackTop == null) {
            currentHeight = gameObject.GetComponent<Renderer>().bounds.size.y / 2;
            currentPos = gameObject.transform.position;
            currentPos.y += _trayYOffset;
            gameObject.tag = "Untagged";
        }
        else {
            _currentStackTop.tag = "Untagged";
            currentHeight = _currentStackTop.Height;
            currentPos = _currentStackTop.transform.position;
        }

        float numberOfUnitsAboveTop = currentHeight / 2 + stackableItem.Height / 2;
        stackableItem.transform.position = new Vector3(
            currentPos.x,
            currentPos.y + numberOfUnitsAboveTop,
            currentPos.z);

        // Delete now uneeded components
        Destroy(stackableItem.GetComponent<Rigidbody>());

        _currentStackTop = stackableItem;
        _stackedItems.Add(stackableItem);
        stackableItem.tray = this;
        stackableItem.gameObject.tag = "StackTop";
        ObjectPooler.instance.RemoveFromPool(stackableItem.PrefabPoolId, stackableItem.gameObject);
    }

    // Enables ragdoll behaviour for all items in the stack
    public void RagdollStack() {
        if(!_isStacked) {
            return;
        }
        _isStacked = false;
        for (int i = 0; i < _stackedItems.Count; i++) {
            _stackedItems[i].gameObject.AddComponent<Rigidbody>();
        }
        _currentStackTop.tag = "Untagged";
        _currentStackTop = null;
        _playerUI.PopulateDeathCanvas(_stackedItems);
        GameManager.TriggerOnEndGame();
    }

    private void MoveStack() {
        for (int i = 0; i < _stackedItems.Count; i++) {
            Vector3 desiredPos = i > 0 ? 
                new Vector3(
                _stackedItems[i - 1].transform.position.x,
                _stackedItems[i].transform.position.y,
                _stackedItems[i - 1].transform.position.z) : 
                new Vector3(
                    gameObject.transform.position.x,
                    _stackedItems[i].transform.position.y,
                    gameObject.transform.position.z);

            _stackedItems[i].transform.position = Vector3.Lerp(
                _stackedItems[i].transform.position, 
                desiredPos, 
                _stackRigidity * Time.deltaTime);
            _stackedItems[i].transform.rotation = gameObject.transform.rotation;
        }
    }

    // Late Update is used since player movement and rotation is handled through Update
    private void LateUpdate() {
        if(_isStacked) {
            MoveStack();
        }
    }

    private void OnDestroy() {
        GameManager.OnAddToStack -= AddToStack;
        GameManager.OnStartGame -= ResetStack;
    }
}

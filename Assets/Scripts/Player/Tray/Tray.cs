using System.Collections.Generic;
using UnityEngine;

public class Tray : MonoBehaviour
{
    [SerializeField] private float _stackRigidity = 1.0f;
    private StackableItem _currentStackTop = null;
    private List<StackableItem> _stackedItems = new List<StackableItem>();
    private bool _isStacked = true;
    private void Start() {
        gameObject.tag = "StackTop";
        GameManager.OnAddToStack += AddToStack;
    }

    private void AddToStack(StackableItem stackableItem) {
        float currentHeight;
        Vector3 currentPos;
        if(_currentStackTop == null) {
            currentHeight = gameObject.GetComponent<Renderer>().bounds.size.y / 2;
            currentPos = gameObject.transform.position;
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

    public void RagdollStack() {
        if(!_isStacked) {
            return;
        }
        _isStacked = false;
        for (int i = 0; i < _stackedItems.Count; i++) {
            _stackedItems[i].gameObject.AddComponent<Rigidbody>();
        }
        _currentStackTop.tag = "Untagged";
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

    private void LateUpdate() {
        if(_isStacked) {
            MoveStack();
        }
    }
}

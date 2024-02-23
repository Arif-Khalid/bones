using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tray : MonoBehaviour
{
    [SerializeField] private float _stackRigidity = 1.0f;
    private GameObject _currentStackTop = null;
    private List<GameObject> _stackedItems = new List<GameObject>();
    private void Start() {
        _currentStackTop = gameObject;
        _stackedItems.Add(gameObject);
        gameObject.layer = LayerMask.NameToLayer("IgnorePlayerCollision");
        gameObject.tag = "StackTop";
        GameManager.OnAddToStack += AddToStack;
    }

    private void AddToStack(FallingItem fallingItem) {
        // If it was a falling item. Currently only time it is not is when it is the base tray.
        GameObject objectToAdd = fallingItem.gameObject;
        float numberOfUnitsAboveTop = _currentStackTop.GetComponent<Collider>().bounds.size.y / 2
            + fallingItem.Height / 2;
        objectToAdd.transform.position = new Vector3(
            _currentStackTop.transform.position.x,
            _currentStackTop.transform.position.y + numberOfUnitsAboveTop,
            _currentStackTop.transform.position.z);

        // Delete now uneeded components
        Destroy(fallingItem);
        Destroy(objectToAdd.GetComponent<Rigidbody>());

        _currentStackTop.tag = "Untagged";
        _currentStackTop = objectToAdd;
        _stackedItems.Add(objectToAdd);
        objectToAdd.layer = LayerMask.NameToLayer("IgnorePlayerCollision");
        objectToAdd.tag = "StackTop";
    }


    private void MoveStack() {
        for (int i = 1; i < _stackedItems.Count; i++) {
            Vector3 desiredPos = new Vector3(
                _stackedItems[i - 1].transform.position.x,
                _stackedItems[i].transform.position.y,
                _stackedItems[i - 1].transform.position.z);

            _stackedItems[i].transform.position = Vector3.Lerp(
                _stackedItems[i].transform.position, 
                desiredPos, 
                _stackRigidity * Time.deltaTime);
            _stackedItems[i].transform.rotation = _stackedItems[0].transform.rotation;
        }
    }

    private void LateUpdate() {
        MoveStack();
    }
}

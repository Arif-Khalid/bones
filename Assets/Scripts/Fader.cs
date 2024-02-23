using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    public static Fader instance;
    [SerializeField] private float _fadeSpeed = 1.0f;
    private Queue<GameObject> _objectsToFade = new();

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }

    public void AddObject(GameObject gameObject) {
        _objectsToFade.Enqueue(gameObject);
    }

    private void Update() {
        FadeObjects();
    }

    private void FadeObjects() {
        int size = _objectsToFade.Count;
        while (size > 0) {
            GameObject obj = _objectsToFade.Dequeue();
            float currentOpacity = obj.GetComponent<Renderer>().material.GetFloat("_Opacity");
            float newOpacity = currentOpacity - _fadeSpeed * Time.deltaTime;
            if (newOpacity > 0) {
                obj.GetComponent<Renderer>().material.SetFloat("_Opacity", newOpacity);
                _objectsToFade.Enqueue(obj);
            }
            else {
                Destroy(obj);
            }
            size -= 1;
        }
    }
}

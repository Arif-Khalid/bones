using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FallingItem : CollisionDetector
{
    // This is necessary because getting bounds during motion is inaccurate
    [HideInInspector] public float Height = 0;
    void Start(){
        Height = GetComponent<Collider>().bounds.size.y;
    }
}

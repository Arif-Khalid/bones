using UnityEngine;

/**
 * Responsible for getting a random 3D position within a boundary
 * Boundary is specified by bottom left and top right edges of a cuboid
 */
public class Boundary : MonoBehaviour
{
    private Vector3 _bottomLeftBoundPos;    // Automatically tied to the first child of this game object
    private Vector3 _topRightBoundPos;      // Automatically tied to the position of the second child of this game object

    public Vector3 RandomPosInBound {
        get
        {
            return new Vector3(
                Random.Range(_bottomLeftBoundPos.x, _topRightBoundPos.x),
                Random.Range(_bottomLeftBoundPos.y, _topRightBoundPos.y),
                Random.Range(_bottomLeftBoundPos.z, _topRightBoundPos.z));
        }
    }

    private void Awake() {
        _bottomLeftBoundPos = transform.GetChild(0).position;
        _topRightBoundPos = transform.GetChild(1).position;
    }

    // Helper function to visualise bounds in editor
    private void OnDrawGizmos() {
        _bottomLeftBoundPos = transform.GetChild(0).position;
        _topRightBoundPos = transform.GetChild(1).position;
        Vector3 topLeftBoundPos = new Vector3(_bottomLeftBoundPos.x, _topRightBoundPos.y, _topRightBoundPos.z);
        Vector3 bottomRightBoundPos = new Vector3(_topRightBoundPos.x, _bottomLeftBoundPos.y, _bottomLeftBoundPos.z);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_bottomLeftBoundPos, topLeftBoundPos);
        Gizmos.DrawLine(topLeftBoundPos, _topRightBoundPos);
        Gizmos.DrawLine(_topRightBoundPos, bottomRightBoundPos);
        Gizmos.DrawLine(bottomRightBoundPos, _bottomLeftBoundPos);
    }


}

using UnityEngine;

public class Boundary : MonoBehaviour
{
    private Vector3 _bottomLeftBoundPos;
    private Vector3 _topRightBoundPos;

    public Vector3 RandomPosInBound {
        get
        {
            return new Vector3(
                Random.Range(_bottomLeftBoundPos.x, _topRightBoundPos.x), 
                Random.Range(_bottomLeftBoundPos.y, _topRightBoundPos.y), 
                Random.Range(_bottomLeftBoundPos.z, _topRightBoundPos.z));
        }
    }


    // Draw the bound
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

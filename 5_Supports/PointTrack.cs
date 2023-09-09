using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTrack : MonoBehaviour
{
    [SerializeField] Color _lineColor = Color.yellow;
    Transform[] _points;

    void OnDrawGizmos()
    {
        Gizmos.color = _lineColor;
        _points = GetComponentsInChildren<Transform>();
        int nextIndex = 1;
        Vector3 currPos = _points[nextIndex].position;
        Vector3 nextPos;
        for(int n = 0; n <= _points.Length; n++)
        {
            nextPos = (++nextIndex >= _points.Length) ? _points[1].position : _points[nextIndex].position;
            Gizmos.DrawLine(currPos, nextPos);
            currPos = nextPos;
        }
    }
}

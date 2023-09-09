using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmosRange : MonoBehaviour
{
    [SerializeField] Color _rangeColor = Color.red;
    [SerializeField] float _radius = 0.5f;

    public void InitSet(float r, Color color)
    {
        _radius = r;
        _rangeColor = color;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = _rangeColor;
        Gizmos.DrawSphere(transform.position, _radius);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCamera : MonoBehaviour
{
    [SerializeField] Vector3 _offSet = Vector3.zero;

    Transform _tfPlayer;

    void Update()
    {
        if(_tfPlayer != null)
        {
            transform.position = _tfPlayer.position + _offSet;
        }
        else
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if(go != null)
            {
                _tfPlayer = go.transform;
            }
        }
    }
}

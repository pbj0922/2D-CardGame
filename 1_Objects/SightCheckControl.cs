using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightCheckControl : MonoBehaviour
{
    SphereCollider _mySightRange;
    Monster _owner;

    void Awake()
    {
        _mySightRange = GetComponent<SphereCollider>();
    }

    public void InitSet(float range)
    {
        _mySightRange.radius = range / 2;
        GetComponent<MyGizmosRange>().InitSet(_mySightRange.radius, new Color(1, 0, 0, 0.1f));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 내 앞에 있는지 체크
            float d = Vector3.Dot(transform.forward, other.transform.position - transform.position);
            if(d > 0)
            {
                // 플레이어와 몬스터 사이에 장애물이 있는지 체크
                RaycastHit rHit;
                Player p = other.gameObject.GetComponent<Player>();
                Monster m = transform.parent.GetComponent<Monster>();
                Ray ray = new Ray(m._castBoneRay.position, p._castBoneRay.position - m._castBoneRay.position);
                if(Physics.Raycast(ray, out rHit, _mySightRange.radius))
                {
                    if (rHit.transform.CompareTag(other.tag))
                    {
                        m.BattleOn(other.transform);
                    }
                }
            }
        }    
    }

    void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }

}

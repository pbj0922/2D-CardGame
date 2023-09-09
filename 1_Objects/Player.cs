using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnumHelper;

public class Player : CharacterBase
{
    [SerializeField] BoxCollider[] _normalDamageZone;
    [SerializeField] Transform _rayBone;

    // 참조
    NavMeshAgent _navAgent;
    Animator _aniControl;
    Camera _myCam;
    Transform _target;
    AudioSource _voiceOutput;
    SimpleStatusBox _StatusBox;

    // 정보
    eCharacterActionState _currActionState;
    bool _selectEnemy = false;
    float _limitAttackRange = 2;
    float _attackTypeRatio = 0;

    // Parameter
    eClassKind _class;
    eTribeKind _tribe;
    int _level;
    int _maxHP;
    int _nowHP;
    int _maxMP;
    int _nowMP;
    float _runSpeed = 4;
    float _speedScale = 1;

    public int _finalDamage
    {
        get { return AttackPowByClass(); }
    }
    public int _finalDeffence
    {
        get { return DeffencePowByClass(); }
    }
    public float _finishSpeed
    {
        get { return _runSpeed * _speedScale; }
    }
    public float _hpRate
    {
        get { return (float)_nowHP / _maxHP; }
    }
    public float _mpRate
    {
        get { return (float)_nowMP / _maxMP; }
    }
    public Transform _castBoneRay
    {
        get { return _rayBone; }
    }

    void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _aniControl = GetComponent<Animator>();
        _voiceOutput = GetComponent<AudioSource>();
        _StatusBox = GetComponent<SimpleStatusBox>();
    }

    void Start()
    {
        _myCam = Camera.main;

        // 임시
        ImsiStartSettings();
        //===
    }

    void Update()
    {
        if(_isDead)
        {
            return;
        }

        switch (_currActionState)
        {
            case eCharacterActionState.Run:
                if (_selectEnemy)
                {
                    if (_target.GetComponent<CharacterBase>()._isDead)
                    {
                        AnimationChangeByAction(eCharacterActionState.Idle);
                    }
                    else
                    {
                        if (_navAgent.remainingDistance <= _limitAttackRange)
                        {
                            AnimationChangeByAction(eCharacterActionState.Attack);
                        }
                        else
                        {
                            // 위치 갱신
                            AnimationChangeByAction(eCharacterActionState.Run);
                            _navAgent.stoppingDistance = _limitAttackRange;
                            _navAgent.destination = _target.position;
                        }
                    }
                }
                else
                {
                    if (_navAgent.remainingDistance == 0)
                    {
                        AnimationChangeByAction(eCharacterActionState.Idle);
                    }
                }
                break;
            case eCharacterActionState.Attack:
                if(_target.GetComponent<CharacterBase>()._isDead)
                {
                    AnimationChangeByAction(eCharacterActionState.Idle);
                }
                else
                {
                    if (Vector3.Distance(transform.position, _target.position) <= _limitAttackRange)
                    {
                        transform.LookAt(_target);
                    }
                    else
                    {
                        AnimationChangeByAction(eCharacterActionState.Run);
                        _navAgent.stoppingDistance = _limitAttackRange;
                        _navAgent.destination = _target.position;
                    }
                }
                break;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit rHit;
            Ray ray = _myCam.ScreenPointToRay(Input.mousePosition);
            int lMask = 1 << LayerMask.NameToLayer("FIELD")
                        | 1 << LayerMask.NameToLayer("SELECTOBJECT") 
                        | 1 << LayerMask.NameToLayer("MONSTER");

            if (Physics.Raycast(ray, out rHit, Mathf.Infinity, lMask))
            {
                if (rHit.transform.CompareTag("DummyObj") || rHit.transform.CompareTag("Monster"))
                {
                    _selectEnemy = true;
                    if(Vector3.Distance(transform.position, rHit.point) >= _limitAttackRange)
                    {
                        AnimationChangeByAction(eCharacterActionState.Run);
                    }
                    else
                    {
                        AnimationChangeByAction(eCharacterActionState.Attack);
                    }
                    _target = rHit.transform;
                    _navAgent.stoppingDistance = _limitAttackRange;
                    _navAgent.destination = _target.position;
                }
                else
                {
                    _selectEnemy = false;
                    AnimationChangeByAction(eCharacterActionState.Run);
                    _navAgent.stoppingDistance = 0;
                    _navAgent.destination = rHit.point;
                }
            }
        }


    }

    #region [public Func]
    public override void InitializeDataSet(string na, int s, int c, int d, int i, int w, int sub1, int sub2)
    {
        _isDead = false;
        _level = 1;
        _name = na;
        _str = s + 1;
        _con = c + 2;
        _dex = d + 1;
        _int = i;
        _wis = w;

        _tribe = (eTribeKind)sub1;
        _class = (eClassKind)sub2;
        AdditionStatByTribe();
        AdditionStatByClass();
        SettingStatusToHPnMP();

        // 기본 설정
        DisableDMZones();
    }

    public override void Hitting(int finishDam)
    {

        int finalDam = finishDam - _finalDeffence;
        finalDam = (finalDam < 1) ? 1 : finalDam;

        _nowHP -= finalDam;
        if (_nowHP <= 0)
        {
            _nowHP = 0;
            AnimationChangeByAction(eCharacterActionState.Die);
        }
        Debug.Log(_nowHP);
    }

    #endregion [public Func]

    #region [private Func]
    void AdditionStatByTribe()
    {
        int t = (int)_tribe;
        int s = TableManager._instance.Get(TableClassName.TribeWeightedValueTable).ToInt32(t, "STR");
        int c = TableManager._instance.Get(TableClassName.TribeWeightedValueTable).ToInt32(t, "CON");
        int d = TableManager._instance.Get(TableClassName.TribeWeightedValueTable).ToInt32(t, "DEX");
        int i = TableManager._instance.Get(TableClassName.TribeWeightedValueTable).ToInt32(t, "INT");
        int w = TableManager._instance.Get(TableClassName.TribeWeightedValueTable).ToInt32(t, "WIS");

        _str += s;
        _con += c;
        _dex += d;
        _int += i;
        _wis += w;
    }
    void AdditionStatByClass()
    {
        int t = (int)_class;
        int s = TableManager._instance.Get(TableClassName.ClassWeightedValueTable).ToInt32(t, "STR");
        int c = TableManager._instance.Get(TableClassName.ClassWeightedValueTable).ToInt32(t, "CON");
        int d = TableManager._instance.Get(TableClassName.ClassWeightedValueTable).ToInt32(t, "DEX");
        int i = TableManager._instance.Get(TableClassName.ClassWeightedValueTable).ToInt32(t, "INT");
        int w = TableManager._instance.Get(TableClassName.ClassWeightedValueTable).ToInt32(t, "WIS");

        _str += s;
        _con += c;
        _dex += d;
        _int += i;
        _wis += w;
    }
    void SettingStatusToHPnMP()
    {
        switch (_class)
        {
            case eClassKind.Fighter:
                _nowHP = _maxHP = (int)(_con * 10 + _str * 3);
                _nowMP = _maxMP = (int)(_int * 3 + _wis * 6);
                break;
            case eClassKind.Wizard:
                _nowHP = _maxHP = (int)(_con * 8);
                _nowMP = _maxMP = (int)(_int * 6 + _wis * 8);
                break;
            case eClassKind.Rogue:
                _nowHP = _maxHP = (int)(_con * 9.2f + _str * 1.5f);
                _nowMP = _maxMP = (int)(_int * 5 + _wis * 6);
                break;
        }
    }
    int AttackPowByClass()
    {
        int attPow = 0;
        switch (_class)
        {
            case eClassKind.Fighter:
                attPow = (int)((_str * 1.5f) + (_con * 0.8f) + (_dex * 0.5f));
                break;
            case eClassKind.Wizard:
                attPow = (int)((_int * 1.8f) + (_wis * 1.1f) + (_dex * 0.2f));
                break;
            case eClassKind.Rogue:
                attPow = (int)((_str * 1.0f) + (_wis * 0.6f) + (_dex * 1.6f));
                break;
        }

        return attPow;
    }
    int DeffencePowByClass()
    {
        int defPow = 0;
        switch (_class)
        {
            case eClassKind.Fighter:
                defPow = (int)((_con * 1.4f) + (_str * 1.1f) + (_dex * 0.6f));
                break;
            case eClassKind.Wizard:
                defPow = (int)((_con * 1.0f) + (_str * 0.2f) + (_wis * 0.6f) + (_int * 0.4f));
                break;
            case eClassKind.Rogue:
                defPow = (int)((_con * 1.3f) + (_str * 0.7f) + (_dex * 1.1f));
                break;
        }

        return defPow;
    }
    void SetParamUponLevelUp()
    {
    }

    void AnimationChangeByAction(eCharacterActionState state)
    {
        if(_isDead)
        {
            return;
        }
        // 임시
        _aniControl.speed = 1;
        //===
        switch (state)
        {
            case eCharacterActionState.Run:
                DisableDMZones();
                _navAgent.speed = _finishSpeed;
                _aniControl.speed = _speedScale;
                break;
            case eCharacterActionState.Die:
                _isDead = true;
                DisableDMZones();
                _aniControl.SetTrigger("IsDead");
                GetComponent<CharacterController>().enabled = false;
                break;
        }

        _aniControl.SetInteger("AniType", (int)state);

        _currActionState = state;
    }
    void SetAttackType()
    {
        int sel = Random.Range(0, 3);
        _attackTypeRatio = sel * 0.5f;
        _aniControl.SetFloat("AttackType", _attackTypeRatio);
    }
    void DisableDMZones()
    {
        foreach(BoxCollider coll in _normalDamageZone)
        {
            coll.enabled = false;
        }
    }
    void DamageZoneOn(int num)
    {
        _normalDamageZone[num - 1].enabled = true;
    }
    void DamageZoneOff(int num)
    {
        _normalDamageZone[num - 1].enabled = false;
    }

    #endregion [private Func]

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("M_DMZ"))
        {
            Monster mon = other.transform.parent.GetComponent<Monster>();
            Hitting(mon._finalDamage);
        }
    }

    void ImsiStartSettings()
    {
        int t = TableManager._instance.Get(TableClassName.BaseStatusTable).ToInt32(0, "Tribe");
        int s = TableManager._instance.Get(TableClassName.BaseStatusTable).ToInt32(0, "STR");
        int c = TableManager._instance.Get(TableClassName.BaseStatusTable).ToInt32(0, "CON");
        int d = TableManager._instance.Get(TableClassName.BaseStatusTable).ToInt32(0, "DEX");
        int i = TableManager._instance.Get(TableClassName.BaseStatusTable).ToInt32(0, "INT");
        int w = TableManager._instance.Get(TableClassName.BaseStatusTable).ToInt32(0, "WIS");
        int ex = TableManager._instance.Get(TableClassName.BaseStatusTable).ToInt32(0, "ExtraPoint");

        InitializeDataSet("깐데또까", s, c, d, i, w, t, 0);
    }
}

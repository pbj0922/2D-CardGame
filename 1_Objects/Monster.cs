using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnumHelper;

public class Monster : CharacterBase
{
    [SerializeField] BoxCollider _damageZone;
    [SerializeField] SightCheckControl _sightChkCtrl;
    [SerializeField] Transform _rayBone;

    // 참조
    Animator _aniControl;
    NavMeshAgent _navAgent;
    List<Vector3> _roamPointList;
    MiniMonStatusWnd _miniStatusWnd;
    Transform _targetPlayer;

    // 정보
    eCharacterActionState _currActionState;
    eEnemyPersonalityType _personalType;
    eRoammingType _roamType;
    Vector3 _spawnPos;
    Vector3 _preBattleLocation;
    float _limitRandomX = 5;
    float _limitRandomZ = 5;
    float _minWaitTime = 3f;
    float _maxWaitTime = 8f;
    float _maxDistanceToChase = 25;
    bool _selectAct = false;
    //int _movementRatio = 50;
    float _waitTime = 0;
    int _index = 0;
    bool _isBack = false;
    bool _isFirst = true;

    // Parameter
    eTribeKind _tribe;
    eGradeType _grade;
    int _maxHP;
    int _nowHP;
    float _walkSpeed = 0.8f;
    float _runSpeed = 2.6f;
    float _returnSpeedScale = 2;
    float _sightRange = 10;
    float _attackLength = 1.5f;

    public int _finalDamage
    {
        get { return (int)((_str * 1.5f) + (_con * 0.8f) + (_dex * 0.5f)); ; }
    }
    public int _finalDeffence
    {
        get { return (int)((_con * 1.5f) + (_str * 0.4f) + (_wis * 0.3f) + (_int * 0.3f)); }
    }
    public float _hpRate
    {
        get { return (float)_nowHP / _maxHP; }
    }

    public Transform _castBoneRay
    {
        get { return _rayBone; }
    }

    void Awake()
    {
        _aniControl = GetComponent<Animator>();
        _navAgent = GetComponent<NavMeshAgent>();

        _spawnPos = transform.position;
    }

    void Start()
    {
            
    }

    void Update()
    {
        if (_isDead)
        {
            return;
        }

        switch (_currActionState)
        {
            case eCharacterActionState.Idle:
                if((_waitTime -= Time.deltaTime) <= 0)
                {
                    _waitTime = 0;
                    _selectAct = false;
                }
                break;
            case eCharacterActionState.Walk:
                if (_navAgent.remainingDistance == 0)
                {
                    _selectAct = false;
                }
                break;
            case eCharacterActionState.Run:
                if (Vector3.Distance(transform.position, _preBattleLocation) >= _maxDistanceToChase)
                {
                    _navAgent.destination = _preBattleLocation;
                    AnimationChangeByAction(eCharacterActionState.BackHome);
                    _targetPlayer = null;
                }
                else
                {
                    if (Vector3.Distance(transform.position, _targetPlayer.position) > _attackLength)
                    {
                        _navAgent.destination = _targetPlayer.position;
                    }
                    else
                    {
                        AnimationChangeByAction(eCharacterActionState.Attack);
                    }
                }
                break;
            case eCharacterActionState.Attack:
                if(_targetPlayer.GetComponent<CharacterBase>()._isDead)
                {
                    _navAgent.destination = _preBattleLocation;
                    AnimationChangeByAction(eCharacterActionState.BackHome);
                    _targetPlayer = null;
                }
                else
                {
                    if (Vector3.Distance(transform.position, _targetPlayer.position) > _attackLength)
                    {
                        _navAgent.destination = _targetPlayer.position;
                        AnimationChangeByAction(eCharacterActionState.Run);
                    }
                    else
                    {
                        transform.LookAt(_targetPlayer);
                    }
                }
                break;
            case eCharacterActionState.BackHome:
                if(_navAgent.remainingDistance == 0)
                {
                    _isFirst = true;
                    _selectAct = false;
                }
                break;
        }
        SelectAIProcess();
    }

    public override void InitializeDataSet(string na, int s, int c, int d, int i, int w, int sub1, int sub2)
    {
        _isDead = false;
        _name = na;
        _str = s;
        _con = c;
        _dex = d;
        _int = i;
        _wis = w;
        // 종족
        _tribe = (eTribeKind)sub1;
        // 등급
        _grade = (eGradeType)sub2;

        SettingStatusToHPnMP();
    }

    public void InitElementSettings(eEnemyPersonalityType pType, eRoammingType rType, List<Vector3> points)
    {
        _personalType = pType;
        _roamType = rType;
        _roamPointList = points;

        _damageZone.enabled = false;
        _sightChkCtrl.InitSet(_sightRange);

        _miniStatusWnd = gameObject.GetComponentInChildren<MiniMonStatusWnd>();
        _miniStatusWnd.InitSet(_name, _grade);
        //_miniStatusWnd.ShowWindow(false);
    }

    public void BattleOn(Transform target)
    {
        if(_currActionState == eCharacterActionState.BackHome)
        {
            return;
        }
        if(_isFirst)
        {
            _preBattleLocation = transform.position;
            _isFirst = false;
        }

        _targetPlayer = target;
        if (Vector3.Distance(transform.position, _targetPlayer.position) > _attackLength)
        {
            AnimationChangeByAction(eCharacterActionState.Run);
            _navAgent.destination = _targetPlayer.position;
        }
        else
        {
            AnimationChangeByAction(eCharacterActionState.Attack);
        }
    }

    void SelectAIProcess()
    {
        if (!_selectAct)
        {
            int pick = Random.Range(0, 100);
            //if(pick < _movementRatio)
            if(pick < (int)_personalType)
            {
                // 이동
                AnimationChangeByAction(eCharacterActionState.Walk);
                _navAgent.destination = GetGoalPosition();
            }
            else
            {
                // 대기
                AnimationChangeByAction(eCharacterActionState.Idle);
                _waitTime = Random.Range(_minWaitTime, _maxWaitTime);
            }

            _selectAct = true;
        }
    }

    Vector3 GetGoalPosToRandom(float halfRangeW, float halfRangeH)
    {
        float gx = Random.Range(-halfRangeW, halfRangeW);
        float gz = Random.Range(-halfRangeH, halfRangeH);

        return _spawnPos + new Vector3(gx, 0, gz);
    }

    Vector3 GetGoalPosition()
    {
        Vector3 nextPos = transform.position;
        switch(_roamType)
        {
            case eRoammingType.RandomField:
                nextPos = GetGoalPosToRandom(_limitRandomX, _limitRandomZ);
                break;
            case eRoammingType.RandomPosition:
                _index = Random.Range(0, _roamPointList.Count);
                nextPos = _roamPointList[_index];
                break;
            case eRoammingType.InOrder:
                nextPos = _roamPointList[_index];
                _index = (++_index >= _roamPointList.Count ? 0 : _index);
                break;
            case eRoammingType.BackNForth:
                nextPos = _roamPointList[_index];
                if(_isBack)
                {
                    if(--_index <= 0)
                    {
                        _isBack = false;
                    }
                    else
                    {
                        if(++_index >= _roamPointList.Count - 1)
                        {
                            _isBack = true;
                        }
                    }
                }
                Debug.Log(_index);
                break;
        }
        return nextPos;
    }

    void SettingStatusToHPnMP()
    {
        _nowHP = _maxHP = (int)(_con * 10 + _str * 3 + _wis * 2);
    }

    void AnimationChangeByAction(eCharacterActionState state)
    {
        if (_isDead)
        {
            return;
        }

        switch (state)
        {
            case eCharacterActionState.Walk:
                _navAgent.speed = _walkSpeed;
                _navAgent.angularSpeed = 180;
                break;
            case eCharacterActionState.Run:
                _navAgent.speed = _runSpeed;
                _navAgent.angularSpeed = 270;
                _navAgent.stoppingDistance = _attackLength;
                _damageZone.enabled = false;
                DamageZoneOff();
                break;
            case eCharacterActionState.Die:
                _isDead = true;
                DamageZoneOff();
                GetComponent<CharacterController>().enabled = false;
                Destroy(gameObject, 5);
                break;
        }

        if (state == eCharacterActionState.BackHome)
        {
            _navAgent.speed = _runSpeed * _returnSpeedScale;
            _aniControl.SetInteger("AniType", (int)eCharacterActionState.Run);
            _aniControl.speed = _returnSpeedScale;
        }
        else
        {
            _aniControl.speed = 1;
            _aniControl.SetInteger("AniType", (int)state);
        }
        _currActionState = state;
    }

    void DamageZoneOn()
    {
        _damageZone.enabled = true;
    }
    void DamageZoneOff()
    {
        _damageZone.enabled = false;
    }

    public override void Hitting(int finishDam)
    {
        int finalDam = finishDam - _finalDeffence;
        finalDam = (finalDam < 1) ? 1 : finalDam;

        _nowHP -= finalDam;
        if(_nowHP <= 0)
        {
            _nowHP = 0;
            Debug.LogFormat("{0}이 체력이 0이 되어 사망했습니다", _name);
            AnimationChangeByAction(eCharacterActionState.Die);
        }
        _miniStatusWnd.SettingHpRate(_hpRate);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("P_DMZ"))
        {
            //Debug.Log("맞았다!!!!");
            Player pp = other.transform.parent.GetComponent<Player>();
            Hitting(pp._finalDamage);
            BattleOn(pp.transform);
        }
    }

    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(10, 10, 200, 100), "IDLE"))
    //    {
    //        AnimationChangeByAction(eCharacterActionState.Idle);
    //    }
    //    if (GUI.Button(new Rect(10, 110, 200, 100), "WALK"))
    //    {
    //        AnimationChangeByAction(eCharacterActionState.Walk);
    //    }
    //    if (GUI.Button(new Rect(10, 210, 200, 100), "RUN"))
    //    {
    //        AnimationChangeByAction(eCharacterActionState.Run);
    //    }
    //    if (GUI.Button(new Rect(10, 310, 200, 100), "ATTACK"))
    //    {
    //        AnimationChangeByAction(eCharacterActionState.Attack);
    //    }
    //    if (GUI.Button(new Rect(10, 410, 200, 100), "DEAD"))
    //    {
    //        AnimationChangeByAction(eCharacterActionState.Die);
    //    }
    //}
}

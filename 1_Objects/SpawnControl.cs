using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumHelper;

public class SpawnControl : MonoBehaviour
{
    [SerializeField] float _generateTime = 7;                       // 생성 주기
    [SerializeField] int _maxNumGenerations = 10;                   // 최대 생성 개수
    [SerializeField] int _limitGenerateCount = 1;                   // 살아있는 한계 생성 수
    [SerializeField] eEnemyPersonalityType _spawnPersonType = eEnemyPersonalityType.none;
    [SerializeField] eRoammingType _spawnRoamType = eRoammingType.RandomPosition;

    List<GameObject> _createObjList;
    List<Vector3> _roammPosList;
    float _checkTime = 0;
    GameObject _prefab;

    void Awake()
    {
        _createObjList = new List<GameObject>();
        _roammPosList = new List<Vector3>();
    }

    void Start()
    {
        _prefab = ResPoolManager._instance.GetCharPrefab(EnumHelper.CharPrefabName.Mon_Skeleton);
        SettingRoammingList();
    }

    void Update()
    {
        if(_maxNumGenerations > 0)
        {
            if(_createObjList.Count < _limitGenerateCount)
            {
                _checkTime += Time.deltaTime;
                if(_checkTime >= _generateTime)
                {
                    SpawnChar();
                    _maxNumGenerations--;
                    _checkTime = 0;
                }
            }
        }

    }

    void LateUpdate()
    {
        for(int n = 0; n < _createObjList.Count; n++)
        {
            if(_createObjList[n] == null)
            {
                _createObjList.RemoveAt(n);
                break;
            }
        }
    }

    void SpawnChar()
    {
        GameObject go = Instantiate(_prefab, transform.position, transform.rotation);
        // 몬스터 초기화
        Monster mon = go.GetComponent<Monster>();

        eEnemyPersonalityType setType = _spawnPersonType;
        if (_spawnPersonType == eEnemyPersonalityType.none)
        {
            switch(Random.Range(0, 3))
            {
                case 0:
                    setType = eEnemyPersonalityType.Lazy;
                    break;
                case 1:
                    setType = eEnemyPersonalityType.Ordinary;
                    break;
                case 2:
                    setType = eEnemyPersonalityType.Passionate;
                    break;
            }
        }
        string name = TableManager._instance.Get(TableClassName.MonsterInfoTable).ToStr(101, "Name");
        int s = TableManager._instance.Get(TableClassName.MonsterInfoTable).ToInt32(101, "STR");
        int c = TableManager._instance.Get(TableClassName.MonsterInfoTable).ToInt32(101, "CON");
        int d = TableManager._instance.Get(TableClassName.MonsterInfoTable).ToInt32(101, "DEX");
        int i = TableManager._instance.Get(TableClassName.MonsterInfoTable).ToInt32(101, "INT");
        int w = TableManager._instance.Get(TableClassName.MonsterInfoTable).ToInt32(101, "WIS");
        int sub1 = TableManager._instance.Get(TableClassName.MonsterInfoTable).ToInt32(101, "Tribe");
        int sub2 = TableManager._instance.Get(TableClassName.MonsterInfoTable).ToInt32(101, "Grade");
        mon.InitializeDataSet(name, s, c, d, i, w, sub1, sub2);
        mon.InitElementSettings(setType, _spawnRoamType, _roammPosList);

        _createObjList.Add(go);
    }

    void SettingRoammingList()
    {
        Transform root = transform.GetChild(0);
        for(int n = 0; n < root.childCount; n++)
        {
            _roammPosList.Add(root.GetChild(n).position);
        }
    }
}

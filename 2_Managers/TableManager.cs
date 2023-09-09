using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumHelper;


public class TableManager : TSingleton<TableManager>
{
    Dictionary<TableClassName, LowBase> _tableList = new Dictionary<TableClassName, LowBase>();

    protected override void Init()
    {
        base.Init();
    }

    public void LoadTable()
    {
        Load<BaseStatusTable>(TableClassName.BaseStatusTable);
        Load<TribeWeightedValueTable>(TableClassName.TribeWeightedValueTable);
        Load<ClassWeightedValueTable>(TableClassName.ClassWeightedValueTable);
        Load<MonsterInfoTable>(TableClassName.MonsterInfoTable);
    }

    public LowBase Load<T>(TableClassName type) where T : LowBase, new()
    {
        // 이미 테이블이 존재한다면 함수에서 나간다
        if(!_tableList.ContainsKey(type))
        {
            TextAsset textAsset = Resources.Load("TableDatas/" + type.ToString()) as TextAsset;

            if(textAsset != null)
            {
                T t = new T();
                t.LoadJson(textAsset.text);
                _tableList.Add(type, t);
            }
            else
            {
                Debug.Log("textAsset을 받아오지 못했습니다. 확인해 주세요.");
                return null;
            }
        }

        return _tableList[type];
    }

    public LowBase Get(TableClassName type)
    {
        if(_tableList.ContainsKey(type))
        {
            return _tableList[type];
        }
        return null;
    }
}

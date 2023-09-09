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
        // �̹� ���̺��� �����Ѵٸ� �Լ����� ������
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
                Debug.Log("textAsset�� �޾ƿ��� ���߽��ϴ�. Ȯ���� �ּ���.");
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

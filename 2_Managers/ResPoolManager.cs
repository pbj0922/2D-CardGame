using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumHelper;

public class ResPoolManager : TSingleton<ResPoolManager>
{
    List<Color> _GradeColorList = new List<Color>();
    List<GameObject> _prefabCharList = new List<GameObject>();

    protected override void Init()
    {
        base.Init();
    }

    #region [�ܺ� ��� �Լ�]
    public void EssentialLoad()
    {
        LoadCharacterPrefabs();
        SettingGradeColors();
    }

    public GameObject GetCharPrefab(CharPrefabName name)
    {
        return _prefabCharList[(int)name];
    }
    public Color GetGradeColor(eGradeType grade)
    {
        return _GradeColorList[(int)grade];
    }
    #endregion [�ܺ� ��� �Լ�]

    #region [���� ��� �Լ�]
    void LoadCharacterPrefabs()
    {
        int count = (int)CharPrefabName.max;
        for (int n = 0; n < count; n++)
        {
            GameObject prefab = Resources.Load("Characters/" + ((CharPrefabName)n).ToString()) as GameObject;
            _prefabCharList.Add(prefab);
        }
    }
    void SettingGradeColors()
    {
        //_GradeColorList.Add(new Color(1, 1, 1));
        _GradeColorList.Add(Color.white);
        _GradeColorList.Add(Color.green);
        _GradeColorList.Add(Color.blue);
        _GradeColorList.Add(Color.yellow);
        _GradeColorList.Add(Color.red);
    }
    #endregion [���� ��� �Լ�]
}

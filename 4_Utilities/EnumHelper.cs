using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnumHelper
{
    #region[Manager Define]
    public enum TableClassName
    {
        BaseStatusTable,
        TribeWeightedValueTable,
        ClassWeightedValueTable,
        MonsterInfoTable,

        max
    }
    public enum SceneType
    {
        none                    = 0,
        IngameScene
    }
    public enum CharPrefabName
    {
        PlayerHunter,
        Mon_Skeleton,

        max
    }
    public enum EffectType
    {
    }
    #endregion[Manager Define]

    #region [Character Define]
    public enum eClassKind
    {
        Fighter             = 0,
        Wizard,
        Rogue
    }
    public enum eTribeKind
    {
        Human               = 0,            // 사람
        Drow,                               // 다크엘프
        HalfOrc                             // 반인반오크
    }
    public enum eGradeType
    {
        Normal,                 // 흰색
        Rare,                   // 녹색
        Elite,                  // 파란색
        Unique,                 // 노란색
        Boss                    // 빨간색
    }
    public enum eCharacterActionState
    {
        Idle                = 0,
        Walk,
        Run,
        Attack,
        Skill,
        Die,
        BackHome
    }
    public enum eEnemyPersonalityType
    {
        none                    = 0,
        Lazy                    = 15,
        Ordinary                = 35,
        Passionate              = 80

    }
    public enum eRoammingType
    {
        RandomField,
        RandomPosition,
        InOrder,
        BackNForth
    }
    #endregion [Character Define]
}

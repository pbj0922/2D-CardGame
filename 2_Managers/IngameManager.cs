using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    static IngameManager _uniqueInstance;

    public static IngameManager _instance
    {
        get { return _uniqueInstance; }
    }
    void Awake()
    {
        _uniqueInstance = this;

        // юс╫ц
        TableManager._instance.LoadTable();
        ResPoolManager._instance.EssentialLoad();
        //===
    }

    //void Start()
    //{
    //    // Test
    //    string val = TableManager._instance.Get(EnumHelper.TableClassName.BaseStatusTable).ToStr(4, "INT");
    //    Debug.LogFormat("[index: {0}, column: {1} || {2}]", 4, "INT", val);
    //    //===
    //}
}

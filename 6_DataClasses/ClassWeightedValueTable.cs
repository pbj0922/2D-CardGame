using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class ClassWeightedValueTable : LowBase
{
    public enum Index
    {
        Index,
        STR,
        CON,
        DEX,
        INT,
        WIS,

        max
    }
    string _mainKey = "Index";

    public override void LoadJson(string strJson)
    {
        JSONNode node = JSONNode.Parse(strJson);
        for (int n = 0; n < (int)Index.max; n++)
        {
            Index subKey = (Index)n;
            if (string.Compare(_mainKey, subKey.ToString()) != 0)
            {
                for (int m = 0; m < node[0].AsArray.Count; m++)
                {
                    Add(node[0][m][_mainKey], subKey.ToString(), node[0][m][subKey.ToString()].Value);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumHelper;

public abstract class LowBase
{
    Dictionary<string, Dictionary<string, string>> _table = new Dictionary<string, Dictionary<string, string>>();

    public Dictionary<string, Dictionary<string, string>> _TABLE
    {
        get { return _table; }
        set { _table = value; }
    }

    protected void Add(string key, string subKey, string Val)
    {
        if (!_table.ContainsKey(key))
        {
            _table.Add(key, new Dictionary<string, string>());
        }
        if (!_table[key].ContainsKey(subKey))
        {
            _table[key].Add(subKey, Val);
        }
    }

    public abstract void LoadJson(string strJson);

    public int MaxCount()
    {
        return _table.Count;
    }

    public string ToStr(int key, string subKey)
    {
        string findValue = string.Empty;
        if (_table.ContainsKey(key.ToString()))
        {
            _table[key.ToString()].TryGetValue(subKey, out findValue);
        }

        return findValue;
    }

    public int ToInt32(int key, string subKey)
    {
        int findValue = int.MinValue;
        string val = ToStr(key, subKey);
        if (val != string.Empty)
        {
            if (!int.TryParse(val, out findValue))
            {
                findValue = int.MinValue;
            }
        }
        return findValue;
    }

    public float ToFloat(int key, string subKey)
    {
        float findValue = float.MinValue;
        string val = ToStr(key, subKey);
        if (val != string.Empty)
        {
            if (!float.TryParse(val, out findValue))
            {
                findValue = float.MinValue;
            }
        }
        return findValue;
    }

    public bool ToBool(int key, string subKey)
    {
        bool findValue = false;
        string val = ToStr(key, subKey);
        if(val.CompareTo("True") == 0)
        {
            findValue = true;
        }
        return findValue;
    }

}

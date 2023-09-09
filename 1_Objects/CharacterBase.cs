using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    protected string _name;         // ÀÌ¸§
    protected int _str;             // Èû
    protected int _con;             // Ã¼·Â
    protected int _dex;             // ¹ÎÃ¸
    protected int _int;             // Áö´É
    protected int _wis;             // ÁöÇı

    public bool _isDead
    {
        get; set;
    }

    public abstract void InitializeDataSet(string na, int s, int c, int d, int i, int w, int sub1, int sub2);

    public abstract void Hitting(int finishDam);
}

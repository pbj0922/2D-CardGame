using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    protected string _name;         // �̸�
    protected int _str;             // ��
    protected int _con;             // ü��
    protected int _dex;             // ��ø
    protected int _int;             // ����
    protected int _wis;             // ����

    public bool _isDead
    {
        get; set;
    }

    public abstract void InitializeDataSet(string na, int s, int c, int d, int i, int w, int sub1, int sub2);

    public abstract void Hitting(int finishDam);
}

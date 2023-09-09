using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumHelper;

public class MiniMonStatusWnd : MonoBehaviour
{
    [SerializeField] Text _txtName;
    [SerializeField] Image _barFill;

    Transform _camTF;
    float _visibleTime = 3;
    float _timeCheck = 0;

    void Update()
    {
        transform.LookAt(_camTF);
    }

    public void InitSet(string name, eGradeType grade, float rate = 1)
    {
        _camTF = Camera.main.transform;
        _txtName.text = name;
        _txtName.color = ResPoolManager._instance.GetGradeColor(grade);
        _barFill.fillAmount = rate;
        ShowWindow(false);
    }

    public void SettingHpRate(float rate)
    {
        ShowWindow(true);
        _timeCheck = 0;
        _barFill.fillAmount = rate;
    }

    public void ShowWindow(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
}

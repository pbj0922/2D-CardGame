using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleStatusBox : MonoBehaviour
{
    [SerializeField] Text _name;
    [SerializeField] Text _level;
    [SerializeField] Image _HPBar;
    [SerializeField] Image _ManaBar;
    [SerializeField] Image _ExpBar;

    public void InitDataSet(string name, int level, float HPrate = 1, float Manarate = 1, float Exprate = 1)
    {
        _name.text = name;
        _level.text = string.Format("Lv.<color=#FFFF00>99</color>>", level);
        _HPBar.fillAmount = HPrate;
        _ManaBar.fillAmount = Manarate;
        _ExpBar.fillAmount = Exprate;
        ShowWindow(true);
    }

    public void ShowWindow(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartProgram : MonoBehaviour
{
    void Start()
    {
        // �ӽ�
        TableManager._instance.LoadTable();
        SceneControlManager._instance.StartIngameScene();
        ResPoolManager._instance.EssentialLoad();
        //===
    }
}

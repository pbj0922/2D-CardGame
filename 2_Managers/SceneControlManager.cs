using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EnumHelper;

public class SceneControlManager : TSingleton<SceneControlManager>
{
    protected override void Init()
    {
        base.Init();

        _prevScene = _currScene = SceneType.none;
    }

    SceneType _prevScene;
    SceneType _currScene;

    public void StartIngameScene(int stageNum = 1)
    {
        _prevScene = _currScene;
        _currScene = SceneType.IngameScene;
        StartCoroutine(StartLoadScene(_currScene.ToString(), stageNum));
    }

    IEnumerator StartLoadScene(string sceneName, int stageNum)
    {
        Scene active;

        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        while (!ao.isDone)
        {
            yield return null;
        }

        if(_currScene == SceneType.IngameScene)
        {
            string mapName = "Map" + stageNum.ToString();

            ao = SceneManager.LoadSceneAsync(mapName, LoadSceneMode.Additive);
            while (!ao.isDone)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
            active = SceneManager.GetSceneByName(mapName);
            SceneManager.SetActiveScene(active);
        }
    }
}

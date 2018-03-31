﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏入口:游戏启动器类
/// </summary>
public class GameLauncher : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject fpsHelperObj;
    private FPSHelper fpsHelper;
    private LogHelper logHelper;
    private InputMgr inputMgr;

    void Awake()
    {
        gameManager = GameManager.GetInstance();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        DontDestroyOnLoad(gameObject);

        //加入输入输出管理器
        inputMgr = gameObject.AddComponent<InputMgr>();

#if SHOW_FPS
        fpsHelperObj = new GameObject("FpsHelperObj");
        fpsHelper = fpsHelperObj.AddComponent<FPSHelper>();
        GameObject.DontDestroyOnLoad(fpsHelperObj);
#endif

#if BUILD_DEBUG_LOG || UNITY_EDITOR
#if UNITY_2017
        Debug.unityLogger.logEnabled = true;
#else
        Debug.logger.logEnabled = true;
#endif
#else
#if UNITY_2017
        Debug.unityLogger.logEnabled = false;
#else
        Debug.logger.logEnabled = false;
#endif
#endif

#if WETEST_SDK
        gameObject.AddComponent<WeTest.U3DAutomation.U3DAutomationBehaviour>();
#endif

#if OUTPUT_LOG
        GameObject logHelperObj = new GameObject("LogHelperObj");
        logHelper = logHelperObj.AddComponent<LogHelper>();
        GameObject.DontDestroyOnLoad(logHelperObj);

        Application.logMessageReceived += logHelper.LogCallback;
#endif
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(InitGameCore());
    }

    void Update()
    {
        gameManager.Update(Time.deltaTime);
    }

    private void LateUpdate()
    {
        gameManager.LateUpdate(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        gameManager.FixedUpdate(Time.fixedDeltaTime);
    }


    private void OnApplicationQuit()
    {
        gameManager.OnApplicationQuit();
    }

    private void OnApplicationPause(bool pause)
    {
        gameManager.OnApplicationPause(pause);
    }

    private void OnApplicationFocus(bool focus)
    {
        gameManager.OnApplicationFocus(focus);
    }

    IEnumerator InitGameCore()
    {
        yield return new WaitForEndOfFrame();
        gameManager.InitGameCore(gameObject);
    }
}

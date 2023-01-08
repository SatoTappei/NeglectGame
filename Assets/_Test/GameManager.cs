using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームマネージャー: シングルトン
/// </summary>
public class GameManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
#if UNITY_EDITOR
        // シーンの再読み込みを行う
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
#endif
    }
}

/// <summary>
/// エディタ上でのみ呼び出せるデバッグログのクラス
/// </summary>
public static class Debug
{
    const string Symbol = "EDITOR_ONLY_DEBUG_LOG";

    [Conditional(Symbol)]
    public static void Log(object message) => UnityEngine.Debug.Log(message);
    [Conditional(Symbol)]
    public static void LogWarning(object message) => UnityEngine.Debug.LogWarning(message);
    [Conditional(Symbol)]
    public static void LogError(object message) => UnityEngine.Debug.LogError(message);
}

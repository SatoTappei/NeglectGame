using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.SceneManagement;

/// <summary>
/// �Q�[���}�l�[�W���[: �V���O���g��
/// </summary>
public class GameManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
#if UNITY_EDITOR
        // �V�[���̍ēǂݍ��݂��s��
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
#endif
    }
}

/// <summary>
/// �G�f�B�^��ł̂݌Ăяo����f�o�b�O���O�̃N���X
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

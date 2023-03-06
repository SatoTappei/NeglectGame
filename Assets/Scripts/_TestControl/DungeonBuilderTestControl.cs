using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ダンジョン生成単体のテスト用制御
/// </summary>
public class DungeonBuilderTestControl : MonoBehaviour
{
    [SerializeField] DungeonBuilder _dungeonBuilder;

    void Start()
    {
        _dungeonBuilder.Build();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

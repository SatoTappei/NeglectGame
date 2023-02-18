using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAgent : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public Vector3 PickLocationInRange(float searchRange)
    {
        // 指定した範囲内でロケーションを探す処理
        return Vector3.one;
    }

    public void MoveTo(Vector3 location)
    {
        // ロケーションに移動する処理
    }

    // 目的地に着いたかどうか？
    public bool AtDestination => true;
}

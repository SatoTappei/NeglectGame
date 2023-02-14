using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    /* 
     *  Awake()とOnEnable()の後、Start()の前でGenerateDecoratorコンポーネントによって初期化される。
     */

    void Start()
    {
        Debug.Log("Startが呼ばれた");

    }

    void Update()
    {
        
    }
}
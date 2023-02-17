using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̓����UI����ɃJ���������Ɍ�����R���|�[�l���g
/// </summary>
public class BillboardUI : MonoBehaviour
{
    Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        Vector3 p = _mainCamera.transform.position;
        p.x = transform.position.x;
        transform.LookAt(p);
        //transform.rotation = _mainCamera.transform.rotation;
    }
}

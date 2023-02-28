using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラの操作を行うコンポーネント
/// </summary>
public class VCamController : MonoBehaviour
{
    static readonly float CameraPosBorder = 37.0f;

    [Header("VCamがFollowしているオブジェクト")]
    [SerializeField] Transform _target;
    [SerializeField] Transform _targetParent;
    [Header("カメラの移動速度")]
    [SerializeField] int _moveSpeed = 30;
    [Header("カメラの回転速度")]
    [SerializeField] int _rotateSpeed = 30;

    void Update()
    {
        float hori = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _target.Rotate(new Vector3(0, -hori, 0) * Time.deltaTime * _rotateSpeed);
        }
        else
        {
            Vector3 velo = (_target.forward * vert + _target.right * hori).normalized;
            Vector3 next = _targetParent.position + velo * _moveSpeed * Time.deltaTime;

            if (Mathf.Abs(next.x) <= CameraPosBorder &&
                Mathf.Abs(next.z) <= CameraPosBorder)
            {
                _targetParent.transform.position = next;
            }
        }
    }
}

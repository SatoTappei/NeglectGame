using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �J�����̑�����s���R���|�[�l���g
/// </summary>
public class VCamController : MonoBehaviour
{
    static readonly float CameraPosBorder = 37.0f;

    [Header("VCam��Follow���Ă���I�u�W�F�N�g")]
    [SerializeField] Transform _target;
    [SerializeField] Transform _targetParent;
    [Header("�J�����̈ړ����x")]
    [SerializeField] int _moveSpeed = 30;
    [Header("�J�����̉�]���x")]
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

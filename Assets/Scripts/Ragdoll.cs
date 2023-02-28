using UnityEngine;
using DG.Tweening;

/// <summary>
/// �L�����N�^�[�����񂾂Ƃ��ɐ�������郉�O�h�[���𐧌䂷��
/// </summary>
public class Ragdoll : MonoBehaviour
{
    static readonly float Delay = 3.0f;
    static readonly float LifeTime = 10.0f;

    [SerializeField] Rigidbody _rb;
    [Header("������͂̋���")]
    [SerializeField] float _power = 200;
    [Header("�Ԃ���񂾌�Ɏ~�߂�Rigidbody")]
    [SerializeField] Rigidbody[] _rbs;

    void Start()
    {
        float dirX = -transform.forward.x;
        float dirZ = -transform.forward.z;
        Vector3 dir = new Vector3(dirX, 1, dirZ).normalized;

        _rb.AddForce(dir * _power, ForceMode.Impulse);

        DOVirtual.DelayedCall(Delay, () => 
        {
            foreach(Rigidbody rb in _rbs)
            {
                rb.isKinematic = true;
            }

            Destroy(gameObject, LifeTime);
        }).SetLink(gameObject);
    }
}

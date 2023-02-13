using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �U�����󂯂��ۂɃ_���[�W�̃��b�Z�[�W���O���s���R���|�[�l���g
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class DamagedMessageSender : MonoBehaviour
{
    static readonly string DamageableObjectTag = "Weapon";
    /// <summary>��{�I�ɐ퓬�̓^�C�}���Ȃ̂�1�ő��v</summary>
    static readonly int ResultsLength = 1;

    [SerializeField] ActorHpControl _actorHpControl;
    [Header("�󂯂�_���[�W")]
    [Tooltip("�����U���͂̊T�O�������̂Ń_���[�W���󂯂鑤�Ń_���[�W�ʂ����߂�")]
    [SerializeField] int _minDamage;
    [SerializeField] int _maxDamage;
    [Header("���S�����ۂɐ퓬�I����ʒm���鋗��")]
    [SerializeField] float _sendRadius;
    [Header("���S�����ۂɐ퓬�I���̏�����ʒm���郌�C���[")]
    [Tooltip("�G�Ȃ�L�����N�^�[�ɁA�L�����N�^�[�Ȃ�G�ɐ퓬�I���̏�����ʒm����")]
    [SerializeField] LayerMask _sendLayer;

    Collider[] _results = new Collider[ResultsLength];

    void Awake()
    {
        // �~�X�h�~�̂���RigidBody�̊e�퍀�ڂ��������ŘM��
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.mass = 1;
        rb.angularDrag = 0;
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(DamageableObjectTag))
        {
            int value = Random.Range(_minDamage, _maxDamage + 1);

            // HpControl�R���|�[�l���g�ɉ��_���[�W�󂯂�����n���ď������Ă��炤
            // �������S��������͂̑Ώۂ̐퓬�I���̏������Ă�
            _actorHpControl.DecreaseHp(value);

            if (_actorHpControl.IsHpEqualZero())
            {
                Physics.OverlapSphereNonAlloc(transform.position, _sendRadius, _results, _sendLayer);
                foreach(Collider c in _results)
                {
                    Debug.Log(c.gameObject.name +" �̐퓬�I���̏������Ă�");
                    c.GetComponent<DamagedMessageReceiver>()?.OnDefeated?.Invoke();
                }
            }
        }
    }
}

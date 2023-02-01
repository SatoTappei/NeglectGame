using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̎��E�𐧌䂷��R���|�[�l���g
/// </summary>
public class ActorSight : MonoBehaviour
{
    readonly int SightableArrLength = 4;
    // �L�����N�^�[���f���̓��ォ��Ray���΂���悤�ɐݒ肷��
    readonly float ActorModelHeight = 1.5f;

    [SerializeField] Transform _actorModel;
    [Header("�L�����N�^�[�̎��E�ɉf�郌�C���[")]
    [SerializeField] LayerMask _sightableLayer;
    [Header("�L�����N�^�[�̎��E���Օ����郌�C���[")]
    [SerializeField] LayerMask _sightBlockableLayer;
    [Header("���E�̃p�����[�^��ݒ�")]
    [SerializeField] float _sightRange;
    [SerializeField] float _sightAngle;

    // OverlapSphereNonAlloc()���g�p����̂ŗ\�ߊi�[�p�̔z����m�ۂ��Ă���
    Collider[] _sightableArr;

    void Awake()
    {
        _sightableArr = new Collider[SightableArrLength];
    }

    void Update()
    {
        if (IsFindTreasure())
        {
            Debug.Log("�����");
        }
        else
        {
            Debug.Log("�̂񂿂��");
        }
    }

    internal bool IsFindTreasure()
    {
        Physics.OverlapSphereNonAlloc(transform.position, _sightRange, _sightableArr, _sightableLayer);

        foreach (Collider col in _sightableArr)
        {
            if (!col) break;

            Vector3 modelForward = _actorModel.forward;
            Vector3 treasurePos = col.gameObject.transform.position;
            Vector3 treasureDir = (treasurePos - _actorModel.position).normalized;

            float distance = Vector3.Distance(treasurePos, _actorModel.position);
            float angle = Vector3.Angle(modelForward, treasureDir);

            // �Ώۂ܂�Ray���΂��ăq�b�g���Ȃ������� �̔��肾�ƑΏۂ������ȏ�ǂɖ��܂��Ă����false���Ԃ�
            Vector3 rayOrigin = _actorModel.transform.position;
            rayOrigin.y += ActorModelHeight;
            bool isUnobstructed = !Physics.Raycast(rayOrigin, treasureDir, distance, _sightBlockableLayer);

            return distance <= _sightRange && angle <= _sightAngle && isUnobstructed;
        }

        return false;
    }
}

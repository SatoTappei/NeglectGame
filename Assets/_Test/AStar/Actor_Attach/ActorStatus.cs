using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// �L�����N�^�[�̓����X�e�[�^�X��\���R���|�[�l���g�H
/// </summary>
public class ActorStatus : MonoBehaviour
{
    readonly int SightableArrLength = 4;

    [Header("�ӗ~�̍ő�l")]
    [SerializeField] int _maxMotivation;
    [Header("�ӗ~�̌�����(�b)")]
    [SerializeField] int _decreaseQuantity;
    [Header("�ӗ~�������Ɣ��肳���臒l")]
    [SerializeField] int _motivationThreshold;
    [Header("���E�Ƃ��Ďg������̓����蔻��̐ݒ�")]
    [SerializeField] LayerMask _mask;
    [SerializeField] float _overlapSphereRadius;
    // �e�X�g�I�ɂ�����Model�ւ̎Q�Ƃ��Ƃ�
    [SerializeField] Transform _model;

    int _motivation;
    // OverlapSphereNonAlloc()���g�p����̂ŗ\�ߊi�[�p�̔z����m�ۂ��Ă���
    Collider[] _sightableArr;

    // �m�F���g�p���邽�߂̃e�X�g�p��UI
    [SerializeField] Text _text;
    // ���E�̃e�X�g�̂��߂̃}�e���A��
    [SerializeField] Material _testMat;
    [SerializeField] Material _defMat;

    void Awake()
    {
        _motivation = _maxMotivation;
        _sightableArr = new Collider[SightableArrLength];
    }

    void Start()
    {
        InvokeRepeating(nameof(DecreaseMotivation), 0, 1);
    }

    void Update()
    {
        IsFindTreasure();
    }

    internal bool IsBelowMotivationThreshold() => _motivation < _motivationThreshold;

    void DecreaseMotivation()
    {
        Mathf.Clamp(_motivation -= _decreaseQuantity, 0, _maxMotivation);

        // �e�X�g�p��UI�ɕ\��
        _text.text = _motivation.ToString();
    }

    internal bool IsFindTreasure()
    {
        // ����΂��Ă���Ƀq�b�g������Ȃ񂩌����������Ƃ݂Ȃ�
        // ���t���[���Ăяo���Əd���̂�0.1�b���Ƃɂ���
        Physics.OverlapSphereNonAlloc(transform.position, _overlapSphereRadius, _sightableArr, _mask);

        foreach(Collider v in _sightableArr)
        {
            if (v == null) break;

            if (_defMat == null) _defMat = v.GetComponent<MeshRenderer>().material;

            Vector3 f = _model.transform.forward;
            Vector3 t = v.gameObject.transform.position;
            Vector3 diff = t - _model.transform.position;
            Vector3 d = v.gameObject.transform.position - _model.transform.position;
            float rad = 45 * Mathf.Deg2Rad;
            float theta = Mathf.Cos(rad);

            bool b = IsWithinRangeAngle(f, diff, theta);

            Vector3 rv = _model.transform.position;
            rv.y += 1;
            bool bb = Physics.Raycast(rv, d, 20);

            if (!bb && b && d.sqrMagnitude <20)
            {
                v.GetComponent<MeshRenderer>().material = _testMat;
            }
            else
            {
                v.GetComponent<MeshRenderer>().material = _defMat;
            }
        }

        // �擾�������g���W���[�̓Q�b�g�ł����B
        // Physics�̕������Z�����𒼐ڌĂяo���̂ŃR���C�_�[�ƃ��W�{�͕K�v�Ȃ�
        // ���Ƃ͂�������C���[�ōi�荞�ޕK�v������B
        // ���̂��ƂɊp�x�Ő��ɍi�荞��
        // �i�荞�񂾒��Ƀg���W���[�������true

        return false;
    }

    bool IsWithinRangeAngle(Vector3 i_forwardDir, Vector3 i_toTargetDir, float i_cosTheta)
    {
        // �����x�N�g���������ꍇ�́A���ʒu�ɂ�����̂��Ɣ��f����B
        if (i_toTargetDir.sqrMagnitude <= Mathf.Epsilon)
        {
            return true;
        }

        float dot = Vector3.Dot(i_forwardDir, i_toTargetDir);
        return dot >= i_cosTheta;
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(other.gameObject.name);
    //}
}

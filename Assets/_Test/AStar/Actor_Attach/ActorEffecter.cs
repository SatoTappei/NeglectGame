using UnityEngine;

/// <summary>
/// �L�����N�^�[�̎��͂̃I�u�W�F�N�g�𑀍삷��R���|�[�l���g
/// </summary>
public class ActorEffecter : MonoBehaviour
{
    // ���͂̑���\�ȃI�u�W�F�N�g�̐��ɉ����đ��₷
    readonly int ResultsLength = 4;

    [Header("����\�Ȕ͈�")]
    [SerializeField] float _effectRange = 5;
    [Header("���E�ɉf��I�u�W�F�N�g�̃��C���[")]
    [SerializeField] LayerMask _effectLayer = 8;

    Collider[] _results;

    void Awake()
    {
        _results = new Collider[ResultsLength];
    }

    internal void EffectAround()
    {
        Physics.OverlapSphereNonAlloc(transform.position, _effectRange, _results, _effectLayer);

        // TODO:���̏����Ƃ��Ď擾�����Ώۂ�j�����鏈���A���ۂ̓C���^�[�t�F�[�X���o�R����
        Destroy(_results[0].gameObject);
        Debug.Log("����");
    }
}

using UnityEngine;

/// <summary>
/// �L�����N�^�[�̎��͂̃I�u�W�F�N�g�𑀍삷��R���|�[�l���g
/// </summary>
public class ActorEffecter : MonoBehaviour
{
    /// <summary>���͂̑���\�ȃI�u�W�F�N�g�̐��ɉ����đ��₷</summary>
    static readonly int ResultsLength = 4;

    [Header("����\�Ȕ͈�")]
    [SerializeField] float _effectRadius = 5;
    [Header("���E�ɉf��I�u�W�F�N�g�̃��C���[")]
    [SerializeField] LayerMask _effectLayer = 8;

    Collider[] _results = new Collider[ResultsLength];

    internal void EffectAround()
    {
        Physics.OverlapSphereNonAlloc(transform.position, _effectRadius, _results, _effectLayer);

        foreach (Collider collider in _results)
        {
            if (collider == null) break;
            //collider.gameObject.GetComponent<IEffectable>().EffectByActor();
        }
    }
}

using UnityEngine;

/// <summary>
/// 㩂̃R���|�[�l���g
/// </summary>
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Trap : MonoBehaviour
{
    static readonly string ParticlePoolTag = "ParticlePool";

    [SerializeField] GameObject _particlePrefab;

    GameObject _particle;
    Transform _particlePool;

    void Start()
    {
        _particlePool = GameObject.FindGameObjectWithTag(ParticlePoolTag).transform;
        _particle = Instantiate(_particlePrefab, transform.position, Quaternion.identity, _particlePool);
        _particle.SetActive(false);
    }

    void Update()
    {
        /* ���݂̏������Ƒ�ʂ�㩂���������Ă��܂��̂Ō������ǂ��Ȃ�
         * �ŏ��ɋK�萔�������Ă����A�N���b�N�̌��Ɉړ�������
         * �L���[�ŕۑ����Ă����Ɨǂ�����
         */
    }

    void OnTriggerEnter(Collider other)
    {
        ActorHpControl actorHpControl = other.gameObject.GetComponentInParent<ActorHpControl>();

        if (actorHpControl != null)
        {
            // �L�����N�^�[��HP�����炷����
            _particle.SetActive(true);
            Debug.Log("����");
            transform.localScale = Vector3.zero;
        }
    }
}

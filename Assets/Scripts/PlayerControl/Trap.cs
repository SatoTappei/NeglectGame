using UnityEngine;

/// <summary>
/// 㩂̃R���|�[�l���g
/// </summary>
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Trap : MonoBehaviour
{
    static readonly string ParticlePoolTag = "ParticlePool";

    [Header("�_���[�W��")]
    [SerializeField] int _damage;
    [Header("Particle�̃v���n�u")]
    [SerializeField] GameObject _particlePrefab;

    GameObject _particle;
    Transform _particlePool;

    void Start()
    {
        _particlePool = GameObject.FindGameObjectWithTag(ParticlePoolTag).transform;
        _particle = Instantiate(_particlePrefab, transform.position, Quaternion.identity, _particlePool);
        _particle.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        ActorHpControl actorHpControl = other.gameObject.GetComponentInParent<ActorHpControl>();
        if (actorHpControl == null) return;

        _particle.transform.position = transform.position;
        _particle.SetActive(true);

        actorHpControl.Damage(_damage);
    }
}

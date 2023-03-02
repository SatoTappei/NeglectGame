using UnityEngine;

/// <summary>
/// 罠のコンポーネント
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
        /* 現在の処理だと大量の罠が生成されてしまうので効率が良くない
         * 最初に規定数生成しておき、クリックの個所に移動させる
         * キューで保存しておくと良い感じ
         */
    }

    void OnTriggerEnter(Collider other)
    {
        ActorHpControl actorHpControl = other.gameObject.GetComponentInParent<ActorHpControl>();

        if (actorHpControl != null)
        {
            // キャラクターのHPを減らす処理
            _particle.SetActive(true);
            Debug.Log("踏み");
            transform.localScale = Vector3.zero;
        }
    }
}

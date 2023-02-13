using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃を受けた際にダメージのメッセージングを行うコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class DamagedMessageSender : MonoBehaviour
{
    static readonly string DamageableObjectTag = "Weapon";
    /// <summary>基本的に戦闘はタイマンなので1で大丈夫</summary>
    static readonly int ResultsLength = 1;

    [SerializeField] ActorHpControl _actorHpControl;
    [Header("受けるダメージ")]
    [Tooltip("武器や攻撃力の概念が無いのでダメージを受ける側でダメージ量を決める")]
    [SerializeField] int _minDamage;
    [SerializeField] int _maxDamage;
    [Header("死亡した際に戦闘終了を通知する距離")]
    [SerializeField] float _sendRadius;
    [Header("死亡した際に戦闘終了の処理を通知するレイヤー")]
    [Tooltip("敵ならキャラクターに、キャラクターなら敵に戦闘終了の処理を通知する")]
    [SerializeField] LayerMask _sendLayer;

    Collider[] _results = new Collider[ResultsLength];

    void Awake()
    {
        // ミス防止のためRigidBodyの各種項目をこっちで弄る
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

            // HpControlコンポーネントに何ダメージ受けたかを渡して処理してもらう
            // もし死亡したら周囲の対象の戦闘終了の処理を呼ぶ
            _actorHpControl.DecreaseHp(value);

            if (_actorHpControl.IsHpEqualZero())
            {
                Physics.OverlapSphereNonAlloc(transform.position, _sendRadius, _results, _sendLayer);
                foreach(Collider c in _results)
                {
                    Debug.Log(c.gameObject.name +" の戦闘終了の処理を呼ぶ");
                    c.GetComponent<DamagedMessageReceiver>()?.OnDefeated?.Invoke();
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

/// <summary>
/// ダメージを受けるオブジェクト(仮)
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class DamageReceiver : MonoBehaviour
{
    /* 
     *  TODO:MessagePipeを使ったメッセージングに直せないか検討する
     *       そうすることでりじぼがいらないくなる
     */

    int _hp = 9;

    void Awake()
    {
        // ミス防止のためりじぼの各種項目をこっちで弄る
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.mass = 1;
        rb.angularDrag = 0;
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    void OnTriggerEnter(Collider other)
    {
        // TOOD:一度の攻撃で複数回のヒット処理をしてしまうのでintervalを設けたりする必要がある
        if (other.gameObject.CompareTag("Weapon"))
        {
            _hp--;
            Debug.Log("ヒット");
            if (_hp == 0)
            {
                MessageBroker.Default.Publish(new DamageData());

                Destroy(gameObject);

            }
        }
    }

    void OnDestroy()
    {
        // 死んだときにMessagePipeを用いてメッセージを発行する
        // ダメージを与えるコンポネ＆メソッドをつくらないとりじぼがいる
    }
}

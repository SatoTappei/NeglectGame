using UnityEngine;

/// <summary>
/// 敵のプレハブを制御するコンポーネント
/// </summary>
public class Enemy : MonoBehaviour, IEffectable
{
    [SerializeField] Animator _anim;
    [SerializeField] SightableObject _sightableObject;
    [Header("キャラクターの視線のRayがヒットするコライダー")]
    [SerializeField] Collider _rayHitCollider;
    [Header("攻撃アニメーションを再生する時間")]
    [SerializeField] float _playingAnimationTime = 4.0f;
    [Header("キャラクターが再び視認できるようになるまでの時間")]
    [SerializeField] float _visibleAgainTime = 8.0f;

    ///// <summary>複数のキャラクターに視認されないようにするためのフラグ</summary>
    //bool _isAvailable = true;

    //public bool IsAvailable => _isAvailable;

    void OnEnable()
    {
        _sightableObject.OnSelectedMovingTarget += InactiveCollider;
    }

    void OnDisable()
    {
        _sightableObject.OnSelectedMovingTarget -= InactiveCollider;
    }

    void InactiveCollider()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    ///// <summary>視認可能かどうかの切り替えはActor側のRayによる取得で行う</summary>
    //public void SetUnAvailable() => _isAvailable = false;

    void IEffectable.Effect(string message)
    {
        if (message == "ActorWin")
        {
            // 攻撃アニメーション再生
            // _playingAnimationTime秒後
            // アニメーション停止＆Destroy＆演出
        }
        else if (message == "ActorLose")
        {
            // 攻撃アニメーション再生
            // _playingAnimationTime秒後
            // アニメーション停止
            // _visibleAgainTime秒後、再び視認可能になる
        }
        else
        {
            Debug.LogWarning("Enemyでは処理できないメッセージです: " + message);
        }
    }

    

    // キャラクターの視線がヒットしたらコライダーオフ
    // 課題:アニメーションの開始と終了のタイミング
    // 発見=>ダッシュ=>敵の真ん前に来たら攻撃開始
    // 攻撃開始はコライダーがヒットしたらでおｋ
    // 攻撃終了のタイミング is 秒でこっちが指定する
    // ノードで文字列型のメッセージング

    // 勝った場合と負けた場合がある
    // キャラクターが勝つ場合はぶっ飛ぶ
    // キャラクターが負ける場合は再びコライダーがアクティブ化しないといけない
}

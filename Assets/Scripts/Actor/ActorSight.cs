using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターの視界を制御するコンポーネント
/// </summary>
public class ActorSight : MonoBehaviour
{
    /// <summary>視界の更新間隔、間隔を広くすると視界のパラメータによっては認識しなくなる</summary>
    static readonly float UpdateInterval = 0.25f;
    /// <summary>視界に映る周囲のオブジェクト数に応じて設定する</summary>
    static readonly int InSightCap = 4;
    /// <summary>頭上からRayを飛ばすためにキャラクターのモデルの高さに応じて設定する</summary>
    static readonly float ActorModelHeight = 1.5f;

    [Header("視線のRayを飛ばすキャラクターモデル")]
    [SerializeField] Transform _actorModel;
    [Header("キャラクターの視界に映るレイヤー")]
    [SerializeField] LayerMask _sightableLayer;
    [Header("キャラクターの視界を遮蔽するレイヤー")]
    [SerializeField] LayerMask _sightBlockableLayer;
    [Header("視界のパラメータを設定")]
    [SerializeField] float _sightRange = 5.0f;
    [SerializeField] float _sightAngle = 60.0f;

    Collider[] _results = new Collider[InSightCap];
    Queue<SightableObject> _inSightObjectQueue = new(InSightCap);

    internal Queue<SightableObject> InSightObjectQueue => _inSightObjectQueue;

    public void StartLookInSight() => InvokeRepeating(nameof(LookInSight), 0, UpdateInterval);
    public void StopLookInSight() => CancelInvoke(nameof(LookInSight));
    public void ResetLookInSight() => _inSightObjectQueue.Clear();

    void LookInSight()
    {
        Physics.OverlapSphereNonAlloc(transform.position, _sightRange, _results, _sightableLayer);

        // デバッグ用:キャラの前方への視界のRay
        Vector3 r = _actorModel.transform.position;
        r.y += ActorModelHeight;
        Vector3 f = _actorModel.transform.forward;
        UnityEngine.Debug.DrawRay(r, f * _sightRange, Color.blue, 0.1f, false);
        // キャラの前方への視界のRayここまで

        // 複数のオブジェクトを見つけた場合は最初の1つが返る
        foreach (Collider rangeInSide in _results)
        {
            // 定期的に呼び出しているので何も視界にない場合はbreakする
            if (rangeInSide == null) break;

            Vector3 rangeInSidePos = rangeInSide.gameObject.transform.position;
            Vector3 rangeInSideDir = (rangeInSidePos - _actorModel.position).normalized;

            float distance = Vector3.Distance(rangeInSidePos, _actorModel.position);
            float angle = Vector3.Angle(_actorModel.forward, rangeInSideDir);

            // モデルの頭上から水平にRayを飛ばしているのでモデルより低い障害物は無視される
            // 対象までRayを飛ばしてヒットしなかったら の判定だと対象が半分以上壁に埋まっているとfalseが返る
            Vector3 rayOrigin = _actorModel.transform.position;
            rayOrigin.y += ActorModelHeight;
            bool dontBlocked = !Physics.Raycast(rayOrigin, rangeInSideDir, distance, _sightBlockableLayer);

            if (distance <= _sightRange && angle <= _sightAngle && dontBlocked)
            {
                _inSightObjectQueue.Enqueue(rangeInSide.GetComponent<SightableObject>());
            }
        }
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, _sightRange);
    //}
}
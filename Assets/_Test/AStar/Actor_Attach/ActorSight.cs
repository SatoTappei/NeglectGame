using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターの視界を制御するコンポーネント
/// </summary>
public class ActorSight : MonoBehaviour
{
    readonly int SightableArrLength = 4;
    // キャラクターモデルの頭上からRayを飛ばせるように設定する
    readonly float ActorModelHeight = 1.5f;

    [Header("視線のRayを飛ばすキャラクターモデル")]
    [SerializeField] Transform _actorModel;
    [Header("キャラクターの視界に映るレイヤー")]
    [SerializeField] LayerMask _sightableLayer;
    [Header("キャラクターの視界を遮蔽するレイヤー")]
    [SerializeField] LayerMask _sightBlockableLayer;
    [Header("視界のパラメータを設定")]
    [SerializeField] float _sightRange;
    [SerializeField] float _sightAngle;

    ActorStatus _actorStatus;
    // OverlapSphereNonAlloc()を使用するので予め格納用の配列を確保しておく
    Collider[] _sightableArr;

    internal void Init(ActorStatus actorStatus)
    {
        _actorStatus = actorStatus;
        _sightableArr = new Collider[SightableArrLength];
    }

    /// <summary>複数のオブジェクトを見つけた場合は最初の1つが返る</summary>
    internal GameObject GetInSightObject()
    {
        Physics.OverlapSphereNonAlloc(transform.position, _sightRange, _sightableArr, _sightableLayer);

        foreach (Collider col in _sightableArr)
        {
            if (!col) break;

            Vector3 modelForward = _actorModel.forward;
            Vector3 treasurePos = col.gameObject.transform.position;
            Vector3 treasureDir = (treasurePos - _actorModel.position).normalized;

            float distance = Vector3.Distance(treasurePos, _actorModel.position);
            float angle = Vector3.Angle(modelForward, treasureDir);

            // 対象までRayを飛ばしてヒットしなかったら の判定だと対象が半分以上壁に埋まっているとfalseが返る
            Vector3 rayOrigin = _actorModel.transform.position;
            rayOrigin.y += ActorModelHeight;
            bool isUnobstructed = !Physics.Raycast(rayOrigin, treasureDir, distance, _sightBlockableLayer);

            if(distance <= _sightRange && angle <= _sightAngle && isUnobstructed)
            {
                return col.gameObject;
            }
        }

        return null;
    }
}

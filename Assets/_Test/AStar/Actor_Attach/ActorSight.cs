using UnityEngine;

/// <summary>
/// キャラクターの視界を制御するコンポーネント
/// </summary>
public class ActorSight : MonoBehaviour
{
    // 周囲の視界に映るオブジェクトの数に応じて増やす
    readonly int ResultsLength = 4;
    // キャラクターモデルの頭上からRayを飛ばせるように設定する
    readonly float ActorModelHeight = 1.5f;

    [Header("視線のRayを飛ばすキャラクターモデル")]
    [SerializeField] Transform _actorModel;
    [Header("キャラクターの視界に映るレイヤー")]
    [SerializeField] LayerMask _sightableLayer;
    [Header("キャラクターの視界を遮蔽するレイヤー")]
    [SerializeField] LayerMask _sightBlockableLayer;
    [Header("視界の更新間隔")]
    [SerializeField] float _updateDuration = 0.25f;
    [Header("視界のパラメータを設定")]
    [SerializeField] float _sightRange = 5.0f;
    [SerializeField] float _sightAngle = 60.0f;

    Collider[] _results;
    GameObject _currentInSightObject;

    public GameObject CurrentInSightObject => _currentInSightObject;

    void Awake()
    {
        _results = new Collider[ResultsLength];
    }

    void Start()
    {
        StartInSight();
    }

    void StartInSight() => InvokeRepeating(nameof(InSightObject), 0, _updateDuration);
    void StopInSight() { /* 視界の更新を止める処理 */ }
    internal bool IsFindInSight() => _currentInSightObject != null;

    /// <summary>複数のオブジェクトを見つけた場合は最初の1つが返る</summary>
    void InSightObject()
    {
        Physics.OverlapSphereNonAlloc(transform.position, _sightRange, _results, _sightableLayer);

        foreach (Collider col in _results)
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
                _currentInSightObject = col.gameObject;
            }
        }
    }
}

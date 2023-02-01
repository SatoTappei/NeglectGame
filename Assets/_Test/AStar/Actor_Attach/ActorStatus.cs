using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// キャラクターの内部ステータスを表すコンポーネント？
/// </summary>
public class ActorStatus : MonoBehaviour
{
    readonly int SightableArrLength = 4;

    [Header("意欲の最大値")]
    [SerializeField] int _maxMotivation;
    [Header("意欲の減少量(秒)")]
    [SerializeField] int _decreaseQuantity;
    [Header("意欲が無いと判定される閾値")]
    [SerializeField] int _motivationThreshold;
    [Header("視界として使う球状の当たり判定の設定")]
    [SerializeField] LayerMask _mask;
    [SerializeField] float _overlapSphereRadius;
    // テスト的にここでModelへの参照をとる
    [SerializeField] Transform _model;

    int _motivation;
    // OverlapSphereNonAlloc()を使用するので予め格納用の配列を確保しておく
    Collider[] _sightableArr;

    // 確認し使用するためのテスト用のUI
    [SerializeField] Text _text;
    // 視界のテストのためのマテリアル
    [SerializeField] Material _testMat;
    [SerializeField] Material _defMat;

    void Awake()
    {
        _motivation = _maxMotivation;
        _sightableArr = new Collider[SightableArrLength];
    }

    void Start()
    {
        InvokeRepeating(nameof(DecreaseMotivation), 0, 1);
    }

    void Update()
    {
        IsFindTreasure();
    }

    internal bool IsBelowMotivationThreshold() => _motivation < _motivationThreshold;

    void DecreaseMotivation()
    {
        Mathf.Clamp(_motivation -= _decreaseQuantity, 0, _maxMotivation);

        // テスト用にUIに表示
        _text.text = _motivation.ToString();
    }

    internal bool IsFindTreasure()
    {
        // 例を飛ばしてお宝にヒットしたらなんか見っけったとみなす
        // 毎フレーム呼び出すと重いので0.1秒ごとにする
        Physics.OverlapSphereNonAlloc(transform.position, _overlapSphereRadius, _sightableArr, _mask);

        foreach(Collider v in _sightableArr)
        {
            if (v == null) break;

            if (_defMat == null) _defMat = v.GetComponent<MeshRenderer>().material;

            Vector3 f = _model.transform.forward;
            Vector3 t = v.gameObject.transform.position;
            Vector3 diff = t - _model.transform.position;
            Vector3 d = v.gameObject.transform.position - _model.transform.position;
            float rad = 45 * Mathf.Deg2Rad;
            float theta = Mathf.Cos(rad);

            bool b = IsWithinRangeAngle(f, diff, theta);

            Vector3 rv = _model.transform.position;
            rv.y += 1;
            bool bb = Physics.Raycast(rv, d, 20);

            if (!bb && b && d.sqrMagnitude <20)
            {
                v.GetComponent<MeshRenderer>().material = _testMat;
            }
            else
            {
                v.GetComponent<MeshRenderer>().material = _defMat;
            }
        }

        // 取得したいトレジャーはゲットできた。
        // Physicsの物理演算処理を直接呼び出すのでコライダーとリジボは必要なし
        // あとはこれをレイヤーで絞り込む必要がある。
        // そのあとに角度で扇状に絞り込む
        // 絞り込んだ中にトレジャーがあればtrue

        return false;
    }

    bool IsWithinRangeAngle(Vector3 i_forwardDir, Vector3 i_toTargetDir, float i_cosTheta)
    {
        // 方向ベクトルが無い場合は、同位置にあるものだと判断する。
        if (i_toTargetDir.sqrMagnitude <= Mathf.Epsilon)
        {
            return true;
        }

        float dot = Vector3.Dot(i_forwardDir, i_toTargetDir);
        return dot >= i_cosTheta;
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(other.gameObject.name);
    //}
}

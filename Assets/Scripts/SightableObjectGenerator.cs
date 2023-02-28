using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// ダンジョンの部屋に配置されているオブジェクトの生成を行うコンポーネント
/// 未使用
/// </summary>
public class SightableObjectGenerator : MonoBehaviour
{
    [Header("生成するプレハブ")]
    [SerializeField] GameObject _prefab;
    [Header("生成する位置")]
    [SerializeField] Vector3 _generatePos;
    [Header("生成する角度(Y軸)")]
    [SerializeField] float _rotY;

    ReactiveProperty<GameObject> _generatedInstance = new ReactiveProperty<GameObject>();

    void Start()
    {
        //_generatedInstance.Value = Instantiate(_prefab, _generatePos,
        //    Quaternion.Euler(new Vector3(0, _rotY, 0)), transform);

        //_generatedInstance.Where(go => go == null)
        //    .Delay(System.TimeSpan.FromSeconds(3.0f))
        //    .Subscribe(go3 => Instantiate(_prefab, _generatePos,
        //    Quaternion.Euler(new Vector3(0, _rotY, 0)), transform));
    }
}

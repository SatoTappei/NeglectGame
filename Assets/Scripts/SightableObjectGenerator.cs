using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// �_���W�����̕����ɔz�u����Ă���I�u�W�F�N�g�̐������s���R���|�[�l���g
/// ���g�p
/// </summary>
public class SightableObjectGenerator : MonoBehaviour
{
    [Header("��������v���n�u")]
    [SerializeField] GameObject _prefab;
    [Header("��������ʒu")]
    [SerializeField] Vector3 _generatePos;
    [Header("��������p�x(Y��)")]
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

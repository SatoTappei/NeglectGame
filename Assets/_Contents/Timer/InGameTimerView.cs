using Cysharp.Text;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �C���Q�[�����̃^�C����UI�̃e�L�X�g�ɕ\������R���|�[�l���g
/// </summary>
public class InGameTimerView : MonoBehaviour
{
    [SerializeField] InGameTimer _inGameTimer;
    [SerializeField] Text _text;

    void Start()
    {
        _inGameTimer.Count.Subscribe(i =>
        {
            _text.text = ZString.Concat(i);
        }).AddTo(this);
    }
}

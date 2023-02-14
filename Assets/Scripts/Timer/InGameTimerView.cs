using Cysharp.Text;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// インゲーム中のタイムをUIのテキストに表示するコンポーネント
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

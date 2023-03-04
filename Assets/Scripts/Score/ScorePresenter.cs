using UniRx;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 撃破数をUIに表示するPresenter
/// </summary>
public class ScorePresenter : MonoBehaviour
{
    [SerializeField] ActorMonitor _actorMonitor;
    [Header("インゲーム中のスコア表示UI")]
    [SerializeField] Text _scoreValueText;
    [Header("リザルト画面のスコア表示UI")]
    [SerializeField] Text _resultValueText;

    void Start()
    {
        _actorMonitor.DefeatedCount.Subscribe(i => PrintScore(i, _scoreValueText)).AddTo(this);
        _actorMonitor.DefeatedCount.Subscribe(i => _resultValueText.text = i.ToString()).AddTo(this);
    }

    void PrintScore(int score, Text textUI)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_scoreValueText.transform.DOScale(Vector3.zero, 0));
        sequence.Append(_scoreValueText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
        sequence.SetLink(_scoreValueText.gameObject);

        textUI.text = score.ToString();
    }
}

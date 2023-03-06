using UniRx;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// ���j����UI�ɕ\������Presenter
/// </summary>
public class ScorePresenter : MonoBehaviour, IPauseable
{
    [SerializeField] ActorMonitor _actorMonitor;
    [Header("�C���Q�[�����̃X�R�A�\��UI")]
    [SerializeField] Text _scoreValueText;
    [Header("���U���g��ʂ̃X�R�A�\��UI")]
    [SerializeField] Text _resultValueText;

    System.IDisposable _scoreDisposable;
    System.IDisposable _resultDisposable;

    void Start()
    {
        _scoreDisposable = _actorMonitor.DefeatedCount.Subscribe(i => PrintScore(i, _scoreValueText)).AddTo(this);
        _resultDisposable = _actorMonitor.DefeatedCount.Subscribe(i => _resultValueText.text = i.ToString()).AddTo(this);
    }

    void PrintScore(int score, Text textUI)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_scoreValueText.transform.DOScale(Vector3.zero, 0));
        sequence.Append(_scoreValueText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
        sequence.SetLink(_scoreValueText.gameObject);

        textUI.text = score.ToString();
    }

    void IPauseable.Pause()
    {
        _scoreDisposable.Dispose();
        _resultDisposable.Dispose();
    }
}
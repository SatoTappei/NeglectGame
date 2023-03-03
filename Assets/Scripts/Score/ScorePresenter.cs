using UniRx;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// åÇîjêîÇUIÇ…ï\é¶Ç∑ÇÈPresenter
/// </summary>
public class ScorePresenter : MonoBehaviour
{
    [SerializeField] ActorMonitor _actorMonitor;
    [SerializeField] Text _scoreValueText;

    void Start()
    {
        _actorMonitor.DefeatedCount.Subscribe(i => PrintScore(i)).AddTo(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) PrintScore(Time.frameCount);
    }

    void PrintScore(int score)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_scoreValueText.transform.DOScale(Vector3.zero, 0));
        sequence.Append(_scoreValueText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
        sequence.SetLink(_scoreValueText.gameObject);

        _scoreValueText.text = score.ToString();
    }
}

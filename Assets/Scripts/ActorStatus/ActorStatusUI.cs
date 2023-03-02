using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// キャラクターのステータスを表示するUIのコンポーネント
/// </summary>
public class ActorStatusUI : MonoBehaviour
{
    static readonly float MovingDistance = 500.0f;
    static readonly float MovingDuration = 0.25f;

    [SerializeField] Image _icon;
    [SerializeField] Text _hpLabelText;
    [SerializeField] Text _hpValueText;

    ActorStatusUIManager _manager;
    Tween _tween;

    public void Init(ActorStatusUIManager manager)
    {
        _manager = manager;
    }

    public void SetValueAll(Sprite sprite, int maxHp)
    {
        _icon.sprite = sprite;
        string maxHpStr = maxHp.ToString();
        _hpLabelText.text = "/ " + maxHpStr;
        _hpValueText.text = maxHpStr;
    }

    public void Play()
    {
        _tween?.Complete();
        _tween = transform.DOMoveX(MovingDistance, MovingDuration).SetLink(gameObject);
    }

    public void Release()
    {
        _manager.ReturnUI(this);

        _tween?.Complete();
        _tween = transform.DOMoveX(-MovingDistance, MovingDuration).SetLink(gameObject);
    }

    public void SetHp(int value) => _hpValueText.text = value.ToString();
}

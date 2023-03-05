using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// キャラクターのステータスを表示するUIのコンポーネント
/// </summary>
public class ActorStatusUI : MonoBehaviour
{
    static readonly float MovingDistance = 200.0f;
    static readonly float MovingDuration = 0.25f;

    [SerializeField] Image _icon;
    [SerializeField] Text _hpLabelText;
    [SerializeField] Text _hpValueText;
    [SerializeField] Image _lineFrameImage;
    [SerializeField] Text _lineText;

    ActorStatusUIManager _manager;
    Tween _tween;
    Color _defaultColor;

    public void Init(ActorStatusUIManager manager)
    {
        _lineFrameImage.enabled = false;
        _lineText.text = "";
        _defaultColor = _hpLabelText.color;
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
        _lineFrameImage.enabled = false;

        _tween?.Complete();
        _tween = transform.DOMoveX(-MovingDistance, MovingDuration).SetLink(gameObject);
    }

    public void SetHp(int currentHp, int maxHp)
    {
        if (currentHp*1.0f / maxHp * 1.0f < 0.5f)
        {
            _hpLabelText.color = Color.red;
            _hpValueText.color = Color.red;
            _hpValueText.text = currentHp.ToString();
        }
        else
        {
            _hpLabelText.color = _defaultColor;
            _hpValueText.color = _defaultColor;
            _hpValueText.text = currentHp.ToString();
        }
    }

    public void PrintLine(string line)
    {
        _lineFrameImage.enabled = true;
        _lineText.text = line;
    }
}

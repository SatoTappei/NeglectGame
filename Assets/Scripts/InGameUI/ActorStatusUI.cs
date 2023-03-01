using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// キャラクターのステータスを表示するUIのコンポーネント
/// </summary>
public class ActorStatusUI : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] Text _hpLabelText;
    [SerializeField] Text _hpValueText;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Init(Sprite sprite, int maxHp)
    {
        _icon.sprite = sprite;

        string maxHpStr = maxHp.ToString();
        _hpLabelText.text = "/ " + maxHpStr;
        _hpValueText.text = maxHpStr;
    }

    public void SetHp(int value) => _hpValueText.text = value.ToString();
}

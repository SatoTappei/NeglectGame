using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �L�����N�^�[�̃X�e�[�^�X��\������UI�̃R���|�[�l���g
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

    public void Init()
    {

    }

    public void SetHp(int value) => _hpValueText.text = value.ToString();
}

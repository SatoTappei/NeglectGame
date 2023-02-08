using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �A�N�^�[�̎��E�ɓ������ۂɃI�u�W�F�N�g�̎�ނ𔻒肷�邽�߂̗񋓌^
/// </summary>
enum SightableType
{
    Treasure,
    Enemy,
}

/// <summary>
/// �A�N�^�[�̎��E�ɉf��I�u�W�F�N�g�̃R���|�[�l���g
/// </summary>
public class SightableObject : MonoBehaviour
{
    [Header("�A�N�^�[���猩���I�u�W�F�N�g�̎��")]
    [SerializeField] SightableType _sightableType;

    internal SightableType SightableType => _sightableType;
}

using UnityEngine;

/// <summary>
/// �L�����N�^�[���g�p���邱�Ƃ��o����I�u�W�F�N�g�̃R���|�[�l���g
/// </summary>
public class EffectableObject : MonoBehaviour, IEffectable
{
    void IEffectable.EffectByActor()
    {
        // �����ɃA�j���[�V�����Ȃǂ̉��o������
        Destroy(gameObject);
    }
}

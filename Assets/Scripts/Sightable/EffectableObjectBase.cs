using UnityEngine;

/// <summary>
/// �L�����N�^�[�ɂ���ĉe����^������I�u�W�F�N�g�̃R���|�[�l���g�̒��ۃN���X
/// </summary>
public abstract class EffectableObjectBase : SightableObject, IEffectable
{
    [Header("�N�������ɍĐ������Particle")]
    [SerializeField] GameObject _popParticle;

    GameObject _particle;
    Actor _effectedActor;

    void OnEnable()
    {
        _particle ??= Instantiate(_popParticle, transform.position, Quaternion.identity);

        _particle.SetActive(true);
        ResetEffectedActor();
    }

    protected void ResetEffectedActor() => _effectedActor = null;

    void IEffectable.Effect(string message) => Effect(message);

    protected abstract void Effect(string message);

    /// <summary>
    /// �e����^������I�u�W�F�N�g�ɑ΂��Ă�1�l�̃L�����N�^�[�����G��邱�Ƃ��o���Ȃ��̂�
    /// �h���N���X�ŏ���������������悤�Ȃ��Ƃ����Ă͂����Ȃ�
    /// </summary>
    public sealed override bool IsAvailable(Actor actor)
    {
        if (_effectedActor == null)
        {
            _effectedActor = actor;
            return true;
        }
        else if (_effectedActor == actor)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

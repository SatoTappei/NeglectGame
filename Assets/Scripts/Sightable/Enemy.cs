using UnityEngine;

/// <summary>
/// �G�̃v���n�u�𐧌䂷��R���|�[�l���g
/// </summary>
public class Enemy : MonoBehaviour, IEffectable
{
    [SerializeField] Animator _anim;
    [SerializeField] SightableObject _sightableObject;
    [Header("�L�����N�^�[�̎�����Ray���q�b�g����R���C�_�[")]
    [SerializeField] Collider _rayHitCollider;
    [Header("�U���A�j���[�V�������Đ����鎞��")]
    [SerializeField] float _playingAnimationTime = 4.0f;
    [Header("�L�����N�^�[���Ăю��F�ł���悤�ɂȂ�܂ł̎���")]
    [SerializeField] float _visibleAgainTime = 8.0f;

    ///// <summary>�����̃L�����N�^�[�Ɏ��F����Ȃ��悤�ɂ��邽�߂̃t���O</summary>
    //bool _isAvailable = true;

    //public bool IsAvailable => _isAvailable;

    void OnEnable()
    {
        _sightableObject.OnSelectedMovingTarget += InactiveCollider;
    }

    void OnDisable()
    {
        _sightableObject.OnSelectedMovingTarget -= InactiveCollider;
    }

    void InactiveCollider()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    ///// <summary>���F�\���ǂ����̐؂�ւ���Actor����Ray�ɂ��擾�ōs��</summary>
    //public void SetUnAvailable() => _isAvailable = false;

    void IEffectable.Effect(string message)
    {
        if (message == "ActorWin")
        {
            // �U���A�j���[�V�����Đ�
            // _playingAnimationTime�b��
            // �A�j���[�V������~��Destroy�����o
        }
        else if (message == "ActorLose")
        {
            // �U���A�j���[�V�����Đ�
            // _playingAnimationTime�b��
            // �A�j���[�V������~
            // _visibleAgainTime�b��A�Ăю��F�\�ɂȂ�
        }
        else
        {
            Debug.LogWarning("Enemy�ł͏����ł��Ȃ����b�Z�[�W�ł�: " + message);
        }
    }

    

    // �L�����N�^�[�̎������q�b�g������R���C�_�[�I�t
    // �ۑ�:�A�j���[�V�����̊J�n�ƏI���̃^�C�~���O
    // ����=>�_�b�V��=>�G�̐^��O�ɗ�����U���J�n
    // �U���J�n�̓R���C�_�[���q�b�g������ł���
    // �U���I���̃^�C�~���O is �b�ł��������w�肷��
    // �m�[�h�ŕ�����^�̃��b�Z�[�W���O

    // �������ꍇ�ƕ������ꍇ������
    // �L�����N�^�[�����ꍇ�͂Ԃ����
    // �L�����N�^�[��������ꍇ�͍ĂуR���C�_�[���A�N�e�B�u�����Ȃ��Ƃ����Ȃ�
}

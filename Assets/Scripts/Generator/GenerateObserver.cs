using UniRx;
using UnityEngine;

/// <summary>
/// Generator�Ő��������I�u�W�F�N�g�𐶐������^�C�~���O��
/// �Q�Ƃ������R���|�[�l���g���g���ď������������肷��
/// </summary>
public class GenerateObserver : MonoBehaviour
{
    [SerializeField] Generator _generator;
    //[Header("�Q�Ƃ��������������R���|�[�l���g�ւ̎Q��")]

    void Awake()
    {
        // Awake()��Enabled()�̌�AStart()�̑O�ɌĂ΂��
        _generator.LastInstantiatedPrefab.Where(gameobject => gameobject != null).Subscribe(gameObject =>
        {
            // �ʒu���K�i�ɂ���

            // UI�ɏ��������n��
        });
    }
}

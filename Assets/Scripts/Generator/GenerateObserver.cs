using UniRx;
using UnityEngine;

/// <summary>
/// Generator�Ő��������I�u�W�F�N�g�𐶐������^�C�~���O��
/// �Q�Ƃ������R���|�[�l���g���g���ď������������肷��
/// </summary>
public class GenerateObserver : MonoBehaviour
{
    [SerializeField] Generator _generator;

    void Awake()
    {
        _generator.LastInstantiatedPrefab.Where(gameobject => gameobject!=null).Subscribe(gameObject => 
        {
            Debug.Log(gameObject.name);
        });
    }

    /// <summary>Awake()��OnEnable()����AStart()�̑O�ɌĂ΂��</summary>
    public void Decorate(GameObject instance)
    {
        // �ʒu���K�i�ɂ���
        // UI�ɏ��������n��
    }
}

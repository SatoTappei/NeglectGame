using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;

/// <summary>
/// ���U���g��ʂŎg�p����UI�𐧌䂷��R���|�[�l���g
/// </summary>
public class ResultUIControl : MonoBehaviour
{
    static float FirstAnimDuration = 0.15f;
    static float SecondAnimDuration = 0.25f;
    static float ThirdAnimDuration = 0.15f;

    [SerializeField] Transform _resultItemRoot;

    void Start()
    {
        _resultItemRoot.transform.localScale = Vector3.zero;
    }

    public async UniTask AnimationAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_resultItemRoot.transform.DOScale(new Vector3(0.01f, 1.0f, 1.0f), FirstAnimDuration));
        sequence.Append(_resultItemRoot.transform.DOScale(new Vector3(1.1f, 1.0f, 1.0f), SecondAnimDuration));
        sequence.Append(_resultItemRoot.transform.DOScale(Vector3.one, ThirdAnimDuration));
        sequence.SetLink(gameObject);

        float delay = FirstAnimDuration + SecondAnimDuration + ThirdAnimDuration;
        await UniTask.Delay(System.TimeSpan.FromSeconds(delay), cancellationToken: token);
    }


    // �|���������L�^����K�v������
    // MVRP�ōs��
    // View�͂��邪Model��Presenter���Ȃ�

    // UI�ɑ���������\������
    // ���U���g��UI��\��

    // �{�^�����N���b�N������^�C�g���ɖ߂�
    // �{�^���̉������񂾃A�j���[�V������Ƀt�F�[�h����
    // �V�[���̃����[�h
}

using UnityEngine;
using Cysharp.Text;

/// <summary>
/// �m���ILSystem��p���ĕ�����̐������s��
/// </summary>
public class LSystem : MonoBehaviour
{
    [Header("�K�p����K����SO")]
    [SerializeField] LSystemRuleSO _ruleSO;
    [Header("�����������s����")]
    [Range(0, 10)]
    [SerializeField] int _iteration;

    /// <summary>�C���X�y�N�^�[�Ɋ��蓖�Ă��l�ŕ�����̐������s��</summary>
    internal string Generate() => Generate(_ruleSO.InitLetter, _ruleSO.RuleArr);

    string Generate(string letter, RewriteRule[] ruleArr)
    {
        using (Utf16ValueStringBuilder builder = ZString.CreateStringBuilder())
        {
            builder.Append(letter);
            return Recursive(builder, ruleArr);
        }
    }

    /// <summary>���߂�ꂽ�񐔂����ċA�I�ɏ���������</summary>
    string Recursive(Utf16ValueStringBuilder builder, RewriteRule[] ruleArr, int currentIte = 0)
    {
        // �񐔂𒴂��Ă����當�����Ԃ��ď����𔲂���
        if (++currentIte > _iteration)
            return builder.ToString();

        // �S�Ẵ��[����K�p���ď���������
        // ��������������������ꍇ�͓��m���Ń����_���ɓK�p����
        foreach (RewriteRule rule in ruleArr)
            builder.Replace(rule.Target, rule.Rewrite[Random.Range(0, rule.Rewrite.Length)]);
        //Debug.Log(currentIte + "��ڂ̏��������̌���: " + builder.ToString());
        
        return Recursive(builder, ruleArr, currentIte);
    }
}

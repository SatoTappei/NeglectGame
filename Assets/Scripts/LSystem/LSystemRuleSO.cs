using UnityEngine;
using System;

/// <summary>
/// LSystem��p���ď����������s���ۂ̃��[���̍\����
/// </summary>
[Serializable]
internal struct RewriteRule
{
    [SerializeField] char _target;
    [SerializeField] string[] _rewrite;

    internal string Target => _target.ToString();
    internal string[] Rewrite => _rewrite;
}

/// <summary>
/// LSystem�Ő�������ۂ̃��[�����L�q����SO
/// </summary>
[CreateAssetMenu(fileName = "LSystemRule_")]
public class LSystemRuleSO : ScriptableObject
{
    [Header("����������")]
    [SerializeField] string _initLine;
    [Header("�����������[��")]
    [SerializeField] RewriteRule[] _rules;

    internal string InitLine => _initLine;
    internal RewriteRule[] Rules => _rules;
}

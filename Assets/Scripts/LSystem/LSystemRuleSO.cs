using UnityEngine;
using System;

/// <summary>
/// LSystemを用いて書き換えを行う際のルールの構造体
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
/// LSystemで生成する際のルールを記述するSO
/// </summary>
[CreateAssetMenu(fileName = "LSystemRule_")]
public class LSystemRuleSO : ScriptableObject
{
    [Header("初期文字列")]
    [SerializeField] string _initLetter;
    [Header("書き換えルール")]
    [SerializeField] RewriteRule[] _ruleArr;

    internal string InitLetter => _initLetter;
    internal RewriteRule[] RuleArr => _ruleArr;
}

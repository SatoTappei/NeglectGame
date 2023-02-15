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
    [SerializeField] string _initLine;
    [Header("書き換えルール")]
    [SerializeField] RewriteRule[] _rules;

    internal string InitLine => _initLine;
    internal RewriteRule[] Rules => _rules;
}

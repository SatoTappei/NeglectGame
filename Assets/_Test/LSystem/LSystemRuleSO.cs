using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// LSystemで生成する際のルールを記述するSO
/// </summary>
[CreateAssetMenu(fileName = "LSystemRule_")]
public class LSystemRuleSO : ScriptableObject
{
    [Serializable]
    internal struct RewriteRule
    {
        [SerializeField] char _target;
        [SerializeField] string[] _rewrite;

        internal string Target => _target.ToString();
        internal string[] Rewrite => _rewrite;
    }

    [Header("初期文字列")]
    [SerializeField] string _initLetter;
    [Header("書き換えルール")]
    [SerializeField] RewriteRule[] _ruleArr;

    internal string InitLetter => _initLetter;
    internal RewriteRule[] RuleArr => _ruleArr;
}

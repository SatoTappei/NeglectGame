using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// LSystem�Ő�������ۂ̃��[�����L�q����SO
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

    [Header("����������")]
    [SerializeField] string _initLetter;
    [Header("�����������[��")]
    [SerializeField] RewriteRule[] _ruleArr;

    internal string InitLetter => _initLetter;
    internal RewriteRule[] RuleArr => _ruleArr;
}

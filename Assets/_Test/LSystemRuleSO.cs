using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// LSystem‚Å¶¬‚·‚éÛ‚Ìƒ‹[ƒ‹‚ğ‹LÚ‚·‚éSO
/// </summary>
[CreateAssetMenu(fileName = "LSystemRule_")]
public class LSystemRuleSO : ScriptableObject
{
    [Serializable]
    public struct RewriteRule
    {
        [SerializeField] char _target;
        [SerializeField] string[] _rewrite;

        public string Target => _target.ToString();
        public string[] Rewrite => _rewrite;
    }

    [Header("‰Šú•¶š—ñ")]
    [SerializeField] string _initLetter;
    [Header("‘‚«Š·‚¦ƒ‹[ƒ‹")]
    [SerializeField] RewriteRule[] _ruleArr;

    public string InitLetter => _initLetter;
    public RewriteRule[] RuleArr => _ruleArr;
}

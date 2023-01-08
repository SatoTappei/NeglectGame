using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Text;
using RewriteRule = LSystemRuleSO.RewriteRule;

/// <summary>
/// 確率的LSystemを用いた地形生成を行う
/// </summary>
public class LSystem : MonoBehaviour
{
    [Header("適用する規則のSO")]
    [SerializeField] LSystemRuleSO _ruleSO;
    [Header("書き換えを行う回数")]
    [Range(0, 10)]
    [SerializeField] int _iteration;

    void Start()
    {
        string result = Generate(_ruleSO.InitLetter, _ruleSO.RuleArr);
    }

    string Generate(string letter, RewriteRule[] ruleArr)
    {
        using (Utf16ValueStringBuilder builder = ZString.CreateStringBuilder())
        {
            builder.Append(letter);
            return Recursive(builder, ruleArr);
        }
    }

    /// <summary>決められた回数だけ再帰的に書き換える</summary>
    string Recursive(Utf16ValueStringBuilder builder, RewriteRule[] ruleArr, int currentIte = 0)
    {
        // 回数を超えていたら文字列を返して処理を抜ける
        if (++currentIte > _iteration)
            return builder.ToString();

        // 全てのルールを適用して書き換える
        foreach (RewriteRule rule in ruleArr)
            builder.Replace(rule.Target, rule.Rewrite[Random.Range(0, rule.Rewrite.Length)]);

        Debug.Log(currentIte + "回目の書き換えの結果: " + builder.ToString());
        
        return Recursive(builder, ruleArr, currentIte);
    }
}

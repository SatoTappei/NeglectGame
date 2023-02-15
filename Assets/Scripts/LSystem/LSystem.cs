using UnityEngine;
using Cysharp.Text;

/// <summary>
/// 確率的LSystemを用いて文字列の生成を行う
/// </summary>
public class LSystem : MonoBehaviour
{
    [Header("適用する規則のSO")]
    [SerializeField] LSystemRuleSO _ruleSO;
    [Header("書き換えを行う回数")]
    [Range(0, 10)]
    [SerializeField] int _iteration;

    /// <summary>インスペクターに割り当てた値で文字列の生成を行う</summary>
    internal string Generate() => Generate(_ruleSO.InitLetter, _ruleSO.RuleArr);

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
        // 書き換え方が複数ある場合は同確率でランダムに適用する
        foreach (RewriteRule rule in ruleArr)
            builder.Replace(rule.Target, rule.Rewrite[Random.Range(0, rule.Rewrite.Length)]);
        //Debug.Log(currentIte + "回目の書き換えの結果: " + builder.ToString());
        
        return Recursive(builder, ruleArr, currentIte);
    }
}

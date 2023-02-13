using UnityEngine;

/// <summary>
/// キャラクターが使用することが出来るオブジェクトのコンポーネント
/// </summary>
public class EffectableObject : MonoBehaviour, IEffectable
{
    void IEffectable.EffectByActor()
    {
        // ここにアニメーションなどの演出を書く
        Destroy(gameObject);
    }
}

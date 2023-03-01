using UnityEngine;

/// <summary>
/// キャラクターによって影響を与えられるオブジェクトのコンポーネントの抽象クラス
/// </summary>
public abstract class EffectableObjectBase : SightableObject, IEffectable
{
    [Header("湧いた時に再生されるParticle")]
    [SerializeField] GameObject _popParticle;

    GameObject _particle;
    Actor _effectedActor;

    void OnEnable()
    {
        _particle ??= Instantiate(_popParticle, transform.position, Quaternion.identity);

        _particle.SetActive(true);
        ResetEffectedActor();
    }

    protected void ResetEffectedActor() => _effectedActor = null;

    void IEffectable.Effect(string message) => Effect(message);

    protected abstract void Effect(string message);

    /// <summary>
    /// 影響を与えられるオブジェクトに対しては1人のキャラクターしか触れることが出来ないので
    /// 派生クラスで条件を書き換えるようなことをしてはいけない
    /// </summary>
    public sealed override bool IsAvailable(Actor actor)
    {
        if (_effectedActor == null)
        {
            _effectedActor = actor;
            return true;
        }
        else if (_effectedActor == actor)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

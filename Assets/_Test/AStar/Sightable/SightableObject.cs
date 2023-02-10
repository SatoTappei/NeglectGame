using UnityEngine;

/// <summary>
/// アクターの視界に入った際にオブジェクトの種類を判定するための列挙型
/// </summary>
enum SightableType
{
    Treasure,
    Enemy,
}

/// <summary>
/// アクターの視界に映るオブジェクトのコンポーネント
/// </summary>
public class SightableObject : MonoBehaviour
{
    [Header("アクターから見たオブジェクトの種類")]
    [SerializeField] SightableType _sightableType;

    GameObject _currentWitness;

    internal SightableType SightableType => _sightableType;

    internal void SetWitness(GameObject obj) => _currentWitness = obj;
    internal void ReleaseWitness() => _currentWitness = null;
    internal bool HasWitness() => _currentWitness != null;
}
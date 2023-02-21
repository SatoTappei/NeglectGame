using UnityEngine;

/// <summary>
/// キャラクターの視界に入った際にオブジェクトの種類を判定するための列挙型
/// </summary>
enum SightableType
{
    RoomEntrance,
    Treasure,
    Enemy,
}

/// <summary>
/// キャラクターの視界に映るオブジェクトのコンポーネント
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class SightableObject : MonoBehaviour
{
    [Header("オブジェクトの判別に使う種類")]
    [SerializeField] SightableType _sightableType;

    internal SightableType SightableType => _sightableType;

    void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Sightable");

        SphereCollider collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;
    }
}
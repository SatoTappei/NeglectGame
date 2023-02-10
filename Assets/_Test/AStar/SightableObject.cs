using System.Collections;
using System.Collections.Generic;
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

    internal SightableType SightableType => _sightableType;
}

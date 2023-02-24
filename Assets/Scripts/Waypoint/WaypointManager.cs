using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 経路探索に使用するWaypointを管理するコンポーネント
/// 各キャラクターはこのクラスからWaypointの一覧を取得し、キャラクター毎にWaypointを管理する
/// </summary>
public class WaypointManager : MonoBehaviour, IWaypointManage
{
    static readonly int DicCapacity = Enum.GetValues(typeof(WaypointType)).Length;

    [Header("Waypointが子として登録されているオブジェクト")]
    [SerializeField] Transform _waypointParent;

    Dictionary<WaypointType, List<Vector3>> _waypointDic = new(DicCapacity);

    // TODO:ValueのListを読み取り専用にキャストするとキャスト不可能エラーが出るのをどうにかする
    //      Listを読み取り専用で渡していないのでDictionaryを読み取り専用にしても意味がない
    IReadOnlyDictionary<WaypointType, List<Vector3>> IWaypointManage.WaypointDic => _waypointDic;

    /// <summary>
    /// WayPointの数や位置が更新されたタイミングで呼ぶことで更新可能
    /// キャラクターはDictionaryを参照しているので更新してもキャラクター側はそのままで大丈夫
    /// </summary>
    public void RegisterWaypoint()
    {
        foreach(Transform child in _waypointParent)
        {
            if(child.TryGetComponent(out WaypointTag component))
            {
                WaypointType type = component.Type;
                Vector3 pos = child.position;

                if (!_waypointDic.ContainsKey(type))
                {
                    _waypointDic.Add(type, new List<Vector3>());
                }

                _waypointDic[type].Add(pos);
            }
            else
            {
                Debug.LogError("Waypointとして使用できません: " + child.gameObject.name);
            }
        }
    }

    public List<Vector3> GetWaypointListWithWaypointType(WaypointType type) => _waypointDic[type];
}


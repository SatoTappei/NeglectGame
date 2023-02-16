using UnityEngine;

/// <summary>
/// 経路探索を用いて移動する先を管理するコンポーネント
/// </summary>
internal class PathfindingTarget : MonoBehaviour, ITargetSelectable
{
    [SerializeField] Transform[] _targetArr;
    [SerializeField] Transform _exit;

    // TODO:前回と同じ地点をゴールとして選んでしまうと移動しなくなる不具合があるのでどうにかする
    int _prev = -1;

    Vector3 ITargetSelectable.GetNextWaypointPos()
    {
        int r;
        while (true)
        {
            r = Random.Range(0, _targetArr.Length);
            if (r != _prev)
            {
                _prev = r;
                break;
            }
        }

        return _targetArr[r].position;
    }

    Vector3 ITargetSelectable.GetExitPos() => _exit.position;
}

// Waypointの生成はダンジョン生成側が行う
// 通路、部屋の入口、階段があるので判別する何かが必要、後の追加も考える
// この座標のリストは○○のWaypointというやり方はいかがなものか
// 増えてきたら管理が難しくなる？
// 

// こちらは生成されたWaypointを読み取る
// DungeonWaypointVisualizer => Presetnerに生成
// WaypointTarget => Presetnerから読み取り
// 時間的結合が存在する Waypoint生成 => Targetで読み込み
// だがこれはダンジョン生成=>パスファインドのグリッド生成の順で行えばおｋなので問題なし
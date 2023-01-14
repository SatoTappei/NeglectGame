using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = DungeonHelper.Direction;

/// <summary>
/// �_���W�����̒ʘH�ɉ����������𗧂Ă�R���|�[�l���g
/// </summary>
public class DungeonRoomBuilder : MonoBehaviour
{
    readonly int RoomDicCap = 16;
    readonly int PlaceDicCap = 64;
    readonly int BlockPosSetCap = 64;

    [Header("�����̃v���n�u")]
    [SerializeField] DungeonRoomData[] _roomDataArr;
    [Header("���������v���n�u�̐e")]
    [SerializeField] Transform _parent;

    [SerializeField] GameObject _test;

    DungeonHelper _helper;
    Dictionary<Vector3Int, GameObject> _roomDic;

    void Awake()
    {
        _helper = new DungeonHelper();
        _roomDic = new Dictionary<Vector3Int, GameObject>(RoomDicCap);
    }

    /// <summary>�����̐������s��</summary>
    internal void GenerateRoom(IReadOnlyCollection<Vector3Int> passAll)
    {
        // �����𐶐��\�ȍ��W�̎������쐬����
        Dictionary<Vector3Int, Direction> placeDic = new Dictionary<Vector3Int, Direction>(PlaceDicCap);
        InsertToDic(placeDic, passAll);

        //// �����_����1�ӏ��擾����
        //// �擾�����ӏ��������s�\�ȉӏ��������ꍇ�͍Ď擾����K�v������
        //KeyValuePair<Vector3Int, Direction> paair = placeDic.ElementAtOrDefault(Random.Range(0, placeDic.Count));
        //DungeonRoomData data = _roomDataArr[0];
        //Quaternion rot = GetInverseRot(paair.Value);

        //Instantiate(data.GetPrefab(), paair.Key, rot, _parent);

        // �����s�\�ȍ��W��ێ����Ă����n�b�V���Z�b�g
        HashSet<Vector3Int> blockPosSet = new HashSet<Vector3Int>(BlockPosSetCap);

        // 
        int index = 0;
        // �����_���ɕ��ёւ����ݒu���̎��������ɑ�������
        foreach (KeyValuePair<Vector3Int, Direction> pair in placeDic.OrderBy(_ => System.Guid.NewGuid()))
        {
            // �������镔���̃f�[�^
            DungeonRoomData data = _roomDataArr[index];

            var ret = Check(pair.Key, pair.Value, data.Size, blockPosSet,passAll);

            // �܂��͂��̈ʒu�ɕK�v�Ȏ�ނ̕����������ł��邩���ׂ�
            if (ret.Item1)
            {
                Quaternion rot = GetInverseRot(pair.Value);
                Instantiate(data.GetPrefab(), pair.Key, rot, _parent);

                foreach(var v in ret.Item2)
                {
                    blockPosSet.Add(v);
                    //Instantiate(_test, v, Quaternion.identity);
                }
            }
            else
            {

            }
        }
    }

    (bool,HashSet<Vector3Int>) Check(Vector3Int pos, Direction dir, int size, IReadOnlyCollection<Vector3Int> blockPosSet, IReadOnlyCollection<Vector3Int> set2)
    {
        HashSet<Vector3Int> tempSet = new HashSet<Vector3Int>();

        // ���E�Ɖ��s�𒲂ׂ�
        switch (dir)
        {
            case Direction.Forward:
                // ���s���J��Ԃ�
                for(int i = 0; i < size; i++)
                {
                    Vector3Int tempPos = new Vector3Int(pos.x, pos.y, pos.z + i * _helper.PrefabScale);
                    // ���_���u���b�N����ӏ��Ƃ��Ēǉ�
                    tempSet.Add(tempPos);
                    //Instantiate(_test, tempPos, Quaternion.identity);
                    // ���E�̕������u���b�N����ӏ��Ƃ��Ēǉ�
                    for (int j = 1; j <= size; j++)
                    {
                        Vector3Int SideLeft = tempPos + Vector3Int.left * j * _helper.PrefabScale;
                        Vector3Int SideRight = tempPos + Vector3Int.right * j * _helper.PrefabScale;

                        tempSet.Add(SideLeft);
                        tempSet.Add(SideRight);
                        //Instantiate(_test, SideLeft, Quaternion.identity);
                        //Instantiate(_test, SideRight, Quaternion.identity);
                    }
                }
                break;
            case Direction.Back:
                // ���s���J��Ԃ�
                for (int i = 0; i < size; i++)
                {
                    Vector3Int tempPos = new Vector3Int(pos.x, pos.y, pos.z - i * _helper.PrefabScale);
                    // ���_���u���b�N����ӏ��Ƃ��Ēǉ�
                    tempSet.Add(tempPos);

                    // ���E�̕������u���b�N����ӏ��Ƃ��Ēǉ�
                    for (int j = 1; j <= size; j++)
                    {
                        Vector3Int SideLeft = tempPos + Vector3Int.left * j * _helper.PrefabScale;
                        Vector3Int SideRight = tempPos + Vector3Int.right * j * _helper.PrefabScale;

                        tempSet.Add(SideLeft);
                        tempSet.Add(SideRight);
                    }
                }
                break;
            case Direction.Left:
                // ���s���J��Ԃ�
                for (int i = 0; i < size; i++)
                {
                    Vector3Int tempPos = new Vector3Int(pos.x - i * _helper.PrefabScale, pos.y, pos.z);
                    // ���_���u���b�N����ӏ��Ƃ��Ēǉ�
                    tempSet.Add(tempPos);

                    // ���E�̕������u���b�N����ӏ��Ƃ��Ēǉ�
                    for (int j = 1; j <= size; j++)
                    {
                        Vector3Int SideLeft = tempPos + Vector3Int.forward * j * _helper.PrefabScale;
                        Vector3Int SideRight = tempPos + Vector3Int.back * j * _helper.PrefabScale;

                        tempSet.Add(SideLeft);
                        tempSet.Add(SideRight);
                    }
                }
                break;
            case Direction.Right:
                // ���s���J��Ԃ�
                for (int i = 0; i < size; i++)
                {
                    Vector3Int tempPos = new Vector3Int(pos.x + i * _helper.PrefabScale, pos.y, pos.z);
                    // ���_���u���b�N����ӏ��Ƃ��Ēǉ�
                    tempSet.Add(tempPos);

                    // ���E�̕������u���b�N����ӏ��Ƃ��Ēǉ�
                    for (int j = 1; j <= size; j++)
                    {
                        Vector3Int SideLeft = tempPos + Vector3Int.forward * j * _helper.PrefabScale;
                        Vector3Int SideRight = tempPos + Vector3Int.back * j * _helper.PrefabScale;

                        tempSet.Add(SideLeft);
                        tempSet.Add(SideRight);
                    }
                }
                break;
        }

        bool flag = true;
        foreach(var v in tempSet)
        {
            bool b = !blockPosSet.Contains(v);
            bool bb = !set2.Contains(v);

            if (b && bb)
            {
                
            }
            else
            {
                flag = false;
            }
        }

        return (flag,tempSet);
    }

    /// <summary>�����������\�ȏꏊ�������ɑ}������</summary>
    void InsertToDic(Dictionary<Vector3Int, Direction> dic, IReadOnlyCollection<Vector3Int> passAll)
    {
        foreach (Vector3Int pos in passAll)
        {
            // ���̖͂��܂��Ă���}�X�̕��p���擾����
            (int dirs, _) = _helper.GetNeighbourInt(pos, passAll);

            // �e�����ɒʘH��������΂��̕����𕔉��𐶐��\�ȏꏊ�Ƃ��Ď����ɒǉ�����
            if ((dirs & _helper.BForward) != _helper.BForward) AddDic(Direction.Forward);
            if ((dirs & _helper.BBack) != _helper.BBack) AddDic(Direction.Back);
            if ((dirs & _helper.BLeft) != _helper.BLeft) AddDic(Direction.Left);
            if ((dirs & _helper.BRight) != _helper.BRight) AddDic(Direction.Right);

            void AddDic(Direction dir)
            {
                Vector3Int placePos = pos + GetSidePos(dir);
                // �d���`�F�b�N
                if (dic.ContainsKey(placePos)) return;
                dic.Add(placePos, dir);
            }
        }
    }

    /// <summary>�����Ƌt�̉�]���擾����</summary>
    Quaternion GetInverseRot(Direction dir)
    {
        switch (dir)
        {
            case Direction.Forward:
                return Quaternion.Euler(0, 0, 0);
            case Direction.Back:
                return Quaternion.Euler(0, 180, 0);
            case Direction.Left:
                return Quaternion.Euler(0, -90, 0);
            case Direction.Right:
                return Quaternion.Euler(0, 90, 0);
            default:
                Debug.LogError("�񋓌^Direction�Œ�`����Ă��Ȃ��l�ł��B: " + dir);
                return Quaternion.identity;
        }
    }

    /// <summary>�����ɉ�����1�}�X�e�̍��W��Ԃ�</summary>
    Vector3Int GetSidePos(Direction dir)
    {
        switch (dir)
        {
            case Direction.Forward:
                return Vector3Int.forward * _helper.PrefabScale;
            case Direction.Back:
                return Vector3Int.back * _helper.PrefabScale;
            case Direction.Left:
                return Vector3Int.left * _helper.PrefabScale;
            case Direction.Right:
                return Vector3Int.right * _helper.PrefabScale;
            default:
                Debug.LogError("�񋓌^Direction�Œ�`����Ă��Ȃ��l�ł��B: " + dir);
                return Vector3Int.zero;
        }
    }
}

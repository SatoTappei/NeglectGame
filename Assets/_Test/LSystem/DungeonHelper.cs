using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// �_���W���������֌W�̃N���X�Ŏg���w���p�[�N���X
/// </summary>
internal class DungeonHelper
{
    internal enum Direction
    {
        Forward,
        Back,
        Right,
        Left,
    }

    // �_���W���������Ɏg���v���n�u�̑傫��(Transform��Scale�Ƃ͕�)
    internal readonly int PrefabScale = 3;
    // �O�㍶�E��4������\���o�C�i��
    internal const int BinaryForward = 0b1000;
    internal const int BinaryBack    = 0b0100;
    internal const int BinaryLeft    = 0b0010;
    internal const int BinaryRight   = 0b0001;

    /// <summary>�ׂɃI�u�W�F�N�g�����݂���������܂Ƃ߂ĕԂ�</summary>
    internal HashSet<Direction> GetNeighbour(Vector3Int pos, ICollection<Vector3Int> coll)
    {
        HashSet<Direction> dirSet = new HashSet<Direction>(4);
        if (coll.Contains(pos + Vector3Int.forward * PrefabScale)) dirSet.Add(Direction.Forward);
        if (coll.Contains(pos + Vector3Int.back * PrefabScale))    dirSet.Add(Direction.Back);
        if (coll.Contains(pos + Vector3Int.right * PrefabScale))   dirSet.Add(Direction.Right);
        if (coll.Contains(pos + Vector3Int.left * PrefabScale))    dirSet.Add(Direction.Left);

        return dirSet;
    }

    /// <summary>
    /// �ׂɃI�u�W�F�N�g����������Ɛڑ������o�C�i���ŕԂ�
    /// �w�肳�ꂽ�������܂܂�Ă��邩�ǂ����̔���͂�������g��
    /// </summary>
    /// <returns>0b�O�㍶�E, �ڑ���</returns>
    internal (int dirs, int count) GetNeighbourBinary(Vector3Int pos, IReadOnlyCollection<Vector3Int> coll)
    {
        int dirs = 0b0000;
        int count = 0;
        if (coll.Contains(pos + Vector3Int.forward * PrefabScale))
        {
            dirs += BinaryForward;
            count++;
        }
        if (coll.Contains(pos + Vector3Int.back * PrefabScale))
        {
            dirs += BinaryBack;
            count++;
        }
        if (coll.Contains(pos + Vector3Int.left * PrefabScale))
        {
            dirs += BinaryLeft;
            count++;
        }
        if (coll.Contains(pos + Vector3Int.right * PrefabScale))
        {
            dirs += BinaryRight;
            count++;
        }

        return (dirs, count);
    }

    internal bool IsConnectFromBinary(int dirs, int BinaryDir) => (dirs & BinaryDir) == BinaryDir;

    internal Vector3Int ConvertToPos(Direction dir)
    {
        switch (dir)
        {
            case Direction.Forward:
                return Vector3Int.forward * PrefabScale;
            case Direction.Back:
                return Vector3Int.back * PrefabScale;
            case Direction.Left:
                return Vector3Int.left * PrefabScale;
            case Direction.Right:
                return Vector3Int.right * PrefabScale;
            default:
                Debug.LogError("�񋓌^Direction�Œ�`����Ă��Ȃ��l�ł��B: " + dir);
                return Vector3Int.zero;
        }
    }

    /// <summary>�����Ƌt�̉�]���擾����</summary>
    internal Quaternion ConvertToInverseRot(Direction dir)
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

    internal Direction ConvertToDir(Vector3Int dirVec)
    {
        if (dirVec == Vector3Int.forward) return Direction.Forward;
        else if (dirVec == Vector3Int.back) return Direction.Back;
        else if (dirVec == Vector3Int.left) return Direction.Left;
        else if (dirVec == Vector3Int.right) return Direction.Right;
        else
        {
            Debug.LogError("�����x�N�g���̒l���s���ł�: " + dirVec);
            return Direction.Forward;
        }
    }

    internal Direction ConvertToDir(float rotY)
    {
        if (rotY == 0) return Direction.Forward;
        else if (rotY == 180) return Direction.Back;
        else if (rotY == -90) return Direction.Left;
        else if (rotY == 90) return Direction.Right;
        else
        {
            Debug.LogError("float�̒l���s���ł�: " + rotY);
            return Direction.Forward;
        }
    }
}

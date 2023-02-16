using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// �_���W���������֌W�̃N���X�Ŏg���w���p�[�N���X
/// </summary>
internal class DungeonHelper
{
    /// <summary>�_���W���������Ɏg���v���n�u�̑傫��(Transform��Scale�Ƃ͕�)</summary>
    internal static readonly int PrefabScale = 3;
    /// <summary>�O�㍶�E��4������\���o�C�i��</summary>
    internal static readonly int BinaryForward = 0b1000;
    internal static readonly int BinaryBack    = 0b0100;
    internal static readonly int BinaryLeft    = 0b0010;
    internal static readonly int BinaryRight   = 0b0001;

    internal int GetConnectedFromShape(ComponentShape shape)
    {
        switch (shape)
        {
            case ComponentShape.PassEnd:   return 1;
            case ComponentShape.Pass:      return 2;
            case ComponentShape.Corner:    return 2;
            case ComponentShape.TJunction: return 3;
            case ComponentShape.Cross:     return 4;
            default:
                Debug.LogError("�l���s���ł�: " + shape);
                return -1;
        }
    }

    /// <summary>
    /// �ǂ̕����ɂ����ڑ�����Ă��邩�̔���͂킴�킴�R���N�V�����𐶐����Ȃ��Ă��ǂ�
    /// </summary>
    /// <returns>0b�O�㍶�E, �ڑ���</returns>
    internal (int, int) GetNeighbourBinary(Vector3Int centerPos, IReadOnlyCollection<Vector3Int> positions)
    {
        int binary = 0b0000;
        int count = 0;

        if (positions.Contains(centerPos + Vector3Int.forward * PrefabScale))
        {
            binary += BinaryForward;
            count++;
        }
        if (positions.Contains(centerPos + Vector3Int.back * PrefabScale))
        {
            binary += BinaryBack;
            count++;
        }
        if (positions.Contains(centerPos + Vector3Int.left * PrefabScale))
        {
            binary += BinaryLeft;
            count++;
        }
        if (positions.Contains(centerPos + Vector3Int.right * PrefabScale))
        {
            binary += BinaryRight;
            count++;
        }

        return (binary, count);
    }

    internal bool IsConnectedUsingBinary(int neighbour, int center) => (neighbour & center) == center;

    internal Vector3Int GetDirectionPos(Direction dir)
    {
        switch (dir)
        {
            case Direction.Forward: return Vector3Int.forward * PrefabScale;
            case Direction.Back:    return Vector3Int.back * PrefabScale;
            case Direction.Left:    return Vector3Int.left * PrefabScale;
            case Direction.Right:   return Vector3Int.right * PrefabScale;
            default:
                Debug.LogError("�񋓌^Direction�Œ�`����Ă��Ȃ��l�ł�: " + dir);
                return Vector3Int.zero;
        }
    }

    internal Quaternion GetDirectionAngle(Direction dir)
    {
        switch (dir)
        {
            case Direction.Forward: return Quaternion.Euler(0, 0, 0);
            case Direction.Back:    return Quaternion.Euler(0, 180, 0);
            case Direction.Left:    return Quaternion.Euler(0, -90, 0);
            case Direction.Right:   return Quaternion.Euler(0, 90, 0);
            default:
                Debug.LogError("�񋓌^Direction�Œ�`����Ă��Ȃ��l�ł�: " + dir);
                return Quaternion.identity;
        }
    }

    internal Direction GetDirection(float rotY)
    {
        if      (rotY == 0)   return GetDirection(Vector3Int.forward);
        else if (rotY == 180) return GetDirection(Vector3Int.back);
        else if (rotY == -90) return GetDirection(Vector3Int.left);
        else if (rotY == 90)  return GetDirection(Vector3Int.right);
        else
        {
            Debug.LogError("float�̒l���s���ł�: " + rotY);
            return Direction.Forward;
        }
    }

    internal Direction GetDirection(Vector3Int dirVec)
    {
        if      (dirVec == Vector3Int.forward) return Direction.Forward;
        else if (dirVec == Vector3Int.back)    return Direction.Back;
        else if (dirVec == Vector3Int.left)    return Direction.Left;
        else if (dirVec == Vector3Int.right)   return Direction.Right;
        else
        {
            Debug.LogError("�����x�N�g���̒l���s���ł�: " + dirVec);
            return Direction.Forward;
        }
    }

    /// <summary>�n���ꂽ�����x�N�g������90�x��]�����������x�N�g����Ԃ�</summary>
    /// <param name="isPositive">true���ƑO�E�㍶�̎��v���Afalse���Ɣ����v���</param>
    internal Vector3Int GetDirectionVectorRotate90(Vector3Int dirVec, bool isPositive)
    {
        if      (dirVec == Vector3Int.forward) return isPositive ? Vector3Int.right : Vector3Int.left;
        else if (dirVec == Vector3Int.right)   return isPositive ? Vector3Int.back : Vector3Int.forward;
        else if (dirVec == Vector3Int.back)    return isPositive ? Vector3Int.left : Vector3Int.right;
        else if (dirVec == Vector3Int.left)    return isPositive ? Vector3Int.forward : Vector3Int.back;
        else
        {
            Debug.LogError("�㉺���E�ȊO�̊p�x�ł�: " + dirVec);
            return Vector3Int.zero;
        }
    }

    internal bool IsPassStraight(int dirs)
    {
        if (IsConnectedUsingBinary(dirs, BinaryForward) && IsConnectedUsingBinary(dirs, BinaryBack) ||
            IsConnectedUsingBinary(dirs, BinaryLeft) && IsConnectedUsingBinary(dirs, BinaryRight))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    internal float GetPassStraightRotY(Direction roomDir)
    {
        if (roomDir == Direction.Left || roomDir == Direction.Right)
        {
            return 90;
        }
        else
        {
            return 0;
        }
    }

    internal float GetCornerRotY(Direction roomDir, Direction frontMassDir)
    {
        if ((roomDir == Direction.Forward && frontMassDir == Direction.Right) || 
            (roomDir == Direction.Left && frontMassDir == Direction.Back))
        {
            return 90;
        }
        else if ((roomDir == Direction.Back && frontMassDir == Direction.Left) ||
                 (roomDir == Direction.Right && frontMassDir == Direction.Forward))
        {
            return -90;
        }
        else if ((roomDir == Direction.Forward && frontMassDir == Direction.Left) ||
                 (roomDir == Direction.Right && frontMassDir == Direction.Back))
        {
            return 180;
        }
        else
        {
            return 0;
        }
    }

    internal float GetCornerRotY(int dirs)
    {
        if (IsConnectedUsingBinary(dirs, BinaryForward) && IsConnectedUsingBinary(dirs, BinaryRight))
        {
            return 180;
        }
        else if (IsConnectedUsingBinary(dirs, BinaryForward) && IsConnectedUsingBinary(dirs, BinaryLeft))
        {
            return 90;
        }
        else if (IsConnectedUsingBinary(dirs, BinaryBack) && IsConnectedUsingBinary(dirs, BinaryRight))
        {
            return -90;
        }
        else
        {
            return 0;
        }
    }

    internal float GetTJunctionRotY(Direction roomDir, Direction frontMassDir, ComponentShape frontMassShape)
    {
        // �ʘH�ɕ������אڂ��Đ��������p�^�[��
        if (frontMassShape == ComponentShape.Pass)
        {
            if      (roomDir == Direction.Forward) return 180;
            else if (roomDir == Direction.Left)    return 90;
            else if (roomDir == Direction.Right)   return -90;
        }
        // �ʘH�̒[�ŕ���2�����ݍ��ރp�^�[��
        else if (frontMassShape == ComponentShape.PassEnd)
        {
            if      (frontMassDir == Direction.Back)  return 180;
            else if (frontMassDir == Direction.Left)  return -90;
            else if (frontMassDir == Direction.Right) return 90;
        }
        // �ʘH�̊p�ɕ��������������p�^�[��
        else if (frontMassShape == ComponentShape.Corner)
        {
            if ((roomDir == Direction.Forward && frontMassDir == Direction.Forward) ||
                (roomDir == Direction.Back && frontMassDir == Direction.Right))
            {
                return 90;
            }
            else if ((roomDir == Direction.Forward && frontMassDir == Direction.Left) ||
                     (roomDir == Direction.Back && frontMassDir == Direction.Back))
            {
                return -90;
            }
            else if (roomDir == Direction.Left && frontMassDir == Direction.Back)
            {
                return -180;
            }
            else if (roomDir == Direction.Right && frontMassDir == Direction.Right)
            {
                return 180;
            }
        }

        return 0;
    }

    internal float GetTJunctionRotY(int dirs)
    {
        if (IsConnectedUsingBinary(dirs, BinaryForward) && 
            IsConnectedUsingBinary(dirs, BinaryBack) && 
            IsConnectedUsingBinary(dirs, BinaryLeft))
        {
            return 90;
        }
        else if (IsConnectedUsingBinary(dirs, BinaryForward) && 
                 IsConnectedUsingBinary(dirs, BinaryBack) && 
                 IsConnectedUsingBinary(dirs, BinaryRight))
        {
            return -90;
        }
        else if (IsConnectedUsingBinary(dirs, BinaryForward) && 
                 IsConnectedUsingBinary(dirs, BinaryLeft) && 
                 IsConnectedUsingBinary(dirs, BinaryRight))
        {
            return 180;
        }

        return 0;
    }

    internal float GetPassEndRotY(int dirs)
    {
        if      (IsConnectedUsingBinary(dirs, BinaryForward)) return 180;
        else if (IsConnectedUsingBinary(dirs, BinaryLeft)) return 90;
        else if (IsConnectedUsingBinary(dirs, BinaryRight)) return -90;

        return 0;
    }
}

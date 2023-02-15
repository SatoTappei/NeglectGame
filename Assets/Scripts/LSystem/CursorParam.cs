using UnityEngine;

/// <summary>
/// LSystem��p���Đ������������񂩂�_���W�����̒ʘH�𐶐�����ۂɎg��
/// ����̊�ƂȂ�p�����[�^
/// </summary>
internal class CursorParam
{
    internal CursorParam(Vector3Int pos, Vector3Int dirVec, int dist)
    {
        Pos = pos;
        DirVec = dirVec;
        Dist = dist;
    }

    internal Vector3Int Pos { get; }
    internal Vector3Int DirVec { get; }
    internal int Dist { get; }
}
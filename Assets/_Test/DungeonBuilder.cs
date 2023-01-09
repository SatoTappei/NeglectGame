using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ɑΉ������_���W���������Ă�
/// </summary>
public class DungeonBuilder : MonoBehaviour
{
    enum TurtleCommand
    {
        Forward = 'F',
        RotRight = '+',
        RotLeft = '-',
        Save = '[',
        Load = ']',
    }

    //readonly int Angle = 90;
    readonly int PrefabScale = 3;
    readonly int DecreaseDist = 2;
    //readonly int MaxDist = 8;
    readonly int SaveStackCap = 4;

    // TODO:���݂͉��̂��߂�����LSystem�̎Q�Ƃ��������Ă��邪�A��X�ړ������邱�Ƃ𗯈ӂ��Ă���
    [SerializeField] LSystem _lSystem;
    [Header("�_���W�������\�����镔�i")]
    [SerializeField] GameObject _passPrefab;
    [Header("���������_���W�����̕��i�̐e")]
    [SerializeField] Transform _parent;

    List<Vector3Int> _posList = new List<Vector3Int>(10);
    //int _dist;

    void Start()
    {
        //_dist = MaxDist;
        Convert(_lSystem.Generate());
    }

    /// <summary>��������_���W�����̕��i�v���n�u�ɕϊ�����</summary>
    void Convert(string str)
    {
        // �Z�[�u/���[�h�̃R�}���h�p
        Stack<TurtleParam> saveStack = new Stack<TurtleParam>(SaveStackCap);

        Vector3Int currentPos = Vector3Int.zero;
        Vector3Int tempPos = Vector3Int.zero;
        Vector3Int dir = Vector3Int.forward;
        int dist = 8;

        _posList.Add(currentPos);
        
        foreach (char command in str)
        {
            switch ((TurtleCommand)command)
            {
                case TurtleCommand.Forward:
                    GeneratePass(currentPos, dir, dist);
                    currentPos = currentPos + dir * dist * PrefabScale;
                    dist -= DecreaseDist;
                    dist = Mathf.Max(1, dist);
                    break;
                case TurtleCommand.RotRight:
                    dir = RotDir(dir, isPositive: true);
                    break;
                case TurtleCommand.RotLeft:
                    dir = RotDir(dir, isPositive: false);
                    break;
                case TurtleCommand.Save:
                    saveStack.Push(new TurtleParam(currentPos, dir, dist));
                    //Debug.Log($"Push:{currentPos},{dir},{dist}");
                    break;
                case TurtleCommand.Load:
                    if (saveStack.Count == 0) break;
                    TurtleParam param = saveStack.Pop();
                    currentPos = param.Pos;
                    dir        = param.Dir;
                    dist       = param.Dist;
                    //Debug.Log($"Pop:{currentPos},{dir},{dist}");
                    break;
            }
        }
    }

    /// <summary>�����̒ʘH�𐶐�����</summary>
    void GeneratePass(Vector3Int startPos, Vector3Int dir, int dist)
    {
        for (int i = 0; i < dist; i++)
        {
            Instantiate(_passPrefab, startPos + dir * i * PrefabScale, Quaternion.identity, _parent);
        }
    }

    /// <summary>�O�㍶�E�̕�������]������</summary>
    /// <param name="isPositive">true���ƑO�E�㍶�̎��v���Afalse���Ɣ����v���</param>
    Vector3Int RotDir(Vector3Int currentDir, bool isPositive)
    {
        if (currentDir == Vector3Int.forward)
        {
            if (isPositive) return Vector3Int.right;
            else            return Vector3Int.left;
        }
        else if (currentDir == Vector3Int.right)
        {
            if (isPositive) return Vector3Int.back;
            else            return Vector3Int.forward;
        }
        else if (currentDir == Vector3Int.back)
        {
            if (isPositive) return Vector3Int.left;
            else            return Vector3Int.right;
        }
        else if (currentDir == Vector3Int.left)
        {
            if (isPositive) return Vector3Int.forward;
            else            return Vector3Int.back;
        }
        else
        {
            Debug.LogError("�㉺���E�ȊO�̊p�x�ł��B: " + currentDir);
            return Vector3Int.zero;
        }
    }
}

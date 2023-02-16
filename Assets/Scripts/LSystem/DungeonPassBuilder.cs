using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ɑΉ������_���W�����̒ʘH�����Ă�R���|�[�l���g
/// </summary>
public class DungeonPassBuilder : MonoBehaviour
{
    enum Command
    {
        Forward = 'F',
        RotRight = '+',
        RotLeft = '-',
        Save = '[',
        Load = ']',
    }

    /// <summary>��{��6�����A�����ʘH���~�����ꍇ��8</summary>
    static readonly int MaxPassDist = 6;
    /// <summary>�����̐������ɉe����^����̂ŁA��{��2�ŌŒ�</summary>
    static readonly int DecreaseDist = 2;

    // ���������ʘH1�}�X�����i�[����R���N�V�����̏����e�ʂ�
    // LSystem��3�񏑂�������MaxPassDist��6����ɐݒ�
    static readonly int PassMassDicCap = 450;
    static readonly int FixPassSetCap = 70;
    // �Z�[�u�̃R�}���h�͕�����̏����������[������ł�����ł��\��������̂�
    // �Œ���_���W�����炵�����򐔂ɂȂ鏉���e�ʂ�ݒ�
    static readonly int SaveCommandStackCap = 4;

    [Header("�ʘH���\������v���n�u")]
    [SerializeField] GameObject _passPrefab;
    [SerializeField] GameObject _cornerPrefab;
    [SerializeField] GameObject _tJunctionPrefab;
    [SerializeField] GameObject _crossPrefab;
    [SerializeField] GameObject _passEndPrefab;
    [Header("���������v���n�u�̐e")]
    [SerializeField] Transform _prefabParent;

    DungeonHelper _helper = new();
    Dictionary<Vector3Int, DungeonPassMassData> _passMassDic = new (PassMassDicCap);
    /// <summary>������Ɍ����ڂ��C�����邽�߂ɏ����𖞂������ʘH��ێ����Ă���</summary>
    HashSet<Vector3Int> _fixPassSet = new (FixPassSetCap);
    /// <summary>
    /// �ʘH�̐ڑ����ƍs���~�܂��Waypoint��ݒ肷��̂�
    /// �����e�ʂ͏C�����̃}�X�̃R���N�V�����̔����̗e�ʂ�����Ί�{�I�ɂ͏\��
    /// </summary>
    List<Vector3Int> _waypointList = new (FixPassSetCap/2);

    internal IReadOnlyDictionary<Vector3Int, DungeonPassMassData> PassMassDic => _passMassDic;
    internal IReadOnlyCollection<Vector3Int> WaypointList => _waypointList;

    internal void BuildDungeonPass(string str)
    {
        // �Z�[�u/���[�h�̃R�}���h�p
        Stack<CursorParam> saveStack = new (SaveCommandStackCap);

        Vector3Int currentPos = Vector3Int.zero;
        Vector3Int dirVec = Vector3Int.forward;
        int passDist = MaxPassDist;
        
        foreach (char command in str)
        {
            switch ((Command)command)
            {
                // �����̒ʘH�𐶐����ĒʘH�̐�Ɋ�_���ڂ�
                case Command.Forward:
                    GeneratePassStraight(currentPos, dirVec, passDist);
                    currentPos = currentPos + dirVec * passDist * DungeonHelper.PrefabScale;
                    passDist -= DecreaseDist;
                    passDist = Mathf.Max(1, passDist);
                    break;
                // ��_���E��90����]������
                case Command.RotRight:
                    dirVec = _helper.GetDirectionVectorRotate90(dirVec, isPositive: true);
                    break;
                // ��_������90����]������
                case Command.RotLeft:
                    dirVec = _helper.GetDirectionVectorRotate90(dirVec, isPositive: false);
                    break;
                // ���݂̊�_���X�^�b�N�ɐς�
                case Command.Save:
                    saveStack.Push(new (currentPos, dirVec, passDist));
                    break;
                // �X�^�b�N�����_�����o���Ă��̈ʒu�Ɋ�_��ύX����
                case Command.Load:
                    if (saveStack.Count == 0) break;
                    CursorParam param = saveStack.Pop();
                    currentPos = param.Pos;
                    dirVec = param.DirVec;
                    passDist = param.Dist;
                    break;
            }
        }
    }

    void GeneratePassStraight(Vector3Int startPos, Vector3Int dirVec, int dist)
    {
        for (int i = 0; i < dist; i++)
        {
            Vector3Int pos = startPos + dirVec * i * DungeonHelper.PrefabScale;

            // �������W�ɒʘH����������Ȃ��悤�Ƀ`�F�b�N
            if (_passMassDic.ContainsKey(pos)) continue;

            Direction dir = _helper.GetDirection(dirVec);
            float rotY = _helper.GetPassStraightRotY(dir);
            GameObject go = Instantiate(_passPrefab, pos, Quaternion.Euler(0, rotY, 0), _prefabParent);
            int connect = _helper.GetConnectedFromShape(ComponentShape.Pass);
            
            DungeonPassMassData massData = new (dir, ComponentShape.Pass, go, connect);
            
            _passMassDic.Add(pos, massData);

            // ��ڂ̃}�X�Ǝn�_�ƏI�_���p�̃R���N�V�����ɒǉ�����
            // �����������ƌ����ڂ̏C�����̐��x���オ�邪�������ׂ����ˏオ��
            if ((i / 2 == 1) || i == 0 || i == dist - 1)
            {
                _fixPassSet.Add(pos);
            }
        }
    }

    /// <summary>�ʘH���C������̂Ő�ɒʘH�𐶐����Ă���K�v������</summary>
    internal void FixPassVisual()
    {
        foreach (Vector3Int pos in _fixPassSet)
        {
            // ���̍��W���O�㍶�E�ǂ̕����ɐڑ�����Ă��邩�Ō�����ύX
            // �����ڑ�����Ă��邩�őΉ����錩���ڂɕύX����
            (int binary, int connect) = _helper.GetNeighbourBinary(pos, _passMassDic.Keys);

            // �����̒ʘH�̏ꍇ�͏������Ȃ�
            if (connect == 2 && _helper.IsPassStraight(binary)) continue;

            float rotY;
            GameObject prefab;
            ComponentShape shape;

            switch (connect)
            {
                // �s���~�܂�
                case 1:
                    rotY = _helper.GetPassEndRotY(binary);
                    prefab = Instantiate(_passEndPrefab, pos, Quaternion.Euler(0, rotY, 0), _prefabParent);
                    shape = ComponentShape.PassEnd;

                    _waypointList.Add(pos);
                    break;
                // �p
                case 2:
                    rotY = _helper.GetCornerRotY(binary);
                    prefab = Instantiate(_cornerPrefab, pos, Quaternion.Euler(0, rotY, 0), _prefabParent);
                    shape = ComponentShape.Corner;
                    break;
                // �����H
                case 3:
                    rotY = _helper.GetTJunctionRotY(binary);
                    prefab = Instantiate(_tJunctionPrefab, pos, Quaternion.Euler(0, rotY, 0), _prefabParent);
                    shape = ComponentShape.TJunction;

                    _waypointList.Add(pos);
                    break;
                // �\���H
                case 4:
                    rotY = 0;
                    prefab = Instantiate(_crossPrefab, pos, Quaternion.Euler(0, rotY, 0), _prefabParent);
                    shape = ComponentShape.Cross;

                    _waypointList.Add(pos);
                    break;
                default:
                    Debug.LogError("�s����case�ł�: " + pos);
                    continue;
            }

            Direction dir = _helper.GetDirection(rotY);

            // �v���n�u��u��������̂Ō��̃v���n�u�͍폜����
            Destroy(_passMassDic[pos].Prefab);
            _passMassDic[pos].Replace(dir, shape, prefab, connect);
        }
    }

    /// <summary>�����̏o�����̐��ʂ̃}�X�𑀍삷��̂Ő�ɕ����𐶐����Ă����K�v������</summary>
    internal void FixConnectRoomEntrance(IReadOnlyDictionary<Vector3Int, Direction> roomEntranceDataDic)
    {
        foreach (KeyValuePair<Vector3Int, Direction> pair in roomEntranceDataDic)
        {
            Vector3Int roomPos = pair.Key;
            Direction roomDir = pair.Value;

            // �o�����̍��W�ƕ����������Ă���������畔���̐��ʂ̍��W�����߂�
            Vector3Int frontPos = roomPos - _helper.GetDirectionPos(roomDir);
            DungeonPassMassData frontmassData = _passMassDic[frontPos];

            float rotY;
            GameObject prefab;
            ComponentShape shape;

            // ���ʂ̃}�X�̐ڑ�����+1���āA�����̏o������ƌq�����������ڂɕύX����
            switch (++frontmassData.Connect)
            {
                // ����
                case 2 when roomDir == frontmassData.Dir:
                    rotY = _helper.GetPassStraightRotY(roomDir);
                    prefab = Instantiate(_passPrefab, frontPos, Quaternion.Euler(0, rotY, 0), _prefabParent);
                    shape = ComponentShape.Pass;
                    break;
                // �p
                case 2:
                    rotY = _helper.GetCornerRotY(roomDir, frontmassData.Dir);
                    prefab = Instantiate(_cornerPrefab, frontPos, Quaternion.Euler(0, rotY, 0), _prefabParent);
                    shape = ComponentShape.Corner;
                    break;
                // �����H
                case 3:
                    rotY = _helper.GetTJunctionRotY(roomDir, frontmassData.Dir, frontmassData.Shape);
                    prefab = Instantiate(_tJunctionPrefab, frontPos, Quaternion.Euler(0, rotY, 0), _prefabParent);
                    shape = ComponentShape.TJunction;
                    break;
                // �\���H
                case 4:
                    rotY = 0;
                    prefab = Instantiate(_crossPrefab, frontPos, Quaternion.Euler(0, rotY, 0), _prefabParent);
                    shape = ComponentShape.Cross;
                    break;
                default:
                    Debug.LogError("�s����case�ł�: " + roomPos);
                    continue;
            }

            Direction dir = _helper.GetDirection(rotY);

            // �v���n�u��u��������̂Ō��̃v���n�u�͍폜����
            Destroy(frontmassData.Prefab);
            frontmassData.Replace(dir, shape, prefab, frontmassData.Connect);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �y�ʉ������I���W�i���̃R���N�V�����N���X
/// </summary>
public class OriginSet<T> where T : IOriginSet<T>
{
    /* 
     *  ���p�ӏ��𒲍����̂��߁A���݂��̃N���X�͎g�p���Ȃ�
     */

    T[] _array;
    int _length;

    public OriginSet(int size)
    {
        _array = new T[size];
    }

    public void Add(T value)
    {
        // �z��Ɋi�[����l�̓Y�������i�[����z����ł̓Y�����ɕύX
        value.Index = _length;
        // �z��Ɋi�[
        _array[_length] = value;
        // ���i�[�����l��n���ă\�[�g
        SortUp(value);
        // ���ɒǉ�����ׂɓY�������C���N�������g
        _length++;
    }

    public T RemoveFirst()
    {
        // �擪�̃I�u�W�F�N�g����
        T firstItem = _array[0];
        // ���炷�̂Ō��炷
        _length--;
        // �擪�̒l����Ԍ��̒l�ɕύX
        _array[0] = _array[_length];
        // ��Ԍ�납�玝���Ă����l�̓Y������0(�擪)
        _array[0].Index = 0;
        // �擪��n���č~���\�[�g(�擪�̒l�̈ʒu�𒲐����邽��)
        SortDown(_array[0]);

        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int GetLength() => _length;

    public bool Contains(T value)
    {
        // �z����̓񕪖ؓ��̓Y�����̔ԍ��̒l�ƈ����̒l���������ǂ���
        return Equals(_array[value.Index], value);
    }

    void SortDown(T value)
    {
        while (true)
        {
            // �����̒l�̍��E�̓Y����
            int childIndexLeft = value.Index * 2 + 1;
            int childIndexRight = value.Index * 2 + 2;
            // �����p
            int swapIndex = 0;

            // ���݂̔z��̓Y������荶�Ȃ�
            if (childIndexLeft < _length)
            {
                // ������̓Y���������̓Y�����ɂ���
                swapIndex = childIndexLeft;

                // ���݂̓Y�������E�Ȃ�
                if(childIndexRight < _length)
                {
                    // ���̃A�C�e���ƉE�̃A�C�e�����r���āA���̕���������������
                    if (_array[childIndexLeft].CompareTo(_array[childIndexRight]) < 0)
                    {
                        // �����p�̓Y�������E��
                        swapIndex = childIndexRight;
                    }
                }

                // �����̒l�ƌ����Ώۂ̒l���ׂāA�����̒l�̕������������
                if (value.CompareTo(_array[swapIndex]) < 0)
                {
                    Swap(value, _array[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    void SortUp(T value)
    {
        // �����̒l�̐e�̓Y����
        int parentIndex = (value.Index - 1) / 2;
        while (true)
        {
            // �񕪖ؓ��̓Y�������g���Đe�̃A�C�e�����擾
            T parentItem = _array[parentIndex];
            // �����̒l�̕����e��菬�������ǂ���
            if (value.CompareTo(parentItem) > 0)
            {
                // ��������Ό���
                Swap(value, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (value.Index - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        // �������T�^�ɂ͓Y�����̃C���^�[�t�F�[�X���������Ă���̂�
        // ���̂悤�Ɍ������Ă������̓Y�����͂��̂܂܂Ȃ̂ł��̂悤�Ɍ����ł���
        _array[itemA.Index] = itemB;
        _array[itemB.Index] = itemA;
        
        // ���݂��̓Y�����̌���
        int itemAIndex = itemA.Index;
        itemA.Index = itemB.Index;
        itemB.Index = itemAIndex;
    }
}

/// <summary>
/// �񕪒T���؂ň������߂̓Y��������������C���^�[�t�F�[�X
/// </summary>
public interface IOriginSet<T> : IComparable<T>
{
    int Index { get; set; }
}

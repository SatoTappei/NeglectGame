using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 軽量化したオリジナルのコレクションクラス
/// </summary>
public class OriginSet<T> where T : IOriginSet<T>
{
    /* 
     *  利用箇所を調査中のため、現在このクラスは使用しない
     */

    T[] _array;
    int _length;

    public OriginSet(int size)
    {
        _array = new T[size];
    }

    public void Add(T value)
    {
        // 配列に格納する値の添え字を格納する配列内での添え字に変更
        value.Index = _length;
        // 配列に格納
        _array[_length] = value;
        // 今格納した値を渡してソート
        SortUp(value);
        // 次に追加する為に添え字をインクリメント
        _length++;
    }

    public T RemoveFirst()
    {
        // 先頭のオブジェクトを代入
        T firstItem = _array[0];
        // 減らすので減らす
        _length--;
        // 先頭の値を一番後ろの値に変更
        _array[0] = _array[_length];
        // 一番後ろから持ってきた値の添え字を0(先頭)
        _array[0].Index = 0;
        // 先頭を渡して降順ソート(先頭の値の位置を調整するため)
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
        // 配列内の二分木内の添え字の番号の値と引数の値が同じかどうか
        return Equals(_array[value.Index], value);
    }

    void SortDown(T value)
    {
        while (true)
        {
            // 引数の値の左右の添え字
            int childIndexLeft = value.Index * 2 + 1;
            int childIndexRight = value.Index * 2 + 2;
            // 交換用
            int swapIndex = 0;

            // 現在の配列の添え字より左なら
            if (childIndexLeft < _length)
            {
                // 交換基準の添え字を左の添え字にする
                swapIndex = childIndexLeft;

                // 現在の添え字より右なら
                if(childIndexRight < _length)
                {
                    // 左のアイテムと右のアイテムを比較して、左の方が小さかったら
                    if (_array[childIndexLeft].CompareTo(_array[childIndexRight]) < 0)
                    {
                        // 交換用の添え字を右に
                        swapIndex = childIndexRight;
                    }
                }

                // 引数の値と交換対象の値を比べて、引数の値の方が小さければ
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
        // 引数の値の親の添え字
        int parentIndex = (value.Index - 1) / 2;
        while (true)
        {
            // 二分木内の添え字を使って親のアイテムを取得
            T parentItem = _array[parentIndex];
            // 引数の値の方が親より小さいかどうか
            if (value.CompareTo(parentItem) > 0)
            {
                // 小さければ交換
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
        // 代入するT型には添え字のインターフェースを実装しているので
        // このように交換しても内部の添え字はそのままなのでこのように交換できる
        _array[itemA.Index] = itemB;
        _array[itemB.Index] = itemA;
        
        // お互いの添え字の交換
        int itemAIndex = itemA.Index;
        itemA.Index = itemB.Index;
        itemB.Index = itemAIndex;
    }
}

/// <summary>
/// 二分探索木で扱うための添え字を実装するインターフェース
/// </summary>
public interface IOriginSet<T> : IComparable<T>
{
    int Index { get; set; }
}

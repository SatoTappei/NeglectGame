internal struct DamageData
{
    internal int Quantity;
    internal float Range;
    // TreasureとEnemyについているタグコンポーネントをプレイヤー側にも対応させる
    // ダメージを与えるかどうかの判定が必要

    public DamageData(int quantity, float range)
    {
        Quantity = quantity;
        Range = range;
    }
}

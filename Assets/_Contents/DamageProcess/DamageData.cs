internal struct DamageData
{
    internal int Quantity;
    internal float Range;
    // Treasure��Enemy�ɂ��Ă���^�O�R���|�[�l���g���v���C���[���ɂ��Ή�������
    // �_���[�W��^���邩�ǂ����̔��肪�K�v

    public DamageData(int quantity, float range)
    {
        Quantity = quantity;
        Range = range;
    }
}

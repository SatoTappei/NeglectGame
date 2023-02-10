using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[����Ŏg�p����v�Z/����n�̃��\�b�h�𔲂��o�����w���p�[�N���X
/// </summary>
internal class ActorControlHelper
{
    internal string StateIDToString(StateID id)
    {
        switch (id)
        {
            case StateID.Appear: return "Appear";
            case StateID.Attack: return "Attack";
            case StateID.Joy: return "Joy";
            case StateID.LookAround: return "LookAround";
            case StateID.Panic: return "Panic";
        }

        Debug.LogError("�X�e�[�gID���o�^����Ă��܂���:" + id);
        return string.Empty;
    }
}

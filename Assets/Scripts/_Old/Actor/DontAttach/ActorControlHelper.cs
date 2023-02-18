using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[����Ŏg�p����v�Z/����n�̃��\�b�h�𔲂��o�����w���p�[�N���X
/// </summary>
internal class ActorControlHelper
{
    internal string StateIDToString(StateIDOld id)
    {
        switch (id)
        {
            case StateIDOld.Appear: return "Appear";
            case StateIDOld.Attack: return "Attack";
            case StateIDOld.Joy: return "Joy";
            case StateIDOld.LookAround: return "LookAround";
            case StateIDOld.Panic: return "Panic";
        }

        Debug.LogError("�X�e�[�gID���o�^����Ă��܂���:" + id);
        return string.Empty;
    }
}

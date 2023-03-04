using UnityEngine;

/// <summary>
/// Cursor����N���b�N�����ʒu�Ɍ�����Ray���΂��R���|�[�l���g
/// </summary>
public class CursorRayCaster : MonoBehaviour
{
    static readonly string TrapSettableFloorTag = "TrapSettableFloor";

    [SerializeField] TrapManager _trapManager;
    [Header("�L�����N�^�[���ړ��\��Layer")]
    [SerializeField] LayerMask _layerMask;

    bool _isActive;

    void Update()
    {
        if (!_isActive) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray();
        }
    }

    public void Active() => _isActive = true;

    void Ray()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _layerMask))
        {
            RaycastHit(hit);
        }
    }

    void RaycastHit(RaycastHit hit)
    {
        if (!hit.collider.CompareTag(TrapSettableFloorTag)) return;
        GameObject trap = _trapManager.TryGetTrap(hit.collider.transform.position);
        if (trap == null) return;

        AudioManager.Instance.PlaySE("SE_㩐ݒu");
        trap.transform.position = hit.collider.transform.position;
    }
}

using UnityEngine;

/// <summary>
/// Cursorからクリックした位置に向けてRayを飛ばすコンポーネント
/// </summary>
public class CursorRayCaster : MonoBehaviour, IPauseable
{
    static readonly string TrapSettableFloorTag = "TrapSettableFloor";

    [SerializeField] TrapManager _trapManager;
    [Header("キャラクターが移動可能なLayer")]
    [SerializeField] LayerMask _layerMask;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray();
        }
    }

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

        AudioManager.Instance.PlaySE("SE_罠設置");
        trap.transform.position = hit.collider.transform.position;
    }

    void IPauseable.Pause()
    {
        enabled = false;
    }
}

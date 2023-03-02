using UnityEngine;

/// <summary>
/// Cursorからクリックした位置に向けてRayを飛ばすコンポーネント
/// </summary>
public class CursorRayCaster : MonoBehaviour
{
    static readonly string TrapSettableFloorTag = "TrapSettableFloor";

    [SerializeField] TrapManager _trapManager;
    [Header("キャラクターが移動可能なLayer")]
    [SerializeField] LayerMask _layerMask;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _layerMask))
            {
                if (hit.collider.CompareTag(TrapSettableFloorTag))
                {
                    GameObject trap = _trapManager.TryGetTrap(hit.collider.transform.position);
                    if (trap != null)
                    {
                        trap.transform.position = hit.collider.transform.position;
                    }
                }
            }
        }
    }
}

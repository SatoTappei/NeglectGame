using UnityEngine;

public class MemoPad : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("メモ")]
    [TextArea(int.MaxValue,int.MaxValue)]
    [SerializeField] string _memo;
    [Header("メモ")]
    [TextArea(int.MaxValue, int.MaxValue)]
    [SerializeField] string _memo2;
    [Header("メモ")]
    [TextArea(int.MaxValue, int.MaxValue)]
    [SerializeField] string _memo3;
    [Header("メモ")]
    [TextArea(int.MaxValue, int.MaxValue)]
    [SerializeField] string _memo4;
    [Header("メモ")]
    [TextArea(int.MaxValue, int.MaxValue)]
    [SerializeField] string _memo5;
    [Header("メモ")]
    [TextArea(int.MaxValue, int.MaxValue)]
    [SerializeField] string _memo6;
#endif
}

using UnityEngine;

public class MemoPad : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("����")]
    [TextArea(int.MaxValue,int.MaxValue)]
    [SerializeField] string _memo;
    [Header("����")]
    [TextArea(int.MaxValue, int.MaxValue)]
    [SerializeField] string _memo2;
    [Header("����")]
    [TextArea(int.MaxValue, int.MaxValue)]
    [SerializeField] string _memo3;
    [Header("����")]
    [TextArea(int.MaxValue, int.MaxValue)]
    [SerializeField] string _memo4;
    [Header("����")]
    [TextArea(int.MaxValue, int.MaxValue)]
    [SerializeField] string _memo5;
    [Header("����")]
    [TextArea(int.MaxValue, int.MaxValue)]
    [SerializeField] string _memo6;
#endif
}

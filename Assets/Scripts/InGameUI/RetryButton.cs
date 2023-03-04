using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームをリトライするボタンのコンポーネント
/// </summary>
public class RetryButton : MonoBehaviour
{
    static readonly float DelayTime = 0.3f;

    [SerializeField] Button _button;

    void Start()
    {
        _button.onClick.AddListener(() => 
        {
            _button.interactable = false;

            DOVirtual.DelayedCall(DelayTime, ()=> 
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        });
    }
}

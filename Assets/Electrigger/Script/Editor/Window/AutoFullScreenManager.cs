using UnityEngine;

namespace Electrigger
{
    [DefaultExecutionOrder(-1000)] // 早めに実行されるように設定
    public class AutoFullScreenManager : MonoBehaviour
    {
        [SerializeField] private AutoFullScreenSettings settings;

        void Start()
        {
#if !UNITY_EDITOR
        if (settings != null && settings.enableFullScreen)
        {
            Screen.fullScreen = true;
        }
#endif
        }
    }
}
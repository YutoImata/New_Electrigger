using UnityEngine;
using TMPro;

namespace Electrigger
{
    /// <summary>
    /// プレイモードを表示するUIクラス
    /// </summary>
    public class PlayerModeUI : MonoBehaviour
    {
        [Header("スクリプトを参照")]
        [SerializeField] private MovementManager movementManager;

        [Header("UI設定")]
        [SerializeField] private TextMeshProUGUI modeText;

        [Header("表示テキスト")]

        [SerializeField] private string normalModeText = "通常";
        [SerializeField] private string wireModeText = "ワイヤー";


        private void OnEnable()
        {
            movementManager.OnModeChanged += UpdateModeText;
        }

        private void OnDisable()
        {

            movementManager.OnModeChanged -= UpdateModeText;

        }

        private void Start()
        {
            if (movementManager == null)
            {
                Debug.Log("MovementManagerをアタッチしてください");
                return;
            }
            UpdateModeText(movementManager.CurrentMode);
        }

        private void UpdateModeText(MovementMode mode)
        {
            switch (mode)
            {
                case MovementMode.Normal:
                    modeText.text = normalModeText;
                    break;
                case MovementMode.Wire:
                    modeText.text = wireModeText;
                    break;
                default:
                    modeText.text = "不明";
                    break;
            }
        }
    }
}
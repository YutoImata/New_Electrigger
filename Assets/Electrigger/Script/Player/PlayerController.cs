using UnityEngine;
using UnityEngine.InputSystem;

namespace Electrigger
{
    /// <summary>
    /// プレイヤーの移動を制御するクラス
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header("移動設定")]
        [SerializeField] private float moveSpeed = 5f;

        private Vector2 moveInput; // 入力された移動方向

        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            /* プレイヤーを移動を更新する */
            Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);// 入力値を3D移動方向へ変換
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World); // 時間補正付きでワールド空間上を移動
        }

        /// <summary>
        /// 移動スティックやキー入力を受け取る
        /// </summary>
        /// <param name="context"></param>
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }
    }
}
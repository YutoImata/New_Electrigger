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
        [SerializeField] private float jumpPower = 10f;

        [Header("地面判定")]
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private float groundCheckRadius = 0.3f;
        [SerializeField] private LayerMask groundLayerMask;


        private Vector2 moveInput; // 入力された移動方向
        private bool isGrounded;   // 地面に接触しているか
        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            /* 物理演算のためFixedUpdate内に書く */
            MovePlayer();
        }

        private void Update()
        {
            /* 地面判定のメソッドを呼ぶ */
            CheckGrounded();
        }

        /// <summary>
        /// プレイヤーの移動処理
        /// </summary>
        private void MovePlayer()
        {
            Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
            Vector3 targetVelocity = moveDirection * moveSpeed;

            /* 現在の垂直速度をそのまま引き継ぐ */
            targetVelocity.y = rb.linearVelocity.y;
            rb.linearVelocity = targetVelocity;
        }

        /// <summary>
        /// 地面に接触しているかを判別する
        /// </summary>
        private void CheckGrounded()
        {
            if (groundCheckPoint != null)
            {
                isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayerMask);
            }
        }

        /// <summary>
        /// 移動スティックやキー入力を受け取る
        /// </summary>
        /// <param name="context"></param>
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }


        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }

        /* ===============================
         * DEBUG: 地面判定の範囲を可視化
         * =============================== */
        private void OnDrawGizmos()
        {
            if (groundCheckPoint != null)
            {
                Gizmos.color = isGrounded ? Color.green : Color.red;
                Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
            }
        }
    }
}
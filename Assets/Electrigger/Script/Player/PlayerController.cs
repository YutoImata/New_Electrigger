using UnityEngine;
using UnityEngine.EventSystems;
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
            Vector3 inputDirection = new Vector3(moveInput.x, 0f, moveInput.y);

            /* カメラの前方と右方向を取得 */
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            /* カメラの方向に基づいて移動方向を決める */
            Vector3 moveDirection = cameraForward * inputDirection.z + cameraRight * inputDirection.x;

            /* 最終的な速度を設定 */
            Vector3 targetVelocity = moveDirection * moveSpeed;
            targetVelocity.y = rb.linearVelocity.y; // 垂直速度は維持
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
    }
}
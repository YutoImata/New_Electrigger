using UnityEngine;
using UnityEngine.InputSystem;

namespace Electrigger
{
    /// <summary>
    /// プレイヤーの移動を制御するクラス
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody))]
    public class NormalMovement : MonoBehaviour
    {
        [Header("移動設定")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpPower = 10f;

        [Header("地面判定")]
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private float groundCheckRadius = 0.3f;
        [SerializeField] private LayerMask groundLayerMask;

        private bool isGrounded;   // 地面に接触しているか
        private Rigidbody rb;

        private Transform cameraTransform;

        // カメラの方向ベクトルをキャッシュ
        private Vector3 cameraForward;
        private Vector3 cameraRight;

        /// <summary>
        /// RigidbodyとカメラのTransformを初期化
        /// </summary>
        /// <param name="rigidbody">
        public void Initialize(Rigidbody rigidbody, Transform camera)
        {
            rb = rigidbody;
            cameraTransform = camera;
        }

        /// <summary>
        /// 入力に基づいてプレイヤーの移動させる
        /// カメラの向きを基準に移動方向を計算し、Rigidbodyの速度を設定
        /// </summary>
        /// <param name="input"></param>
        public void HandleMovement(Vector2 input)
        {
            Vector3 inputDirection = new Vector3(input.x, 0f, input.y);

            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * inputDirection.z + cameraRight * inputDirection.x;

            Vector3 targetVelocity = moveDirection * moveSpeed;
            targetVelocity.y = rb.linearVelocity.y;
            rb.linearVelocity = targetVelocity;
        }

        public void HandleUpdate()
        {
            CheckGrounded();
            UpdateCameraVectors();
        }

        public void HandleJump()
        {
            if (isGrounded)
            {
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }

        /// <summary>
        /// モード開始時にこのコンポーネントを有効化
        /// </summary>
        public void OnModeEnter()
        {
            enabled = true;
        }

        /// <summary>
        /// モード終了時にこのコンポーネントを無効化
        /// </summary>
        public void OnModeExit()
        {
            enabled = false;
        }


        /// <summary>
        /// 地面に接触しているかどうか
        /// </summary>
        private void CheckGrounded()
        {
            if (groundCheckPoint != null)
            {
                isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayerMask);
            }
        }

        /// <summary>
        /// カメラのベクトルを更新
        /// </summary>
        private void UpdateCameraVectors()
        {
            if (cameraTransform != null)
            {
                cameraForward = cameraTransform.forward;
                cameraForward.y = 0f;
                cameraForward.Normalize();

                cameraRight = cameraTransform.right;
                cameraRight.y = 0f;
                cameraRight.Normalize();
            }
        }
    }
}
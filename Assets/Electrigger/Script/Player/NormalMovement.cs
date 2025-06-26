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

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

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
        }

        public void HandleJump()
        {
            if (isGrounded)
            {
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }

        private void CheckGrounded()
        {
            if (groundCheckPoint != null)
            {
                isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayerMask);
            }
        }

        public bool IsGrounded => isGrounded;
    }
}
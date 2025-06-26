using UnityEngine;

namespace Electrigger
{
    /// <summary>
    /// ワイヤーを使用して移動を制御するクラス
    /// </summary>
    public class WireMovement : MonoBehaviour
    {
        [Header("ワイヤー設定")]
        [SerializeField] private float wireSpeed = 15f;

        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void HandleMovement(Vector2 input)
        {
            Vector3 inputDirection = new Vector3(input.x, 0, input.y);

            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * inputDirection.z + cameraRight * inputDirection.x;
            rb.AddForce(moveDirection * wireSpeed, ForceMode.Acceleration);
        }

        public void HandleUpdate()
        {
            // 今後ワイヤーの更新処理を追加予定
        }

        public void HandleJump()
        {
            rb.AddForce(Vector3.up * 20f, ForceMode.Impulse);
        }
    }
}
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
        [SerializeField] private float jumpPower = 20f;
        [SerializeField] private float wireTargetRange = 100f; // ワイヤーターゲットの射程距離
        [SerializeField] private LayerMask wireTargetLayer = -1; // ワイヤーターゲット可能なレイヤー

        const float INPUT_THRESHOLD = 0.01f; // 入力のしきい値
        private Rigidbody rb;
        private Transform cameraTransform;
        private Vector3 cameraForward;
        private Vector3 cameraRight;

        /// <summary>
        /// RigidbodyとカメラのTransformを設定して初期化
        /// </summary>
        /// <param name="rigidbody"></param>
        /// <param name="camera"></param>
        public void Initialize(Rigidbody rigidbody, Transform camera)
        {
            rb = rigidbody;
            cameraTransform = camera;
        }

        /// <summary>
        /// 入力の大きさがしきい値を超えた場合に、ワイヤー移動の力を加える
        /// </summary>
        /// <param name="input"></param>
        public void HandleMovement(Vector2 input)
        {
            // 入力がない場合は処理をスキップ
            if (input.sqrMagnitude < INPUT_THRESHOLD) return;

            Vector3 inputDirection = new Vector3(input.x, 0, input.y);
            Vector3 moveDirection = cameraForward * inputDirection.z + cameraRight * inputDirection.x;

            rb.AddForce(moveDirection * wireSpeed, ForceMode.Acceleration);
        }

        /// <summary>
        /// 左クリックされたオブジェクトに向かってワイヤーで引っ張られる処理
        /// </summary>
        private void HandleWireTargeting()
        {
            if (Input.GetMouseButton(1))
            {
                Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

                Debug.DrawRay(ray.origin, ray.direction * wireTargetRange, Color.red, 200f);

                if (Physics.Raycast(ray, out RaycastHit hit, wireTargetRange, wireTargetLayer))
                {
                    Vector3 directionToTarget = (hit.point - rb.position).normalized;
                    rb.linearVelocity = Vector3.zero;
                    rb.AddForce(directionToTarget * wireSpeed, ForceMode.VelocityChange);
                }
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


        public void HandleUpdate()
        {
            UpdateCameraVectors();
            HandleWireTargeting();
        }

        /// <summary>
        /// ジャンプする処理
        /// </summary>
        public void HandleJump()
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
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
using UniGLTF.Extensions.VRMC_vrm;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Electrigger
{
    /// <summary>
    /// カメラコントローラー
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [Header("追従設定")]
        [SerializeField] private Transform target; // 追従対象
        [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, -2.5f); // カメラのオフセット

        [Header("追従の動作")]
        [SerializeField] private bool smoothFollow = true; // スムーズに追従するか
        [SerializeField] private float followSpeed = 10f; // 追従速度
        [SerializeField] private float rotationSpeed = 5f; // 回転速度

        [Header("カメラ制御")]
        [SerializeField] private bool enableMouseLook = true; // マウスでの視点操作
        [SerializeField] private float mouseSensitivity = 3f; // マウス感度
        [SerializeField] private bool invertY = false; // Y軸反転

        [Header("角度制限")]
        [SerializeField] private float minVerticalAngle = -60f; // 上下角度の最小値
        [SerializeField] private float maxVerticalAngle = 60f; // 上下角度の最大値

        [Header("距離制御")]
        [SerializeField] private bool enableZoom = true; // ズーム機能
        [SerializeField] private float minDistance = 1f; // 最小距離
        [SerializeField] private float maxDistance = 6f; // 最大距離
        [SerializeField] private float zoomSpeed = 3f; // ズーム速度

        [Header("障害物回避")]
        [SerializeField] private bool enableCollisionAvoidance = true; // 障害物回避
        [SerializeField] private LayerMask obstacleLayerMask = 1; // 障害物レイヤー
        [SerializeField] private float collisionBuffer = 0.1f; // 衝突バッファ

        /* プライベート変数 */
        private Vector2 lookInput; // 視点操作の入力
        private float currentDistance; // 現在の距離
        private float horizontalAngle; // 水平角度
        private float verticalAngle; // 垂直角度

        /* 初期値保存用 */
        private Vector3 initialOffset;
        private float initialDistance;

        private void Awake()
        {
            /* 初期値を保存 */
            initialOffset = offset;
            initialDistance = offset.magnitude;
            currentDistance = initialDistance;
            /* カーソルをロック */
            if (enableMouseLook)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private void Start()
        {
            // 初期角度を計算
            CalculateInitialAngles();
        }

        private void Update()
        {
            // ズーム処理を追加
            if (enableZoom)
            {
                float scroll = Input.mouseScrollDelta.y;
                if (scroll != 0)
                {
                    currentDistance = Mathf.Clamp(currentDistance - scroll * zoomSpeed, minDistance, maxDistance);
                }
            }
        }

        private void LateUpdate()
        {
            // カメラの位置と回転を更新
            UpdateCameraTransform();
        }

        /// <summary>
        /// 初期角度を計算
        /// </summary>
        private void CalculateInitialAngles()
        {
            if (target == null)
            {
                Debug.LogError("ターゲットがアタッチされていません" + target);
                return;
            }

            // FPS風：プレイヤーの向きに合わせる
            horizontalAngle = target.eulerAngles.y;
            verticalAngle = 0f; // 水平から開始
        }

        /// <summary>
        /// カメラの位置を回転を更新
        /// </summary>
        private void UpdateCameraTransform()
        {
            if (enableMouseLook)
            {
                UpdateCameraAngles();
            }

            Vector3 targetPosition = CalculateTargetPosition();

            if (enableCollisionAvoidance)
            {
                targetPosition = AvoidObstacles(targetPosition);
            }

            /* カメラの位置を更新 */
            UpdateCameraPosition(targetPosition);

            /* カメラの向きを更新（FPS風） */
            UpdateCameraRotation();
        }

        /// <summary>
        /// マウス入力による角度更新
        /// </summary>
        private void UpdateCameraAngles()
        {
            horizontalAngle += lookInput.x * mouseSensitivity;

            float verticalInput = invertY ? -lookInput.y : lookInput.y;
            verticalAngle += verticalInput * mouseSensitivity;

            verticalAngle = Mathf.Clamp(verticalAngle, minVerticalAngle, maxVerticalAngle); // 垂直角度を制限
        }

        /// <summary>
        /// 目標位置を計算
        /// </summary>
        /// <returns></returns>
        private Vector3 CalculateTargetPosition()
        {
            // 基本的な後方位置を計算
            Quaternion rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
            Vector3 rotatedOffset = rotation * Vector3.back * currentDistance;

            // 頭の高さに調整
            Vector3 headPosition = target.position + Vector3.up * 1.7f; // プレイヤーの頭の高さ
            
            return headPosition + rotatedOffset;
        }

        /// <summary>
        /// 障害物を回避した位置を計算
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        private Vector3 AvoidObstacles(Vector3 targetPosition)
        {
            Vector3 headPosition = target.position + Vector3.up * 1.7f;
            Vector3 direction = targetPosition - headPosition;
            float distance = direction.magnitude;

            /* レイキャストで障害物をチェック */
            if (Physics.Raycast(headPosition, direction.normalized, out RaycastHit hit, distance, obstacleLayerMask))
            {
                // 障害物に当たった場合、少し手前に配置
                float safeDistance = hit.distance - collisionBuffer;
                safeDistance = Mathf.Max(safeDistance, minDistance);

                return headPosition + direction.normalized * safeDistance;
            }

            return targetPosition;
        }

        /// <summary>
        /// カメラの位置を更新
        /// </summary>
        /// <param name="targetPosition"></param>
        private void UpdateCameraPosition(Vector3 targetPosition)
        {
            if (smoothFollow)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = targetPosition;
            }
        }

        /// <summary>
        /// カメラの向きを更新
        /// </summary>
        private void UpdateCameraRotation()
        {
            Quaternion targetRotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);

            if (smoothFollow)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = targetRotation;
            }

            Vector3 playerEuler = target.eulerAngles;
            playerEuler.y = horizontalAngle;
            target.rotation = Quaternion.Euler(playerEuler);
        }

        /// <summary>
        /// 視点操作の入力を受け取る
        /// </summary>
        /// <param name="context"></param>
        public void OnLook(InputAction.CallbackContext context)
        {
            if (enableMouseLook)
            {
                lookInput = context.ReadValue<Vector2>();
            }
        }

        /// <summary>
        /// ズームの入力を受け取る
        /// </summary>
        /// <param name="context"></param>
        public void OnZoom(InputAction.CallbackContext context)
        {
            if (!enableZoom) return;

            float zoomInput = context.ReadValue<float>();
            currentDistance = Mathf.Clamp(currentDistance - zoomInput * zoomSpeed, minDistance, maxDistance);
        }

        /// <summary>
        /// 追従対象を設定
        /// </summary>
        /// <param name="newTarget"></param>
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            if (target != null)
            {
                CalculateInitialAngles();
            }
        }

        /// <summary>
        /// カメラの位置をリセット
        /// </summary>
        public void ResetCamera()
        {
            offset = initialOffset;
            currentDistance = initialDistance;
            CalculateInitialAngles();
        }

        public void SetMouseLookEnabled(bool enable)
        {
            enableMouseLook = enable;
            Cursor.lockState = enable ? CursorLockMode.Locked : CursorLockMode.None;
        }

        /// <summary>
        /// デバッグ用ギズモ描画
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (target == null) return;

            // ターゲットとの接続線
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, target.position);

            // 障害物検知レイ
            if (enableCollisionAvoidance)
            {
                Vector3 direction = transform.position - target.position;
                Gizmos.color = Color.red;
                Gizmos.DrawRay(target.position, direction);
            }

            // 距離制限の可視化
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(target.position, minDistance);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(target.position, maxDistance);
        }
    }
}
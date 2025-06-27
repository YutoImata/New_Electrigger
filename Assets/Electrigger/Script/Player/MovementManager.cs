using UnityEngine;
using UnityEngine.InputSystem;

namespace Electrigger
{
    /// <summary>
    /// 移動システムを管理するクラス
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody))]
    public class MovementManager : MonoBehaviour
    {
        [Header("移動モード")]
        [SerializeField] private MovementMode currentMode = MovementMode.Normal;

        private NormalMovement normalMovement;
        private WireMovement wireMovement;
        private Vector2 moveInput;

        private Transform cameraTransform;

        private void Start()
        {
            // カメラを取得（"MainCamera" タグが必要）
            cameraTransform = Camera.main?.transform;

            if (cameraTransform == null)
            {
                Debug.LogError("Main Camera が見つかりません。MainCamera タグを確認してください。");
            }

            // カメラ参照を各移動クラスに渡す

            normalMovement = GetComponent<NormalMovement>();
            wireMovement = GetComponent<WireMovement>();

            normalMovement.Initialize(GetComponent<Rigidbody>(), cameraTransform);
            wireMovement.Initialize(GetComponent<Rigidbody>(), cameraTransform);
            // 初期設定
            SetMovementMode(currentMode);
        }

        private void FixedUpdate()
        {
            // 現在のモードに応じて移動処理
            switch (currentMode)
            {
                case MovementMode.Normal:
                    normalMovement.HandleMovement(moveInput);
                    break;
                case MovementMode.Wire:
                    wireMovement.HandleMovement(moveInput);
                    break;
            }
        }

        private void Update()
        {
            // 現在のモードに応じて更新処理
            switch (currentMode)
            {
                case MovementMode.Normal:
                    normalMovement.HandleUpdate();
                    break;
                case MovementMode.Wire:
                    wireMovement.HandleUpdate();
                    break;
            }
        }

        /// <summary>
        /// 現在のモードに応じてモードを有効にするメソッドを呼ぶ
        /// </summary>
        /// <param name="newMode"></param>
        public void SetMovementMode(MovementMode newMode)
        {
            /* 現在のモードを終了 */
            switch (currentMode)
            {
                case MovementMode.Normal:
                    normalMovement.OnModeExit();
                    break;
                case MovementMode.Wire:
                    wireMovement.OnModeExit();
                    break;
            }

            currentMode = newMode;

            /* 新しいモードを開始 */
            switch (currentMode)
            {
                case MovementMode.Normal:
                    normalMovement.OnModeEnter();
                    break;
                case MovementMode.Wire:
                    wireMovement.OnModeEnter();
                    break;
            }
        }

        /// <summary>
        /// 移動スティックやキー入力を受け取る
        /// </summary>
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        /// <summary>
        /// ジャンプする処理
        /// モードによってジャンプを変更する
        /// </summary>
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                switch (currentMode)
                {
                    case MovementMode.Normal:
                        normalMovement.HandleJump();
                        break;
                    case MovementMode.Wire:
                        wireMovement.HandleJump();
                        break;
                }
            }
        }

        /// <summary>
        /// プレイヤーの移動モードを通常と、ワイヤーで切り替えるトグル処理
        /// </summary>
        public void OnWireToggle(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                MovementMode newMode = currentMode == MovementMode.Normal
                    ? MovementMode.Wire
                    : MovementMode.Normal;
                SetMovementMode(newMode);
            }
        }
    }
}
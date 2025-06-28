using UnityEngine;

namespace Electrigger
{
    /// <summary>
    /// プレイヤーの全体のパラメーターを管理するクラス
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }

        [SerializeField] private int attackPower = 5;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            // スペースキーを押したら攻撃
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Space pressed! Attacking enemies...");

                // EnemyManagerが存在するか確認
                if (EnemyManager.Instance == null) return;

                foreach (var enemy in EnemyManager.Instance.Enemies)
                {
                    if (enemy != null)
                    {
                        enemy.TakeDamage(attackPower);
                    }
                }
            }
        }
    }
}
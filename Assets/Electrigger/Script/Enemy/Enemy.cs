using UnityEngine;

namespace Electrigger
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private int hp = 20;

        public void TakeDamage(int damage)
        {
            hp -= damage;
            Debug.Log($"ダメージを受けた：{damage} / 残りHP：{hp}");
            if (hp <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("HPが0になりましたt");
            gameObject.SetActive(false);
        }
    }
}
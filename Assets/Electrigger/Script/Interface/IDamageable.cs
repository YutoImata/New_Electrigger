namespace Electrigger
{
    /// <summary>
    /// ダメージを受けることができるオブジェクトに実装させるインターフェース
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// 指定されたダメージをオブジェクトに適用する
        /// </summary>
        /// <param name="damage">受けるダメージの量</param>
        void TakeDamage(int damage);
    }
}
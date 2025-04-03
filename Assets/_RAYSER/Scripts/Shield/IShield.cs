namespace Shield
{
    public interface IShield
    {
        /// <summary>
        /// シールド減少処理
        /// </summary>
        void ShieldReduction(float damage);

        /// <summary>
        /// 機体破壊処理
        /// </summary>
        void FuselageDestruction();

        /// <summary>
        /// 現在のシールドのプロパティ
        /// </summary>
        float CurrentShield { get; }
    }
}

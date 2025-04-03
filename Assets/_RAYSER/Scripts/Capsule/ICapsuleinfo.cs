using Status;

namespace Capsule
{
    /// <summary>
    /// カプセルインターフェース
    /// </summary>
    public interface ICapsuleinfo
    {
        /// <summary>
        /// カプセル名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// アイテム回収
        /// </summary>
        void CapsuleRecovery();

        /// <summary>
        /// カプセル効果発動
        /// </summary>
        void CapsuleEffectActivated();

        /// <summary>
        /// セットされているカプセルEnumを返却
        /// </summary>
        /// <returns>カプセルEnum</returns>
        CapsuleEnum GetCapsuleEnum();
    }
}

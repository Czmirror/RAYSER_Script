namespace _RAYSER.Scripts.SubWeapon
{
    /// <summary>
    /// サブウェポンの発射処理（Visitor）
    /// </summary>
    public interface ISubWeaponVisitor
    {
        void Visit(FireAction action);
        void Reset();
    }
}

namespace _RAYSER.Scripts.Bomb
{
    /// <summary>
    /// ボム有効シグナル
    /// </summary>
    public class BombActiveSignal
    {
        public BombActiveType BombActiveType { get; }

        public BombActiveSignal(BombActiveType bombActiveType)
        {
            BombActiveType = bombActiveType;
        }
    }

    public enum BombActiveType
    {
        Active,
        Inactive,
    }
}

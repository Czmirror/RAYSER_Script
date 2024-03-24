namespace _RAYSER.Scripts.Bomb
{
    /// <summary>
    /// ボム使用シグナル
    /// </summary>
    public class BombUseSignal
    {
        public BombUseType BombUseType { get; }

        public BombUseSignal(BombUseType bombUseType)
        {
            BombUseType = bombUseType;
        }
    }

    public enum BombUseType
    {
        Use,
        Stop,
    }
}

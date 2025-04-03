using Cysharp.Threading.Tasks;

namespace PlayerMove
{
    /// <summary>
    /// 回転用インターフェース
    /// </summary>
    public interface IRollingable
    {
        UniTaskVoid StartRolling();
    }
}

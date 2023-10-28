using Capsule;

namespace Event.Signal
{
    /// <summary>
    /// 自機カプセル取得通知処理
    /// </summary>
    public class PlayerGetCapsule
    {
        public ICapsuleinfo Capsuleinfo;

        public PlayerGetCapsule(ICapsuleinfo capsuleinfo)
        {
            Capsuleinfo = capsuleinfo;
        }
    }
}

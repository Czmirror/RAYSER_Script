using UnityEngine;

namespace Target
{
    /// <summary>
    /// 敵機のターゲット用インターフェース
    /// </summary>
    public interface IEnemyTarget
    {
        /// <summary>
        /// ターゲットのゲームオブジェクト
        /// </summary>
        GameObject Target
        {
            get;
            set;
        }

        /// <summary>
        /// ターゲット設定
        /// </summary>
        /// <param name="target">ターゲットとなるゲームオブジェクト</param>
        void TargetInitialize(GameObject target);

        /// <summary>
        /// ターゲット対象（自機）を返却する処理
        /// </summary>
        /// <returns>ターゲットのゲームオブジェクト</returns>
        GameObject CurrentTarget();
    }
}

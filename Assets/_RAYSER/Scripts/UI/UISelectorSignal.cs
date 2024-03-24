using UnityEngine;

namespace _RAYSER.Scripts.UI
{
    /// <summary>
    /// 選択UIのシグナル（UIの選択がnullになってしまうことがあるため、それを回避するための処理用）
    /// </summary>
    public class UISelectorSignal
    {
        /// <summary>
        /// 選択UIのゲームオブジェクト
        /// </summary>
        public GameObject forcusUIGameObject { get; set; }
    }
}

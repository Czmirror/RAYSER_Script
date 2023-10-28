using System;
using UnityEngine;

namespace _RAYSER.Scripts.Weapon
{
    /// <summary>
    /// サブウェポンUI
    /// </summary>
    public class SubWeaponUI : MonoBehaviour
    {
        /// <summary>
        /// 対象武器のインデックス
        /// </summary>
        [SerializeField] private int weaponIndex;

        private void Start()
        {
            // MessagePipe受信処理
            // this.Subscribe<CurrentSubWeaponIndex>(x =>
            // {
            //     if (x.currentSubWeaponIndex == weaponIndex)
            //     {
            //         // 選択中のサブウェポン
            //         Debug.Log("選択中のサブウェポン");
            //     }
            // }
        }
    }
}

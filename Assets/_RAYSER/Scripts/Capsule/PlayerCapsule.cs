using System;
using Damage;
using UnityEngine;

namespace Capsule
{
    /// <summary>
    /// 自機カプセル取得処理用クラス
    /// </summary>
    public class PlayerCapsule : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ICapsuleinfo itemInfo))
            {
                other.transform.GetComponent<ICapsuleinfo>().CapsuleRecovery();
            }
        }
    }
}

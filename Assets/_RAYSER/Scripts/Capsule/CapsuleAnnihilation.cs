using System;
using Event.Signal;
using Status;
using UniRx;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Capsule
{
    /// <summary>
    /// 敵機全滅カプセル処理
    /// </summary>
    public class CapsuleAnnihilation : MonoBehaviour, ICapsuleinfo
    {
        [SerializeField] private string _name = "AnnihilationEnemy";
        public string Name => _name;

        [SerializeField] private CapsuleEnum capsuleEnum = CapsuleEnum.AnnihilationEnemy;

        public void CapsuleRecovery()
        {
            MessageBroker.Default.Publish(new PlayerGetCapsule(this));
            Destroy(gameObject);
        }

        public void CapsuleEffectActivated()
        {
            MessageBroker.Default.Publish(new AnnihilationEnemy());
        }

        public CapsuleEnum GetCapsuleEnum()
        {
            return capsuleEnum;
        }
    }
}

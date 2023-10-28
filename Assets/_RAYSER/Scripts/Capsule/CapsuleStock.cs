using System;
using System.Collections.Generic;
using System.Linq;
using Event.Signal;
using UniRx;
using UnityEngine;

namespace Capsule
{
    /// <summary>
    /// カプセルEnum（独自UniRx）
    /// </summary>
    [SerializeField]
    public class CapsuleEnumReactiveProperty : ReactiveProperty<CapsuleEnum>
    {
        public CapsuleEnumReactiveProperty()
        {
        }

        public CapsuleEnumReactiveProperty(CapsuleEnum init) : base(init)
        {
        }
    }

    /// <summary>
    /// カプセルストック処理
    /// </summary>
    public class CapsuleStock : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        /// <summary>
        /// カプセル取得時のサウンド
        /// </summary>
        [SerializeField] private AudioClip capsuleRecoverySound;

        /// <summary>
        /// カプセル発動時のサウンド
        /// </summary>
        [SerializeField] private AudioClip capsuleEffectActivatedSound;

        /// <summary>
        /// カプセルストック
        /// </summary>
        Queue<ICapsuleinfo> capsuleStock = new Queue<ICapsuleinfo>();

        /// <summary>
        /// カプセルUI１つ目更新用UniRx変数
        /// </summary>
        public readonly CapsuleEnumReactiveProperty CapsuleUI1 = new CapsuleEnumReactiveProperty(CapsuleEnum.Empty);

        /// <summary>
        /// カプセルUI２つ目更新用UniRx変数
        /// </summary>
        public readonly CapsuleEnumReactiveProperty CapsuleUI2 = new CapsuleEnumReactiveProperty(CapsuleEnum.Empty);

        /// <summary>
        /// カプセルUI３つ目更新用UniRx変数
        /// </summary>
        public readonly CapsuleEnumReactiveProperty CapsuleUI3 = new CapsuleEnumReactiveProperty(CapsuleEnum.Empty);

        /// <summary>
        /// カプセル最大ストック数
        /// </summary>
        private int itemMaxStock = 3;

        private void Start()
        {
            MessageBroker.Default.Receive<PlayerGetCapsule>().Subscribe(x => CapsuleInsert(x.Capsuleinfo)).AddTo(this);
        }

        private void CapsuleInsert(ICapsuleinfo capsuleinfo)
        {
            // カプセル取得処理を排除して、カプセル一つで効果を発動に変更
            capsuleinfo.CapsuleEffectActivated();
            _audioSource.clip = capsuleEffectActivatedSound;
            _audioSource.Play();

            return;

            // カプセル格納
            capsuleStock.Enqueue(capsuleinfo);
            _audioSource.clip = capsuleRecoverySound;
            _audioSource.Play();

            CapsuleLimitCheck();

            CapsuleActivationCheck();

            CapsuleUIValueRefresh();
        }

        /// <summary>
        /// カプセルが上限を超えた場合、Dequeue処理
        /// </summary>
        private void CapsuleLimitCheck()
        {
            if (capsuleStock.Count > itemMaxStock)
            {
                capsuleStock.Dequeue();
            }
        }

        /// <summary>
        /// カプセル発動チェック
        /// </summary>
        private void CapsuleActivationCheck()
        {
            if (capsuleStock.Count() != itemMaxStock)
            {
                return;
            }

            // Classのままだと比較ができないため、Enumのみの配列を取得
            var itemCheck =
                from item in capsuleStock
                select item.GetCapsuleEnum();

            // 同じカプセルを揃えた場合、カプセル効果発動
            if (itemCheck.Distinct().Count() == 1)
            {
                var item = capsuleStock.Dequeue();
                item.CapsuleEffectActivated();
                capsuleStock.Clear();
                CapsuleUI1.Value = CapsuleEnum.Empty;
                CapsuleUI2.Value = CapsuleEnum.Empty;
                CapsuleUI3.Value = CapsuleEnum.Empty;

                _audioSource.clip = capsuleEffectActivatedSound;
                _audioSource.Play();
            }
        }

        /// <summary>
        /// UI用のUniRx変数更新処理
        /// </summary>
        private void CapsuleUIValueRefresh()
        {
            // UI更新用の一時的なキューを複製
            Queue<ICapsuleinfo> _tempItemStock = new Queue<ICapsuleinfo>(capsuleStock.ToArray());

            // UI更新用のラムダ式
            Func<Queue<ICapsuleinfo>, CapsuleEnum> itemUILamda = capsuleQueue =>
            {
                return (capsuleQueue.Count() == 0) ? CapsuleEnum.Empty : capsuleQueue.Dequeue().GetCapsuleEnum();
            };

            // UI用のUniRx変数更新
            CapsuleUI1.Value = itemUILamda(_tempItemStock);
            CapsuleUI2.Value = itemUILamda(_tempItemStock);
            CapsuleUI3.Value = itemUILamda(_tempItemStock);
        }
    }
}

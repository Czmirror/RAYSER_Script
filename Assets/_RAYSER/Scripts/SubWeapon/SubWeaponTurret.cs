using System;
using System.Collections.Generic;
using System.Threading;
using _RAYSER.Scripts.Item;
using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;

namespace _RAYSER.Scripts.SubWeapon
{
    /// <summary>
    /// サブウェポン発射処理
    /// </summary>
    public class SubWeaponTurret : MonoBehaviour, IDisposable
    {
        private ISubscriber<SubweaponUseSignal> _subweaponUseSignalSubscriber;
        private IDisposable _subweaponUseSignalSubscriberDisposable;
        private ItemAcquisition _itemAcquisition;
        private FireAction action;
        private CancellationTokenSource firingCancellationTokenSource;
        private float interval = 0.1f;
        private Dictionary<SubweaponUseType, Func<CancellationToken, UniTask>> subweaponActions;

        public void Setup(ISubscriber<SubweaponUseSignal> subweaponUseSignalSubscriber,
            ItemAcquisition itemAcquisition)
        {
            _subweaponUseSignalSubscriber = subweaponUseSignalSubscriber;
            var d = DisposableBag.CreateBuilder();
            _subweaponUseSignalSubscriber.Subscribe(OnSubweaponUseSignal).AddTo(d);
            _subweaponUseSignalSubscriberDisposable = d.Build();
            firingCancellationTokenSource = new CancellationTokenSource();
            _itemAcquisition = itemAcquisition;

            // SubweaponUseTypeに応じたActionの設定
            subweaponActions = new Dictionary<SubweaponUseType, Func<CancellationToken, UniTask>>
            {
                { SubweaponUseType.Use, StartFiringAsync },
                { SubweaponUseType.Stop, StopFiringAsync }
            };
        }

        private void OnSubweaponUseSignal(SubweaponUseSignal signal)
        {
            if (subweaponActions.TryGetValue(signal.SubweaponUseType, out var action))
            {
                action(firingCancellationTokenSource.Token).Forget();
            }
        }

        private async UniTask StartFiringAsync(CancellationToken cancellationToken)
        {
            firingCancellationTokenSource.Cancel();
            firingCancellationTokenSource = new CancellationTokenSource();

            // サブウェポン インスタンスのリセット
            var subWeapon = _itemAcquisition.GetSelectingSubWeapon();
            if (subWeapon == null)
            {
                return;
            }
            subWeapon.subWeaponVisitor.Reset();

            await FireContinuously(firingCancellationTokenSource.Token);
        }

        private async UniTask StopFiringAsync(CancellationToken cancellationToken)
        {
            firingCancellationTokenSource.Cancel();
            // 処理を終了
            await UniTask.Yield();
        }

        private async UniTask FireContinuously(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var subWeapon = _itemAcquisition.GetSelectingSubWeapon();
                if (subWeapon == null)
                {
                    break;
                }

                // 発射位置を更新
                action = new FireAction(transform.position, transform.rotation);

                // サブウェポン発射処理
                action.Accept(subWeapon.subWeaponVisitor);

                // 次の発射まで待機
                await UniTask.Delay(TimeSpan.FromSeconds(interval), cancellationToken: cancellationToken);
            }
        }

        public void Dispose()
        {
            _subweaponUseSignalSubscriberDisposable.Dispose();
            firingCancellationTokenSource.Cancel();
            firingCancellationTokenSource.Dispose();
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}

using System;
using MessagePipe;
using UnityEngine;

namespace _RAYSER.Scripts.Bomb
{
    /// <summary>
    /// ボム発射発動
    /// </summary>
    public class BombTurret : MonoBehaviour, IDisposable
    {
        private ISubscriber<BombUseSignal> _bombUseSignalSubscriber;
        private IDisposable _bombUseSignalSubscriberDisposable;
        private IBombVisitor _bombVisitor; // 現在のボムのVisitor

        public void Setup(ISubscriber<BombUseSignal> bombUseSignalSubscriber, IBombVisitor bombVisitor)
        {
            Debug.Log("BombTurret: Setupメソッドを呼び出しました");
            _bombUseSignalSubscriber = bombUseSignalSubscriber;
            var d = DisposableBag.CreateBuilder();

            _bombVisitor = bombVisitor;

            // SubscribeメソッドでサブスクリプションをDisposableBagに追加
            _bombUseSignalSubscriber.Subscribe(signal =>
            {
                Debug.Log("BombTurret: ボム使用シグナルを受け取りました");
                if (_bombVisitor.CanUse())
                {
                    Debug.Log("BombTurret: ボムを使用します");
                    var bombAction = new BombAction(transform.position);
                    bombAction.Accept(_bombVisitor);
                }
            }).AddTo(d);
            _bombUseSignalSubscriberDisposable = d.Build();
        }

        public void Dispose()
        {
            _bombUseSignalSubscriberDisposable?.Dispose();
        }

        private void OnDestroy()
        {
            // コンポーネントが破棄されるときに、Disposeメソッドを呼び出してリソースを解放する
            Dispose();
        }
    }
}

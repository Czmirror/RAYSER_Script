using System;
using MessagePipe;
using UnityEngine;

namespace _RAYSER.Scripts.Bomb
{
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

            _bombVisitor = bombVisitor; // ボムのVisitorをセット

            // SubscribeメソッドでサブスクリプションをDisposableBagに追加
            _bombUseSignalSubscriber.Subscribe(signal =>
            {
                Debug.Log("BombTurret: ボム使用シグナルを受け取りました");
                // ボム使用のシグナルを受け取ったら、VisitorのUseメソッドを呼び出す
                if (_bombVisitor.CanUse())
                {
                    Debug.Log("BombTurret: ボムを使用します");
                    _bombVisitor.Use(transform.position); // このオブジェクトの位置をUseメソッドに渡す
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

using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Status
{
    /// <summary>
    /// コンティーニュー用のステート管理クラス
    /// （既存のGameStatus、GameStateなども後ほど統合できるとよいが、影響範囲が広いため一旦保留）
    /// </summary>
    public class GameStateLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<GameStateService>(Lifetime.Singleton);
            builder.Register<GameStatePresenter>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GameStatePresenter>();
        }
    }
}

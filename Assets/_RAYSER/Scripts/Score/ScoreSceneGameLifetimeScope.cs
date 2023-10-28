using _RAYSER.Scripts.Score;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Score
{
    /// <summary>
    /// ゲームシーンのスコアのライフタイムスコープ
    /// ゲームシーンのGameObjectにアタッチして使用する
    /// </summary>
    public class ScoreSceneGameLifetimeScope : LifetimeScope
    {
        [SerializeField] private ScoreScreen _scoreScreen;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<ScoreService>(Lifetime.Singleton);
            builder.Register<ScoreDataPresenter>(Lifetime.Singleton);
            builder.RegisterEntryPoint<ScoreDataPresenter>();
            builder.RegisterComponent(_scoreScreen);
        }
    }
}

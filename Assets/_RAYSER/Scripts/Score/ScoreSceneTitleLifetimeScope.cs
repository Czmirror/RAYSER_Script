using _RAYSER.Scripts.Score;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Score
{
    /// <summary>
    /// タイトルのスコアのライフタイムスコープ
    /// タイトルシーンのGameObjectにアタッチして使用する
    /// </summary>
    public class ScoreSceneTitleLifetimeScope : LifetimeScope
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

using InputSystem;
using MessagePipe;
using Shield;
using TMPro;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _RAYSER.Scripts.Bomb
{
    public class BombLifetimeScope : LifetimeScope
    {

        [SerializeField] private BombTurret bombTurret;
        [SerializeField] private BombPrefab bomPrefab;
        [SerializeField] private ForceField forceField;
        [SerializeField] private PlayerShield playerShield;
        [SerializeField] private PlayerController playerController;

        /// <summary>
        /// ボムの使用回数表示UI
        /// </summary>
        [SerializeField] private TextMeshProUGUI bombUseCountText;

        private BombPresenter bombPresenter;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ForceField>(Lifetime.Singleton);
            builder.Register<PlayerInputSetupService>(Lifetime.Singleton);

            builder.RegisterBuildCallback(container =>
            {
                var setupService = container.Resolve<PlayerInputSetupService>();
                setupService.SetupPlayerController(playerController, container);

                forceField = container.Resolve<ForceField>();
                forceField.SetPrefab(bomPrefab);
                var bombActiveSignalPublisher = container.Resolve<IPublisher<BombActiveSignal>>();
                forceField.SetPublisher(bombActiveSignalPublisher);

                var bombUseSignalSubscriber = container.Resolve<ISubscriber<BombUseSignal>>();
                bombTurret.Setup(bombUseSignalSubscriber, forceField);

                playerShield.Setup(container.Resolve<ISubscriber<BombActiveSignal>>());

                bombPresenter = new BombPresenter(forceField, bombUseCountText);
                bombPresenter.Start();
            });
        }
    }
}

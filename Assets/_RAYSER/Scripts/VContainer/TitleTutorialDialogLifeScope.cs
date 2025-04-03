using _RAYSER.Scripts.UI.Title;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _RAYSER.Scripts.VContainer
{
    public class TitleTutorialDialogLifeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            var options = builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<TutorialDialogOpenSignal>(options);
            builder.RegisterMessageBroker<TutorialDialogCloseSignal>(options);
        }
    }
}

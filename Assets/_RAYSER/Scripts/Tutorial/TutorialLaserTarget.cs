using _RAYSER.Scripts.Event.Signal;
using UI.Game;
using UniRx;
using UnityEngine;

namespace _RAYSER.Scripts.Tutorial
{
    public class TutorialLaserTarget : MonoBehaviour
    {
        private void OnDestroy()
        {
            MessageBroker.Default.Publish(new TutorialItem(TalkEnum.TalkStart));
        }
    }
}

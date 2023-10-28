using Event;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Game
{
    public class GameoverButton : MonoBehaviour
    {
        public void PushButton()
        {
            MessageBroker.Default.Publish(new Gameover());
        }
    }
}

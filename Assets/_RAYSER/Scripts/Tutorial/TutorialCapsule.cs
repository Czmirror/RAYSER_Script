using System;
using _RAYSER.Scripts.Event.Signal;
using UI.Game;
using UniRx;
using UnityEngine;

namespace _RAYSER.Scripts.Tutorial
{
    public class TutorialCapsule : MonoBehaviour
    {
        private float _speed = 0.1f;
        private void Update()
        {
            //x.0の座標まで移動
            if (transform.position.x < 0.0f)
            {
                transform.position += new Vector3(_speed, 0, 0);
            }
        }

        private void OnDestroy()
        {
            MessageBroker.Default.Publish(new TutorialSubWeapon(TalkEnum.TalkStart));
        }
    }
}

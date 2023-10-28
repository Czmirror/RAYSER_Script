using System;
using System.Threading;
using _RAYSER.Scripts.Tweening;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using VRM;

namespace _RAYSER.Scripts.UI.Title
{
    public class MissionStartTalk : MonoBehaviour, ITalk
    {
        TweenExecution _tweenExecution = new TweenExecution();
        private IUIEffect _iuiImplementation;

        private float talkDelay = 1f;

        [SerializeField] private MouthAnimation roydMouthAnimation;
        [SerializeField] private MouthAnimation sophieMouthAnimation;


        public TweenExecution TweenExecution
        {
            get => _iuiImplementation.TweenExecution;
            set => _iuiImplementation.TweenExecution = value;
        }


        /// <summary>
        /// enum Characterをキーにして、MouthAnimationを取得する
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        private MouthAnimation GetMouthAnimation(Character.Character character)
        {
            switch (character)
            {
                case Character.Character.Royd:
                    return roydMouthAnimation;
                case Character.Character.Sophie:
                    return sophieMouthAnimation;
                default:
                    return null;
            }
        }

        /// <summary>
        /// トーク処理
        /// </summary>
        /// <param name="textMeshProUGUI"></param>
        /// <param name="character"></param>
        /// <param name="text"></param>
        /// <param name="speed"></param>
        /// <param name="cancellationToken"></param>
        public async UniTask Talk(TextMeshProUGUI textMeshProUGUI, Scripts.Character.Character character, string text, float speed,
            CancellationToken cancellationToken)
        {
            var mouthAnimation = GetMouthAnimation(character);
            mouthAnimation.MouthAnimationStart();
            await _tweenExecution.HideText(textMeshProUGUI, cancellationToken);
            await _tweenExecution.ShowText(textMeshProUGUI, text, speed, cancellationToken);
            mouthAnimation.MouthAnimationStop();

            await UniTask.Delay(TimeSpan.FromSeconds(talkDelay), cancellationToken: cancellationToken);
        }
    }
}

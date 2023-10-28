using System;
using _RAYSER.Scripts.Score;
using Score;
using UnityEngine;
using UnityEngine.Networking;
using UniRx;

namespace UI.Game
{
    public class TwitterButton : MonoBehaviour
    {
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private int score;

        private void Start()
        {
            _scoreCounter.ScoreObservable.Subscribe(x => score = x).AddTo(this);
        }

        public void Twitter()
        {
            var text = "Mission Complete!\nRAYSER(V3) Score " + score + "\nhttps://unityroom.com/games/rayser_v3";
            var hashtag = "RAYSER_V3,unity1week";

            //urlの作成
            string esctext = UnityWebRequest.EscapeURL(text);
            string esctag = UnityWebRequest.EscapeURL(hashtag);
            string url = "https://twitter.com/intent/tweet?text=" + esctext + "&hashtags=" + esctag;

            //Twitter投稿画面の起動

#if UNITY_EDITOR
            Application.OpenURL(url);
#elif UNITY_WEBGL
            // WebGLの場合は、ゲームプレイ画面と同じウィンドウでツイート画面が開かないよう、処理を変える
            Application.ExternalEval(string.Format("window.open('{0}','_blank')", url));
#else
            Application.OpenURL(url);
#endif
        }
    }
}

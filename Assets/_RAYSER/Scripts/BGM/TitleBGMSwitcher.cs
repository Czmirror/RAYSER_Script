using UnityEngine;

namespace BGM
{
    /// <summary>
    /// タイトル画面BGM制御クラス
    /// </summary>
    public class TitleBGMSwitcher : MonoBehaviour, ILoopSoundPlayer
    {
        /// <summary>
        /// タイトルBGM
        /// </summary>
        [SerializeField] LoopSound titleBGM;

        [SerializeField] private LoopSoundPlayer _loopSoundPlayer;

        public LoopSoundPlayer LoopSoundPlayer
        {
            get => _loopSoundPlayer;
            set => _loopSoundPlayer = value;
        }

        private void Start()
        {
            LoopSoundPlayer.ChangeLoopSound(titleBGM);
        }
    }
}

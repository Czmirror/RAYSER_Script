using System;

namespace _RAYSER.Scripts.Score
{
    /// <summary>
    /// スコア管理クラス
    /// </summary>
    public class ScoreData
    {
        private int _score;

        /// <summary>
        /// スコア更新イベント
        /// </summary>
        public event Action<int> OnScoreChanged;

        public int GetScore()
        {
            return _score;
        }

        public void SetScore(int score)
        {
            if (_score != score)
            {
                _score = score;
                OnScoreChanged?.Invoke(_score);
            }
        }
    }
}

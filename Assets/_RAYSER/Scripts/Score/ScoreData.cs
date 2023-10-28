namespace _RAYSER.Scripts.Score
{
    /// <summary>
    /// スコア管理クラス
    /// </summary>
    public class ScoreData
    {
        private int Score { get; set; }
        public int GetScore()
        {
            return Score;
        }

        public void SetScore(int score)
        {
            Score = score;
        }
    }
}

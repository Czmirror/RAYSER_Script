using Score;
using VContainer;

namespace _RAYSER.Scripts.Score
{
    public class ScoreService
    {
        private ScoreData scoreData;
        private ScoreScreen scoreScreen;

        [Inject]
        public void Construct(ScoreData scoreData, ScoreScreen scoreScreen)
        {
            this.scoreData = scoreData;
            this.scoreScreen = scoreScreen;
        }

        /// <summary>
        /// スコア加算処理
        /// </summary>
        /// <param name="score">スコア</param>
        public void AddScore(int score)
        {
            scoreData.SetScore(scoreData.GetScore() + score);
        }

        /// <summary>
        /// スコア表示処理
        /// </summary>
        public void ShowScore()
        {
            scoreScreen.ShowScore(scoreData.GetScore());
        }
    }
}

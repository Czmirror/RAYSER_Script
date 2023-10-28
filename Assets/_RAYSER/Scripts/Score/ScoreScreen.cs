using TMPro;
using UnityEngine;

namespace _RAYSER.Scripts.Score
{
    /// <summary>
    /// スコアのViewクラス
    /// </summary>
    public class ScoreScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreUI;

        public void ShowScore(int score)
        {
            scoreUI.text = score.ToString();
        }
    }
}

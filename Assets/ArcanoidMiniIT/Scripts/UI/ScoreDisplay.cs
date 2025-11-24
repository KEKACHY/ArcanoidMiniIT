using UnityEngine;
using TMPro;

namespace MiniIT.GAMEPLAY
{
    /// <summary>
    /// Отображает счет игрока на экране
    /// </summary>
    public class ScoreDisplay : MonoBehaviour
    {
        [Header("Компоненты")]
        [SerializeField]
        private TextMeshProUGUI scoreText = null;

        private void Start()
        {
            if(GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged += UpdateScore;
            }
        }

        private void UpdateScore(int newScore)
        {
            scoreText.text = $"{newScore}";
        }

        private void OnDestroy()
        {
            if(GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged -= UpdateScore;
            }    
        }
    }
}

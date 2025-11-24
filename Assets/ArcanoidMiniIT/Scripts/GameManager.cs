using UnityEngine;
using System;

namespace MiniIT.GAMEPLAY
{
    /// <summary>
    /// Управляет состоянием игры, подсчетом очков, проверкой условий побед/поражений
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance = null;
        public static GameManager Instance
        {
            get
            {
                return instance;
            }
        }

        [Header("Настройки")]
        [SerializeField]
        private int brickScoreValue = 10;
        private int totalBricksRemaining = 0; 
        private int currentScore = 0;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        [Obsolete]
        private void Start()
        {
            HealthHandler.OnObjectDestroyed += OnObjectDestroyedHandler;
        }

        private void OnDestroy()
        {
            HealthHandler.OnObjectDestroyed -= OnObjectDestroyedHandler;
        }

        private void OnObjectDestroyedHandler()
        {
            totalBricksRemaining--;
            AddScore(brickScoreValue);
            CheckWinCondition();
        }
        
        public void AddScore(int scoreValue)
        {
            currentScore += scoreValue;
        }
        
        private void CheckWinCondition()
        {
            if (totalBricksRemaining <= 0)
            {
                GameWon();
            }
        }
        
        private void GameWon()
        {
            Debug.Log("ПОБЕДА!");
            Time.timeScale = 0;
        }
        
        public void GameOver()
        {
            Debug.Log("ПОРАЖЕНИЕ!");
            Time.timeScale = 0;
        }
    }
}
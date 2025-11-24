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

        [Header("Компоненты")]
        [SerializeField]
        private HPPlayerHandler playerHealthHandler = null; 

        [SerializeField]
        private Transform paddleLaunchPosition = null; 

        [Header("Настройки")]
        [SerializeField]
        private int brickScoreValue = 1;

        private int totalBricksRemaining = 0; 
        private int currentScore = 0;
        private int activeBallsCount = 0;

        public event Action<int> OnScoreChanged;
        public event Action<int> OnBrickCountChanged;
        public event Action<int> OnHPCountChanged;
        public event Action<Vector3> OnNewBallSpawnNeeded;

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

        private void Start()
        {
            HPBrickHandler.OnObjectDestroyed += OnObjectDestroyedHandler;
            HPPlayerHandler.OnHealthChanged += OnPlayerHealthChanged;
            BallController.OnBallDestroyed += OnBallDestroyedHandler;
            if (playerHealthHandler != null)
            {
                OnHPCountChanged?.Invoke(playerHealthHandler.GetCurrentHealth());
            }
        }

        private void OnDestroy()
        {
            HPBrickHandler.OnObjectDestroyed -= OnObjectDestroyedHandler;
            HPPlayerHandler.OnHealthChanged -= OnPlayerHealthChanged;
            BallController.OnBallDestroyed -= OnBallDestroyedHandler;
        }

        public void AddActiveBall()
        {
            activeBallsCount++;
        }

        public void SetTotalBricks(int count)
        {
            totalBricksRemaining = count;
            OnBrickCountChanged?.Invoke(totalBricksRemaining);
            OnScoreChanged?.Invoke(currentScore);
        }
        private void OnBallDestroyedHandler()
        {
            activeBallsCount--;
            if (activeBallsCount <= 0)
            {
                playerHealthHandler.TakeDamage(1); 
                OnNewBallSpawnNeeded?.Invoke(paddleLaunchPosition.position); 
            }
        }

        private void OnObjectDestroyedHandler()
        {
            totalBricksRemaining--;
            AddScore(brickScoreValue);
            OnBrickCountChanged?.Invoke(totalBricksRemaining);
            OnScoreChanged?.Invoke(currentScore);
            CheckWinCondition();
        }

        private void OnPlayerHealthChanged(int newHealth)
        {
            OnHPCountChanged?.Invoke(newHealth);
            if (newHealth <= 0)
            {
                GameOver();
            }
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
            Time.timeScale = 0;
        }
        
        public void GameOver()
        {
            Time.timeScale = 0;
        }
    }
}
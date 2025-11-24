using System;
using UnityEngine;

namespace MiniIT.GAMEPLAY
{
    /// <summary>
    /// Отвечает за создание новых шаров
    /// </summary>
    public class BallSpawner : MonoBehaviour
    {
        [Header("Компоненты")]
        [SerializeField]
        private GameObject ballPrefab = null;
        [SerializeField]
        private Transform paddleLaunchPosition = null;

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnNewBallSpawnNeeded += SpawnBall;
                SpawnBall(paddleLaunchPosition.position); 
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnNewBallSpawnNeeded -= SpawnBall;
            }
        }

        /// <summary>
        /// Создает новый экземпляр шара в указанной позиции и регистрирует его.
        /// </summary>
        /// <param name="launchPosition">Позиция, в которой должен быть создан шар (Vector3)</param>
        public void SpawnBall(Vector3 launchPosition)
        {
            GameObject newBallObject = Instantiate(ballPrefab, launchPosition, Quaternion.identity);
            if (newBallObject.TryGetComponent(out BallController ballController))
            {
                ballController.InitializeInput(paddleLaunchPosition); 
            }
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddActiveBall();
            }
        }
    }
}

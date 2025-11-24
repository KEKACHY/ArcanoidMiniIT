using UnityEngine;
using System;

namespace MiniIT.GAMEPLAY
{
    /// <summary>
    /// Компонент реализующий логику здоровья, получения урона и смерти
    /// </summary>
    public class HPBrickHandler : MonoBehaviour
    {
        private int currentHealth = 0;
        public static event Action OnObjectDestroyed;

        public void SetHealth(int healthPoint)
        {
            currentHealth = healthPoint;
        }

        public void TakeDamage(int damageAmount)
        {
            currentHealth -= damageAmount;
            if (currentHealth <= 0)
            {
                HandleDestruction();
            }
        }

        private void HandleDestruction()
        {
            OnObjectDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
}
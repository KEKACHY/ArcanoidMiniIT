using UnityEngine;
using System;

namespace MiniIT.GAMEPLAY
{
    /// <summary>
    /// Компонент реализующий логику здоровья, получения урона и смерти
    /// </summary>
    public class HPPlayerHandler : MonoBehaviour
    {
        private int currentHealth = 0;
        public static event Action<int> OnHealthChanged;
        public int GetCurrentHealth() => currentHealth;

        public void SetHealth(int healthPoint)
        {
            currentHealth = healthPoint;
            OnHealthChanged?.Invoke(currentHealth);
        }

        public void TakeDamage(int damageAmount)
        {
            currentHealth -= damageAmount;
            OnHealthChanged?.Invoke(currentHealth);
        }

    }
}
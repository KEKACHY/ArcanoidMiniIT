using UnityEngine;

namespace MiniIT.GAMEPLAY
{
    /// <summary>
    /// Базовый класс кирпича
    /// </summary>
    [RequireComponent(typeof(HealthHandler))]
    public class BrickController : MonoBehaviour
    {
        [Header("Компоненты")]
        [SerializeField]
        private HealthHandler healthHandlerComponent = null;

        [Header("Настройки")]
        [SerializeField]
        private int healthPoint = 1;

        private void Awake()
        {
            healthHandlerComponent.SetHealth(healthPoint);
        }
    }
}
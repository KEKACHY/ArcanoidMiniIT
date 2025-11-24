using UnityEngine;

namespace MiniIT.GAMEPLAY
{
    /// <summary>
    /// Базовый класс кирпича
    /// </summary>
    [RequireComponent(typeof(HPBrickHandler))]
    public class BrickController : MonoBehaviour
    {
        [Header("Компоненты")]
        [SerializeField]
        private HPBrickHandler healthHandlerComponent = null;

        [Header("Настройки")]
        [SerializeField]
        private int healthPoint = 1;

        private void Awake()
        {
            healthHandlerComponent.SetHealth(healthPoint);
        }
    }
}
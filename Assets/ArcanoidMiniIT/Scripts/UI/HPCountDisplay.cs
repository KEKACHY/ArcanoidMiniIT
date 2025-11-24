using System.Collections.Generic;
using UnityEngine;


namespace MiniIT.GAMEPLAY
{
    /// <summary>
    /// Отображает количество хп игрока на экране
    /// </summary>
    public class HPCountDisplay : MonoBehaviour
    {
        [Header("Компоненты")]
        [Tooltip("Префаб иконки здоровья")]
        [SerializeField]
        private GameObject healthIconPrefab = null;

        [Tooltip("Родительский объект (контейнер) для размещения иконок.")]
        [SerializeField]
        private Transform iconContainer = null;

        private List<GameObject> activeIcons = new List<GameObject>();

        private void Start()
        {
            if(GameManager.Instance != null)
            {
                GameManager.Instance.OnHPCountChanged += UpdateHealthVisual;
            }
        }

        /// <summary>
        /// Обновляет визуальное представление здоровья, пересоздавая иконки.
        /// Вызывается при срабатывании события OnHPCountChanged.
        /// </summary>
        /// <param name="newHealth">Текущее целочисленное значение HP.</param>
        private void UpdateHealthVisual(int newHealth)
        {
            ClearOldIcons();
            for (int i = 0; i < newHealth; i++)
            {
                GameObject newIcon = Instantiate(healthIconPrefab, iconContainer);
                activeIcons.Add(newIcon);
            }
        }

        /// <summary>
        /// Удаляет все иконки из контейнера и очищает список activeIcons.
        /// </summary>
        private void ClearOldIcons()
        {
            for (int i = activeIcons.Count - 1; i >= 0; i--)
            {
                Destroy(activeIcons[i]); 
            }
            activeIcons.Clear();
        }

        private void OnDestroy()
        {
            if(GameManager.Instance != null)
            {
                GameManager.Instance.OnHPCountChanged -= UpdateHealthVisual;
            }    
        }
    }
}

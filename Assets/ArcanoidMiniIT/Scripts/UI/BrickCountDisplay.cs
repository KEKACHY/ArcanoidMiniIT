using UnityEngine;
using TMPro;

namespace MiniIT.GAMEPLAY
{
    /// <summary>
    /// Отображает количество кирпичей на экране
    /// </summary>
    public class BrickCountDisplay : MonoBehaviour
    {
        [Header("Компоненты")]
        [SerializeField]
        private TextMeshProUGUI brickCountText = null;

        private void Start()
        {
            if(GameManager.Instance != null)
            {
                GameManager.Instance.OnBrickCountChanged += UpdateBrickCount;
            }
        }

        private void UpdateBrickCount(int newCount)
        {
            brickCountText.text = $"{newCount}";
        }

        private void OnDestroy()
        {
            if(GameManager.Instance != null)
            {
                GameManager.Instance.OnBrickCountChanged -= UpdateBrickCount;
            }    
        }
    }
}

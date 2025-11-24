using UnityEngine;

namespace MiniIT.GAMEPLAY
{
    /// <summary>
    /// Строительство стены из кирпичей
    /// </summary>
    public class BrickBuilder : MonoBehaviour
    {
        [Header("Данные и Префабы")]
        [SerializeField]
        private BrickData brickData = null; 

        [Header("Настройки")]
        [Tooltip("Размер сетки")]
        [SerializeField]
        private Vector2 size = new Vector2(1f, 1f);
        [Tooltip("Пропуск (зазор) между центрами соседних кирпичей.")]
        [SerializeField]
        private Vector2 gap = new Vector2(1f, 1f);

        private void Start()
        {
            BuildWall();
        }
        
        private void OnDrawGizmosSelected()
        {
            if (isActiveAndEnabled)
            {
                float width = size.x; 
                float height = size.y;
                Vector3 centerOffset = new Vector3(width / 2f - 0.5f, -height / 2f + 0.5f, 0f);
                Vector3 newCenter = transform.position + centerOffset;

                Gizmos.color = Color.green; 
                Gizmos.DrawWireCube(newCenter, new Vector3(width, height, 0.1f));
            }
        }

        /// <summary>
        /// Генерирует кирпичи на сцене, автоматически рассчитывая размеры сетки.
        /// </summary>
        private void BuildWall()
        {
            if (brickData == null)
            {
                Debug.LogError("Ошибка: Brick Data не назначен.");
                return;
            }
            Vector3 lastPosition = new Vector3(transform.position.x - (gap.x + 1f), transform.position.y, transform.position.z);
            Vector3 lastScale = transform.localScale;
            bool fillFull = false;
            while(!fillFull)
            {
                int i = Random.Range(0, brickData.GetPrefabs().Length);
                Transform temp = TrySpawnBrick(i, lastPosition, lastScale);
                if(temp != null)
                {
                    temp.gameObject.isStatic = true;
                    lastPosition = temp.position;
                    lastScale = temp.localScale;
                }
                else
                {
                    fillFull = true;
                }
            }
        }

        /// <summary>
        /// Функция для расчета позиции объекта в сетке по Х
        /// </summary>
        /// <param name="lastX">позиция прошлого объекта по Х</param>
        /// <param name="lastS">размер прошлого объекта по Х</param>
        /// <param name="curS">размер текущего объекта по Х</param>
        /// <returns>Позиция текущего объекта по Х</returns>
        private float GetPosForX(float lastX, float lastS, float curS)
        {
            return lastX + lastS/2f + gap.x + curS/2f;
        }

        /// <summary>
        /// Спавнит объект, если он попадает в сетку, если нет, то берет объект меньшим размером
        /// Если объекты по размеру закончились, то значит никакой объект в сетку не поместится
        /// Поэтому переходит на строчку ниже, однако если ниже уже конец сетки, то null
        /// </summary>
        /// <param name="i">индекс объекта</param>
        /// <param name="lastP">позиция прошлого объекта</param>
        /// <param name="lastS">размер прошлого объекта</param>
        /// <returns>Возвращает созданный объект, либо null</returns>
        private Transform TrySpawnBrick(int i, Vector3 lastP, Vector3 lastS)
        {
            if(i < 0)
            {
                if(lastP.y - 2f < transform.position.y - size.y)
                {
                    return null;
                }
                else
                {
                    i = Random.Range(0, brickData.GetPrefabs().Length);
                    lastP = new Vector3(transform.position.x - (gap.x + 1f), lastP.y - (gap.y + 1f), transform.position.z);
                    lastS = transform.localScale;
                    return TrySpawnBrick(i, lastP, lastS);
                }
            }
            float currentS = brickData.GetPrefabByType(i).transform.localScale.x;
            float currentX = GetPosForX(lastP.x, lastS.x, currentS);
            if(currentX + currentS/2f < transform.position.x + size.x)
            {
                return Instantiate(brickData.GetPrefabByType(i), new Vector3(currentX, lastP.y, lastP.z), transform.rotation, transform).transform;
            }
            else
            {
                return TrySpawnBrick(i - 1, lastP, lastS);
            }
        }
    }
}
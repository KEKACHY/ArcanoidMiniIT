using System.Collections;
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
        [Tooltip("Задержка между спавном соседних кирпичей.")] 
        [SerializeField]
        private float spawnDelay = 0.05f;

        private bool isLeftToRight = true;

        private void Start()
        {
            StartCoroutine(BuildWall());
        }
        
        private void OnDrawGizmosSelected()
        {
            if (isActiveAndEnabled)
            {
                Vector3 centerOffset = new Vector3(0, -size.y / 2f + 0.5f, 0f);
                Vector3 newCenter = transform.position + centerOffset;

                Gizmos.color = Color.green; 
                Gizmos.DrawWireCube(newCenter, new Vector3(size.x, size.y, 0.1f));
            }
        }

        /// <summary>
        /// Генерирует кирпичи на сцене, автоматически рассчитывая размеры сетки.
        /// </summary>
        private IEnumerator BuildWall()
        {
            if (brickData == null)
            {
                Debug.LogError("Ошибка: Brick Data не назначен.");
                yield break;
            }
            float startX = transform.position.x - (size.x / 2f + gap.x);
            Vector3 lastPosition = new Vector3(startX, transform.position.y, transform.position.z);
            Vector3 lastScale = Vector3.zero;

            isLeftToRight = true;
            bool fillFull = false;
            int brickCount = 0;
            while(!fillFull)
            {
                int i = Random.Range(0, brickData.GetPrefabs().Length);
                Transform temp = TrySpawnBrick(i, ref lastPosition, ref lastScale);
                if(temp != null)
                {
                    temp.gameObject.isStatic = true;
                    brickCount++;
                    yield return new WaitForSeconds(spawnDelay);
                }
                else
                {
                    fillFull = true;
                    GameManager.Instance.SetTotalBricks(brickCount);
                }
            }
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
        private Transform TrySpawnBrick(int i, ref Vector3 lastP, ref Vector3 lastS)
        {
            float halfWallWidth = size.x / 2f;
            float wallCenter = transform.position.x;
            
            if (i < 0)
            {
                if (lastP.y - (lastS.y == 0 ? 1f : lastS.y + gap.y) < transform.position.y - size.y)
                {
                    return null; 
                }
                
                isLeftToRight = !isLeftToRight;
                
                i = Random.Range(0, brickData.GetPrefabs().Length);
                
                float startX;
                float newY = lastP.y - (lastS.y == 0 ? 1f : lastS.y + gap.y); 

                if (isLeftToRight)
                {
                    startX = wallCenter - (halfWallWidth + gap.x);
                }
                else
                {
                    startX = wallCenter + (halfWallWidth + gap.x);
                }

                lastP = new Vector3(startX, newY, lastP.z);
                lastS = Vector3.zero; 
                
                return TrySpawnBrick(i, ref lastP, ref lastS);
            }

            float currentS = brickData.GetPrefabByType(i).transform.localScale.x;
            
            float currentX;
            if (lastS.x == 0f)
            {
                if (isLeftToRight)
                {
                    currentX = wallCenter - halfWallWidth + currentS / 2f;
                }
                else
                {
                    currentX = wallCenter + halfWallWidth - currentS / 2f;
                }
            }
            else 
            {
                currentX = GetPosForX(lastP.x, lastS.x, currentS);
            }
            
            bool fits;
            if (isLeftToRight)
            {
                fits = (currentX + currentS / 2f) < (wallCenter + halfWallWidth);
            }
            else
            {
                fits = (currentX - currentS / 2f) > (wallCenter - halfWallWidth);
            }
            
            if(fits)
            {
                Transform newBrick = Instantiate(brickData.GetPrefabByType(i), 
                    new Vector3(currentX, lastP.y, lastP.z), transform.rotation, transform).transform;
                
                lastP = newBrick.position;
                lastS = newBrick.localScale;
                
                return newBrick;
            }
            else
            {
                return TrySpawnBrick(i - 1, ref lastP, ref lastS);
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
            if (isLeftToRight)
            {
                return lastX + lastS / 2f + gap.x + curS / 2f;
            }
            else
            {
                return lastX - lastS / 2f - gap.x - curS / 2f;
            }
        }
    }
}
using UnityEngine;

namespace MiniIT.GAMEPLAY
{
    /// <summary>
    /// Контейнер данных для хранения паттернов и префабов кирпичей
    /// </summary>
    [CreateAssetMenu(fileName = "BrickData", menuName = "Game/Brick Data")]
    public class BrickData : ScriptableObject
    {
        [Header("Префабы кирпичей")]
        [Tooltip("Желательно выставлять объект от меньшего к большему по размеру")]
        [SerializeField]
        private GameObject[] brickPrefabs = null;

        public GameObject GetPrefabByType(int type)
        {
            if (type >= 0 && type < brickPrefabs.Length)
            {
                return brickPrefabs[type];
            }
            Debug.LogError($"Ошибка: Не найден префаб для типа {type}. Проверьте массив Prefabs.");
            return null;
        }

        public GameObject[] GetPrefabs()
        {
            return brickPrefabs;
        }
    }
}
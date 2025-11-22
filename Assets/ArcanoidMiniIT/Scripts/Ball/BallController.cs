using UnityEngine;
using UnityEngine.InputSystem;

namespace MiniIT.GAMEPLAY
{
    /// <summary>
    /// Управление движением мяча, его запуском и перезапуском
    /// </summary>
    public class BallController : MonoBehaviour
    {
        [Header("Компоненты")]
        [SerializeField] 
        private Rigidbody rigidbodyComponent = null;
        [SerializeField] 
        private Transform paddleLaunchPosition = null;

        [Header("Настройки")]
        [SerializeField] private float speedBall = 10f;

        private InputSystem inputSystem = null;
        private bool launched = false;

        private void Awake()
        {
            inputSystem = new InputSystem();
            inputSystem.Player.Enable();
            
            inputSystem.Player.Launch.performed += OnLaunch;
        }

        private void Update()
        {
            if (rigidbodyComponent.linearVelocity.sqrMagnitude != speedBall * speedBall)
            {
                rigidbodyComponent.linearVelocity = Vector3.ClampMagnitude(rigidbodyComponent.linearVelocity, speedBall);
            }
        }

        private void OnLaunch(InputAction.CallbackContext context)
        {
            if (!launched)
            {
                launched = true;
                float randomX = Random.Range(-0.75f, 0.75f);
                float randomY = Random.Range(0.5f, 1f);
                Vector3 direction = new Vector3(randomX, randomY, 0f).normalized;

                rigidbodyComponent.linearVelocity = direction * speedBall;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            rigidbodyComponent.linearVelocity = Vector3.zero;
            rigidbodyComponent.angularVelocity = Vector3.zero;
            transform.position = paddleLaunchPosition.position;
            launched = false;
        }

        private void OnDestroy()
        {
            inputSystem.Player.Launch.performed -= OnLaunch;
        }
    }
}

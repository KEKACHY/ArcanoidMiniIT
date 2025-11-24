using System;
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

        [Header("Настройки")]
        [SerializeField] 
        private float speedBall = 10f;
        [SerializeField]
        private int damageAmount = 1;


        private InputSystem inputSystem = null;
        private Transform paddleLaunchPosition = null;
        private bool launched = false;
        public static event Action OnBallDestroyed;

        private void Awake()
        {
            inputSystem = new InputSystem();
            inputSystem.Player.Launch.performed += OnLaunch;
        }

        public void InitializeInput(Transform position)
        {
            inputSystem.Player.Enable();
            paddleLaunchPosition = position;
            launched = false;
        }

        private void FixedUpdate()
        {
            if(!launched)
            {
                transform.position = paddleLaunchPosition.position;
            }
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
                float randomX = UnityEngine.Random.Range(-0.75f, 0.75f);
                float randomY = UnityEngine.Random.Range(0.5f, 1f);
                Vector3 direction = new Vector3(randomX, randomY, 0f).normalized;

                rigidbodyComponent.linearVelocity = direction * speedBall;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.TryGetComponent(out HPBrickHandler healthHandler))
            {
                healthHandler.TakeDamage(damageAmount);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            OnBallDestroyed?.Invoke();
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            inputSystem.Player.Launch.performed -= OnLaunch;
            inputSystem.Player.Launch.canceled -= OnLaunch;
            inputSystem.Dispose();
        }
    }
}

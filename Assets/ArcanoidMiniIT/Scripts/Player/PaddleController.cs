using UnityEngine;
using UnityEngine.InputSystem;

namespace MiniIT.GAMEPLAY
{
    /// <summary>
    /// Горизонтальное управление Paddle игроком
    /// </summary>
    public class PaddleController : MonoBehaviour
    {
        [Header("Компоненты")]
        [SerializeField] 
        private Rigidbody rigidbodyComponent = null;

        [Header("Настройки")]
        [SerializeField] 
        private float speed = 10f;
        [SerializeField] 
        private float limitX = 7f;

        private InputSystem inputSystem = null;
        private float moveInput = 0f;

        private void Awake()
        {
            inputSystem = new InputSystem();
            inputSystem.Player.Enable();

            inputSystem.Player.Move.performed += OnMove;
            inputSystem.Player.Move.canceled += OnMove;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            Vector2 inputVector = context.ReadValue<Vector2>();
            moveInput = inputVector.x;
        }

        private void FixedUpdate()
        {
            float targetX = Mathf.Clamp(rigidbodyComponent.position.x + moveInput * speed * Time.fixedDeltaTime, -limitX, limitX);
            rigidbodyComponent.MovePosition(new Vector3(targetX, rigidbodyComponent.position.y, rigidbodyComponent.position.z));
        }

        private void OnDestroy()
        {
            inputSystem.Player.Move.performed -= OnMove;
            inputSystem.Player.Move.canceled -= OnMove;

            inputSystem.Dispose();
        }
    } 
}

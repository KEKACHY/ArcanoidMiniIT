using UnityEngine;
using UnityEngine.InputSystem;

namespace MiniIT.ARCAINOID.Player
{
    /// <summary>
    /// Горизонтальное управление Paddle игроком
    /// </summary>
    public class PaddleController : MonoBehaviour
    {
        [Header("Компоненты")]
        [SerializeField] private Rigidbody _rigidbody = null;

        [Header("Настройки управления")]
        [SerializeField] private float speed = 10f;
        [SerializeField] private float limitX = 7f;

        private InputSystem _inputSystem = null;
        private float moveInput = 0f;

        private void Awake()
        {
            _inputSystem = new InputSystem();
            _inputSystem.Player.Enable();

            _inputSystem.Player.Move.performed += OnMove;
            _inputSystem.Player.Move.canceled += OnMove;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            Vector2 inputVector = context.ReadValue<Vector2>();
            moveInput = inputVector.x;
        }

        private void FixedUpdate()
        {
            Vector3 velocity = new Vector3(moveInput * speed, 0f, 0f);
            _rigidbody.MovePosition(_rigidbody.position + velocity * Time.fixedDeltaTime);

            Vector3 pos = _rigidbody.position;
            pos.x = Mathf.Clamp(pos.x, -limitX, limitX);
            _rigidbody.position = pos;
        }

        private void OnDestroy()
        {
            _inputSystem.Player.Move.performed -= OnMove;
            _inputSystem.Player.Move.canceled -= OnMove;

            _inputSystem.Dispose();
        }
    } 
}

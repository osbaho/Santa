using Santa.Core;
using Santa.Infrastructure.Input;
using UnityEngine;
namespace Santa.Infrastructure.Combat
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(ActionPointComponentBehaviour))]
    [RequireComponent(typeof(ExplorationPlayerIdentifier))]
    public class Movement : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private InputReader inputReader;
        [SerializeField] private Animator animator; // <--- NUEVA REFERENCIA
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 720f;
        [SerializeField] private float gravityValue = -9.81f;
        private CharacterController _characterController;
        private Transform _mainCameraTransform;
        private Vector2 _moveInput;
        private Vector3 _playerVelocity;
        private bool _isGrounded;
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _mainCameraTransform = UnityEngine.Camera.main.transform;
            // Intentar buscar el animator automáticamente si no se asignó en el inspector
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
        }
        private void OnEnable()
        {
            if (inputReader != null) inputReader.MoveEvent += OnMove;
        }
        private void OnDisable()
        {
            if (inputReader != null) inputReader.MoveEvent -= OnMove;
        }
        private void OnMove(Vector2 moveInput)
        {
            _moveInput = moveInput;
        }
        private void Update()
        {
            _isGrounded = _characterController.isGrounded;
            if (_isGrounded && _playerVelocity.y < 0)
            {
                _playerVelocity.y = -2f;
            }
            Vector3 forward = _mainCameraTransform.forward;
            Vector3 right = _mainCameraTransform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();
            Vector3 desiredMoveDirection = (forward * _moveInput.y + right * _moveInput.x).normalized;
            _playerVelocity.y += gravityValue * Time.deltaTime;
            _characterController.Move((desiredMoveDirection * moveSpeed + _playerVelocity) * Time.deltaTime);
            if (desiredMoveDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(desiredMoveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
            // --- ESTA ES LA MAGIA ---
            if (animator != null)
            {
                // Le pasamos la magnitud del input (entre 0 y 1) al parámetro "Speed" que creamos antes
                animator.SetFloat("Speed", _moveInput.magnitude);
            }
        }
    }
}

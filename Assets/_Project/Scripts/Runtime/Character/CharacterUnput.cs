using System;
using _Project.Scripts.Runtime.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Runtime.Character
{
    public class CharacterInput : MonoBehaviour, IUpdateObserver
    {
        // Movement input values
        private Vector2 _movementInput;
        private bool _jumpInput;
        private bool _sprintInput;
        private bool _crouchInput;
    
        // Input actions reference
        private PlayerInput _playerInput;
        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _sprintAction;
        private InputAction _crouchAction;
    
        // Events for other components to subscribe to

        public event Action OnJumpPressed;
        public event Action<bool> OnSprintChanged;
        public event Action<bool> OnCrouchChanged;
    
        private void Awake()
        {
            // Get the PlayerInput component
            _playerInput = GetComponent<PlayerInput>();
        
            // Cache the references to actions
            _moveAction = _playerInput.actions["Move"];
            _jumpAction = _playerInput.actions["Jump"];
            _sprintAction = _playerInput.actions["Sprint"];
            _crouchAction = _playerInput.actions["Crouch"];
        }
    
        private void OnEnable()
        {
            // Subscribe to Input System events
            _jumpAction.performed += OnJumpPerformed;
            _sprintAction.performed += OnSprintPerformed;
            _sprintAction.canceled += OnSprintCanceled;
            _crouchAction.performed += OnCrouchPerformed;
            _crouchAction.canceled += OnCrouchCanceled;
        
            // Register with UpdateManager
            UpdateManager.RegisterUpdateObserver(this);
        }
    
        private void OnDisable()
        {
            // Unsubscribe from Input System events
            _jumpAction.performed -= OnJumpPerformed;
            _sprintAction.performed -= OnSprintPerformed;
            _sprintAction.canceled -= OnSprintCanceled;
            _crouchAction.performed -= OnCrouchPerformed;
            _crouchAction.canceled -= OnCrouchCanceled;
        
            // Unregister from UpdateManager
            UpdateManager.UnregisterUpdateObserver(this);
        }
    
        // Using ObservedUpdate instead of regular Update (from UpdateManager)
        public void ObservedUpdate()
        {
            // Read the movement input every frame
            _movementInput = _moveAction.ReadValue<Vector2>();
        }
    
        // Event callbacks
        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            _jumpInput = true;
            OnJumpPressed?.Invoke();
        }
    
        private void OnSprintPerformed(InputAction.CallbackContext context)
        {
            _sprintInput = true;
            OnSprintChanged?.Invoke(true);
        }
    
        private void OnSprintCanceled(InputAction.CallbackContext context)
        {
            _sprintInput = false;
            OnSprintChanged?.Invoke(false);
        }
        
        private void OnCrouchPerformed(InputAction.CallbackContext context)
        {
            _crouchInput = true;
            OnCrouchChanged?.Invoke(true);
        }
    
        private void OnCrouchCanceled(InputAction.CallbackContext context)
        {
            _crouchInput = false;
            OnCrouchChanged?.Invoke(false);
        }
    
        // Public accessor methods
        public Vector2 GetMovementInput() => _movementInput;
        public bool GetJumpInput() 
        {
            var result = _jumpInput;
            _jumpInput = false; // Consume the input
            return result;
        }
        public bool GetSprintInput() => _sprintInput;
        public bool GetCrouchInput() => _crouchInput;
    }
}

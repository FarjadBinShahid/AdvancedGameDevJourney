// CharacterController.cs - Main controller

using System;
using _Project.Scripts.Runtime.Character.States;
using _Project.Scripts.Runtime.Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Runtime.Character
{
    public class CharacterController : MonoBehaviour, IUpdateObserver, IFixedUpdateObserver
    {
    
        [SerializeField] private CharacterInput input;

        // References to components
        [SerializeField] private Rigidbody rb;
        
        // Configuration
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float crouchSpeed = 2.5f;
        [SerializeField] private float runSpeed = 10f;
        [SerializeField] private float jumpForce = 8f;
        [SerializeField] private float groundCheckDistance = 0.2f;

        private CharacterState _currentState;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            // animator = GetComponent<Animator>(); // Uncomment if using animator
        
            if (input == null)
                input = GetComponent<CharacterInput>();
        }
        private void Start()
        {
            ChangeState(new IdleState(this));
        }

        private void OnEnable()
        {
            UpdateManager.RegisterUpdateObserver(this);
            UpdateManager.RegisterFixedUpdateObserver(this);
        }

        private void OnDisable()
        {
            UpdateManager.UnregisterUpdateObserver(this);
            UpdateManager.UnregisterFixedUpdateObserver(this);
        }

        public void ObservedUpdate()
        {
            // Check for state transitions
            CharacterState newState = _currentState.CheckSwitchState();
            if (newState != null)
            {
                ChangeState(newState);
            }
        
            _currentState.Update();
        }
        
    
        public void ObservedFixedUpdate()
        {
            _currentState?.FixedUpdate();
        }
    
        public void ChangeState(CharacterState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
    
        // Accessor methods for state use
        public float GetWalkSpeed() => walkSpeed;
        public float GetCrouchSpeed() => crouchSpeed;
        public float GetRunSpeed() => runSpeed;
        public float GetJumpForce() => jumpForce;
        public Rigidbody GetRigidbody() => rb;
    }
}
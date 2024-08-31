using UnityEngine;
using UnityEngine.InputSystem;

namespace GDT1.Inputs
{
    [RequireComponent(typeof(PlayerInput))]
    public class MoveInput : MonoBehaviour
    {
        [SerializeField]
        private Transform _objectToMove;
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _rotationSpeed;

        [SerializeField]
        private float _zLevelStep = 0.5f; // The step size for changing z-level
        private float _currentZLevel = 1f; // Default starting z-level

        private Vector3 _moveDirection = Vector3.zero;

        // To store the normal, small, and big sizes
        private Vector3 _normalSize;
        private Vector3 _smallSize;
        private Vector3 _bigSize;

        void Start()
        {
            // Store the initial scale as the normal size
            _normalSize = _objectToMove.localScale;

            // Calculate small and big sizes based on the normal size. WHY DOESN'T THIS WORK?
            _smallSize = _normalSize - new Vector3(2f, 2f, 0f);
            _bigSize = _normalSize + new Vector3(2f, 2f, 0f);
        }

        void Update()
        {
            MakeMove();
            MakeRotate();
            CheckForZLevelChange();
        }

        private void MakeMove()
        {
            Vector3 newPosition = _objectToMove.position + _moveDirection * _speed * Time.deltaTime;
            newPosition.z = _currentZLevel; // Ensure the z-level remains constant during movement
            _objectToMove.position = newPosition;
        }

        private void MakeRotate()
        {
            if (_moveDirection != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, _moveDirection);
                _objectToMove.rotation = Quaternion.RotateTowards(_objectToMove.rotation, rotation, _rotationSpeed * Time.deltaTime);
            }
        }

        private void CheckForZLevelChange()
        {
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                _currentZLevel = Mathf.Max(_currentZLevel - _zLevelStep, 0.5f); // Decrease z level
                UpdateScale();
            }
            else if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                _currentZLevel = Mathf.Min(_currentZLevel + _zLevelStep, 1.5f); // Increase z level
                UpdateScale();
            }
        }

        private void UpdateScale()
        {
            // Adjust the scale based on the current z-level
            if (_currentZLevel == 0.5f)
            {
                _objectToMove.localScale = _smallSize;
            }
            else if (_currentZLevel == 1.0f)
            {
                _objectToMove.localScale = _normalSize;
            }
            else if (_currentZLevel == 1.5f)
            {
                _objectToMove.localScale = _bigSize;
            }
        }

        public void MoveDirection(Vector2 direction)
        {
            _moveDirection = new Vector3(direction.x, direction.y, 0f);
        }

        public void OnMove(InputValue value)
        {
            _moveDirection = value.Get<Vector2>();
        }
    }
}

using System.Collections;
using UnityEngine;

namespace GDT1.AIBot
{
    public class AIMove : MonoBehaviour
    {
        [SerializeField]
        private CircleCollider2D _triggerCollider;
        [SerializeField]
        private float _detectionRadius = 5f;
        [SerializeField]
        private float _timeBetweenChose = 5f;
        [SerializeField]
        [Tooltip("Acceleration gained when a player is detected, in %")]
        [Range(0f, 1f)]
        private float _acceleration = 0.5f;

        [SerializeField]
        private Transform _objectToMove;
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _rotationSpeed;

        [SerializeField]
        private Vector2 _minBounds = new Vector2(-20, -20); // Default min bounds
        [SerializeField]
        private Vector2 _maxBounds = new Vector2(20, 20); // Default max bounds

        [SerializeField]
        private float _minSpawnDistanceFromPlayer = 1f; // Minimum distance from player after spawning

        private Transform _detectedPlayer;
        private Vector3 _moveDirection;
        private float _currentZLevel = 1f; // Default z level
        private bool _canDetectPlayer = false; // Flag to prevent immediate detection

        // Z-levels and corresponding scales
        private readonly float[] zLevels = { 0.5f, 1.0f, 1.5f };
        private readonly float[] scales = { 0.8f, 1.15f, 1.5f };

        void Start()
        {
            _triggerCollider.radius = _detectionRadius;
            StartCoroutine(StartMovingAfterDelay(1f)); // Add delay before detecting and moving
            UpdateScale(); // Ensure initial scale is set
        }

        private void Update()
        {
            if (_detectedPlayer != null && _canDetectPlayer)
            {
                // Calculate the direction towards the player but keep the z level constant
                _moveDirection = new Vector3(
                    _detectedPlayer.position.x - gameObject.transform.position.x,
                    _detectedPlayer.position.y - gameObject.transform.position.y,
                    0f
                );

                // Check if within 1f range and same z level to "attack"
                float distance = Vector2.Distance(new Vector2(_objectToMove.position.x, _objectToMove.position.y),
                                                  new Vector2(_detectedPlayer.position.x, _detectedPlayer.position.y));

                if (distance <= 1f && Mathf.Approximately(_objectToMove.localScale.x, _detectedPlayer.localScale.x))
                {
                    AttackPlayer();
                }
            }

            MakeMove();
            MakeRotate();
        }

        private void UpdateScale()
        {
            // Find the index of the current z-level in the array
            int zIndex = System.Array.IndexOf(zLevels, _currentZLevel);
            if (zIndex >= 0)
            {
                float scale = scales[zIndex];
                _objectToMove.localScale = new Vector3(scale, scale, 1f);

                // Debugging: Log the new scale and z-level to ensure it's being calculated and applied correctly
                Debug.Log($"Enemy new scale: {_objectToMove.localScale} at z-level: {_currentZLevel}");
            }
            else
            {
                Debug.LogWarning("Z-level not found in predefined levels. No scale change applied.");
            }
        }

        private IEnumerator StartMovingAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _canDetectPlayer = true; // Allow the AI to start detecting the player after the delay
            StartMoving();
        }

        public IEnumerator ChooseDirection()
        {
            while (true)
            {
                _moveDirection = Random.insideUnitCircle;
                yield return new WaitForSeconds(_timeBetweenChose);
                UpdateScale(); // Update scale during movement to reflect any changes in z-level
            }
        }

        private void MakeMove()
        {
            float speed = _detectedPlayer == null ? _speed : _speed + _speed * _acceleration;
            Vector3 newPosition = _objectToMove.position + _moveDirection.normalized * speed * Time.deltaTime;

            // Clamp the new position to stay within the playground bounds
            newPosition.x = Mathf.Clamp(newPosition.x, _minBounds.x, _maxBounds.x);
            newPosition.y = Mathf.Clamp(newPosition.y, _minBounds.y, _maxBounds.y);
            // No need to change z position, we are adjusting scale instead

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

        private void AttackPlayer()
        {
            // Logic for attacking or killing the player
            Debug.Log("Player attacked!");
            // Implement the logic for damaging or killing the player here
        }

        private void StartMoving()
        {
            StartCoroutine(ChooseDirection());
        }

        private void StopMoving()
        {
            StopCoroutine(ChooseDirection());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_canDetectPlayer && collision.gameObject.CompareTag("Player"))
            {
                float distanceToPlayer = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), 
                                                          new Vector2(collision.transform.position.x, collision.transform.position.y));

                // Only detect the player if far enough from the spawn point
                if (distanceToPlayer >= _minSpawnDistanceFromPlayer)
                {
                    _detectedPlayer = collision.gameObject.transform;
                    _currentZLevel = _detectedPlayer.position.z; // Match z-level
                    UpdateScale(); // Update scale immediately to reflect matching z-level
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                _detectedPlayer = null;
                _moveDirection = Random.insideUnitCircle;
                _currentZLevel = 1f; // Reset to default z-level when player is no longer detected
                UpdateScale(); // Revert scale when the player is out of range
            }
        }
    }
}

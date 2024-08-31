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
        private Vector2 _minBounds = new Vector2(-20, -20);
        [SerializeField]
        private Vector2 _maxBounds = new Vector2(20, 20);

        [SerializeField]
        private float _minSpawnDistanceFromPlayer = 1f;

        private Transform _detectedPlayer;
        private Vector3 _moveDirection;
        private float _currentZLevel = 1f;
        private bool _canDetectPlayer = false;

        private Vector3 _normalSize;
        private Vector3 _smallSize;
        private Vector3 _bigSize;

        void Start()
        {
            _triggerCollider.radius = _detectionRadius;
            StartCoroutine(StartMovingAfterDelay(1f));

            // Store the initial scale as the normal size
            _normalSize = _objectToMove.localScale;

            // Calculate small and big sizes based on the normal size
            _smallSize = _normalSize - new Vector3(2f, 2f, 0f);
            _bigSize = _normalSize + new Vector3(2f, 2f, 0f);

            UpdateScale();
        }

        private void Update()
        {
            if (_detectedPlayer != null && _canDetectPlayer)
            {
                _moveDirection = new Vector3(
                    _detectedPlayer.position.x - gameObject.transform.position.x,
                    _detectedPlayer.position.y - gameObject.transform.position.y,
                    0f
                );

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

        private IEnumerator StartMovingAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _canDetectPlayer = true;
            StartMoving();
        }

        public IEnumerator ChooseDirection()
        {
            while (true)
            {
                _moveDirection = Random.insideUnitCircle;
                yield return new WaitForSeconds(_timeBetweenChose);
                UpdateScale();
            }
        }

        private void MakeMove()
        {
            float speed = _detectedPlayer == null ? _speed : _speed + _speed * _acceleration;
            Vector3 newPosition = _objectToMove.position + _moveDirection.normalized * speed * Time.deltaTime;

            newPosition.x = Mathf.Clamp(newPosition.x, _minBounds.x, _maxBounds.x);
            newPosition.y = Mathf.Clamp(newPosition.y, _minBounds.y, _maxBounds.y);

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
            Debug.Log("Player attacked!");
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

                if (distanceToPlayer >= _minSpawnDistanceFromPlayer)
                {
                    _detectedPlayer = collision.gameObject.transform;
                    _currentZLevel = _detectedPlayer.position.z;
                    UpdateScale();
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                _detectedPlayer = null;
                _moveDirection = Random.insideUnitCircle;
                _currentZLevel = 1f;
                UpdateScale();
            }
        }
    }
}

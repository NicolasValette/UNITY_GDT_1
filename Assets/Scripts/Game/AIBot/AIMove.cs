
using System.Collections;
using UnityEngine;

namespace GDT1.AIBot
{
    public class AIMove : MonoBehaviour
    {
        [SerializeReference]
        private CircleCollider2D _triggerCollider;
        [SerializeReference]
        private float _detectionRadius = 5f;
        [SerializeReference]
        private float _timeBetweenChose = 5f;        
        [SerializeReference]
        [Tooltip("Acceleration gagné lors d'une detection, en %)")]
        [Range(0f,1f)]
        private float _acceleration = 0.5f;

        [SerializeField]
        private Transform _objectToMove;
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _rotationSpeed;

        private Transform _detectedPlayer;
        private Vector3 _moveDirection;

        // Start is called before the first frame update
        void Start()
        {
            _triggerCollider.radius = _detectionRadius;
            StartMoving();
        }
        private void Update()
        {
            if (_detectedPlayer != null)
            {
                _moveDirection = _detectedPlayer.position - gameObject.transform.position;
            }
            MakeMove();
            MakeRotate();
        }

        public IEnumerator ChooseDirection()
        {
            //    Physics2D.ray
            while (true)
            {
                _moveDirection = Random.insideUnitCircle;
                yield return new WaitForSeconds(_timeBetweenChose);
            }

        }

        private void MakeMove()
        {
            float speed = _detectedPlayer == null ? _speed : _speed + _speed * _acceleration;
           _objectToMove.Translate(_moveDirection.normalized * speed * Time.deltaTime, Space.World);
        }
        private void MakeRotate()
        {
            if (_moveDirection != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, _moveDirection);
                _objectToMove.rotation = Quaternion.RotateTowards(_objectToMove.rotation, rotation, _rotationSpeed * Time.deltaTime);
            }
        }

        private void StartMoving ()
        {
            StartCoroutine(ChooseDirection());
        }
        private void StopMoving()
        {
            StopCoroutine(ChooseDirection());
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {

                _detectedPlayer = collision.gameObject.transform;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("DetectExit");
                _detectedPlayer = null;
                _moveDirection = Random.insideUnitCircle;
            }
        }

    }
}

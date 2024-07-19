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

        private Vector3 _moveDirection = Vector3.zero;

        // Update is called once per frame
        void Update()
        {
            MakeMove();
            MakeRotate();
        }

        private void MakeMove()
        {
            _objectToMove.Translate(_moveDirection * _speed * Time.deltaTime, Space.World);
        }
        private void MakeRotate()
        {
            if (_moveDirection != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, _moveDirection);
                _objectToMove.rotation = Quaternion.RotateTowards(_objectToMove.rotation, rotation, _rotationSpeed * Time.deltaTime);
            }
        }
        public void MoveDirection(Vector2 direction)
        {
            _moveDirection = new Vector3(direction.x, direction.y, 0f);
        }
        public void OnMove(InputValue value)
        {
            //On récupère l'input de cette frame et on le met dans une variable pour l'appliquer
            _moveDirection = value.Get<Vector2>();
        }
    }
}


using UnityEngine;
using UnityEngine.InputSystem;

namespace GDT1.Inputs
{
    [RequireComponent(typeof(PlayerInput))]
    public class Move : MonoBehaviour
    {
        [SerializeField]
        private Transform _objectToMove;
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _rotationSpeed;

        private Vector3 _moveDirection = Vector3.zero;

        // Start is called before the first frame update
        void Start()
        {
          
        }

        // Update is called once per frame
        void Update()
        {
            MakeMove();
            MakeRotate();
        }
        public void OnMove(InputValue value)
        {
          
            //On récupère l'input de cette frame et on le met dans une variable pour l'appliquer
            Vector2 inputValue = value.Get<Vector2>();
            _moveDirection = new Vector3(inputValue.x, inputValue.y, 0f);
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
            //     _objectToMove.rotation = Quaternion.Slerp(_objectToMove.rotation, Quaternion.LookRotation(_moveDirection - _objectToMove.position), 0.15f);

        }
    }
}

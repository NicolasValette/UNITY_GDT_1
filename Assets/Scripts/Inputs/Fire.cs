using UnityEngine;
using UnityEngine.InputSystem;

namespace GDT1.Inputs
{
    public class Fire : MonoBehaviour
    {
        [SerializeField]
        private float _cooldown = 0.1f;
        [SerializeField]
        private GameObject _bulletPrefab;
        [SerializeField]
        private Transform _bulletSpawnPos;
        [SerializeField]
        private float _bulletSpeed = 10f;
        [SerializeField]
        private float _bulletLifeTime = 2f;

        private bool _isFirePressed = false;
        private float _timeSinceLastShoot = 0f;

        void Update()
        {
            if (_isFirePressed)
            {
                _timeSinceLastShoot += Time.deltaTime;
                if (_timeSinceLastShoot > _cooldown)
                {
                    ShootBullet();
                }
            }
        }

        public void OnFire(InputValue value)
        {
            _isFirePressed = value.isPressed;

            // Shoot immediately when the fire button is pressed
            if (_isFirePressed && _timeSinceLastShoot >= _cooldown)
            {
                ShootBullet();
            }
        }

        private void ShootBullet()
        {
            GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPos.position, gameObject.transform.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * _bulletSpeed, ForceMode2D.Impulse);
            Destroy(bullet, _bulletLifeTime);
            _timeSinceLastShoot = 0f;
        }
    }
}

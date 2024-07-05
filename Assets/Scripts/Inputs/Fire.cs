using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GDT1.Inputs
{
    public class Fire : MonoBehaviour
    {
        [SerializeField]
        private float _cooldown;
        [SerializeField]
        private GameObject _bulletPrefab;
        [SerializeField]
        private Transform _bulletSpawnPos;
        [SerializeField]
        private float _bulletSpeed = 10f;
        [SerializeField]
        private float _bulletLifeTime = 2f;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnFire(InputValue value)
        {
            Debug.Log("press : " + value.isPressed);
            
            GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPos.position, gameObject.transform.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * _bulletSpeed, ForceMode2D.Impulse);
            Destroy(bullet, _bulletLifeTime);
        }
    }
}

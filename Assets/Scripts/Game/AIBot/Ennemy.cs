using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDT1
{
    public class Ennemy : MonoBehaviour
    {
        [SerializeField]
        private int _healthPoint = 1;

        private Vector3 _normalSize;
        private Vector3 _smallSize;
        private Vector3 _bigSize;

        void Start()
        {
            // Store the initial scale as the normal size
            _normalSize = transform.localScale;

            // Calculate small and big sizes based on the normal size
            _smallSize = _normalSize - new Vector3(2f, 2f, 0f);
            _bigSize = _normalSize + new Vector3(2f, 2f, 0f);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                TakeDamage();
            }
        }

        private void TakeDamage()
        {
            _healthPoint--;
            if (_healthPoint <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        public void AdjustSize(float zLevel)
        {
            // Adjust size based on the given z-level
            if (zLevel == 0.5f)
            {
                transform.localScale = _smallSize;
            }
            else if (zLevel == 1.0f)
            {
                transform.localScale = _normalSize;
            }
            else if (zLevel == 1.5f)
            {
                transform.localScale = _bigSize;
            }
        }
    }
}

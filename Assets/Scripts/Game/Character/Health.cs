using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDT1
{
    public class Health : MonoBehaviour
    {
        [SerializeField]
        private int _healthPoint = 1;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ennemy"))
            {
                TakeDamage();
            }
        }

        private void TakeDamage()
        {
            _healthPoint--;
            if (_healthPoint <= 0 )
            {
                Die();
            }
        }
        private void Die()
        {
            Destroy(gameObject);
        }
    }
}

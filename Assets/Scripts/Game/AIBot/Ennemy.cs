using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GDT1
{
    public class Ennemy : MonoBehaviour
    {
        [SerializeField]
        private int _healthPoint = 1;
        
        
        // Start is called before the first frame update

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

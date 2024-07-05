using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDT1.Projectile
{
    public class Bullet : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                GetComponent<Renderer>().enabled = false;
            }
        }
    }
}

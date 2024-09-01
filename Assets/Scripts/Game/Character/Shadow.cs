using UnityEngine;

namespace GDT1
{
    public class Shadow : MonoBehaviour
    {
        [SerializeField] private Transform Reference;
        [SerializeField] private float OffsetStrength = .1f;

        private float _zPosition;

        private void Start()
        {
            _zPosition = transform.position.z;
        }

        void Update()
        {
            transform.position = new Vector3(
                Reference.position.x + Reference.position.x * OffsetStrength,
                Reference.position.y + Reference.position.y * OffsetStrength,
                transform.position.z);
        }
    }
}

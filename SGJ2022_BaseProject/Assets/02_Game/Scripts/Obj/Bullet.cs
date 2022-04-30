using UnityEngine;
using System.Collections;

namespace SGJ
{
    public class Bullet : AttackCollisionBase
    {
        [SerializeField]
        private float m_speed = 10f;

        override protected void Update()
        {
            transform.Translate(Vector3.forward * m_speed * Time.deltaTime, Space.Self);
        }
    }
}

using UnityEngine;
using System.Collections;

namespace SGJ
{
    // プレイヤー固有の関数（アイテム影響）などを記述
    public class Player : CharacterBase
    {
        [SerializeField]
        private Animator m_animator = null;

        [SerializeField]
        private GameObject m_attackPrefab = null;

        [SerializeField]
        private GameObject m_bulletPrefab = null;

        [SerializeField]
        private Transform m_attackPos = null;

        [SerializeField]
        private Transform m_muzzle = null;

        [Label("銃"), SerializeField]
        private bool m_isGun = false;

        private void Start()
        {
            NpcManager.Instance.SetPlayer(transform);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (m_isGun)
                {
                    m_animator.SetTrigger("Shot");
                }
                else
                {
                    m_animator.SetTrigger("Attack");
                }
            }
        }

        public void PopAttackCollision()
        {
            var obj = Instantiate(m_attackPrefab, m_attackPos);
        }


        public void Shot()
        {
            var obj = Instantiate(m_bulletPrefab);
            obj.transform.position = m_muzzle.position;
            obj.transform.forward = transform.forward;
        }

    }
}

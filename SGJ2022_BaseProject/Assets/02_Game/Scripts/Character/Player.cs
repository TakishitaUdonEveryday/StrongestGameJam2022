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

        [Label("銃モデル"), SerializeField]
        private GameObject m_gunModel = null;
        [Label("剣モデル"), SerializeField]
        private GameObject m_sordModel = null;

        private void Start()
        {
            NpcManager.Instance.SetPlayer(transform);
            if (m_isGun)
            {
                m_gunModel.SetActive(true);
                m_sordModel.SetActive(false);
            }
            else
            {
                m_gunModel.SetActive(false);
                m_sordModel.SetActive(true);
            }
        }

        private void Update()
        {
            if (!GameManager.Instance.IsPlay || m_isDeath)
                return;
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

        override protected void Death()
        {
            m_isDeath = true;
            GameManager.Instance.GameOver();
            m_animator.SetTrigger("Death");
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().enabled = false;
        }

        override protected void Damage(Vector3 hitPos)
        {
            m_animator.SetTrigger("Damage");
        }

    }
}

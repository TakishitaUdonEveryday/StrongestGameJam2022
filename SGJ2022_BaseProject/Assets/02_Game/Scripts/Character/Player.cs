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

        [Label("武器セット"), SerializeField]
        private GameObject[] m_weapons; 

        private void Start()
        {
            NpcManager.Instance.SetPlayer(transform);
            SetType(m_isGun);
        }

        private void SetType(bool isGun)
        {
            m_isGun = isGun;
            if (m_isGun)
            {
                m_weapons[0].SetActive(true);
                m_weapons[1].SetActive(false);
                m_weapons[2].SetActive(false);
            }
            else
            {
                m_weapons[0].SetActive(false);
                m_weapons[1].SetActive(true);
                m_weapons[2].SetActive(true);
            }
        }

        private void Update()
        {
            if (!GameManager.Instance.IsPlay && m_isDeath)
                return;

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // 剣
                SetType(false);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // 銃
                SetType(true);
            }


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

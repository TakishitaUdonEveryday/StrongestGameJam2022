using UnityEngine;
using System.Collections;
using UnityEngine.AI;

namespace SGJ
{
    public class Enemy : CharacterBase
    {
        protected NpcManager m_npcManager = null;
        protected NavMeshAgent m_navMesh = null;

        [SerializeField]
        protected Animator m_animator = null;

        [SerializeField]
        protected float m_attackLength = 1f;

        [SerializeField]
        protected GameObject m_attackPrefab = null;

        [SerializeField]
        protected Transform m_attackPos = null;

        protected Rigidbody m_rigidbody = null;

        virtual protected void Start()
        {
            m_npcManager = NpcManager.Instance;
            transform.Rotate(Vector3.up * Random.Range(-180, 180));
            m_navMesh = GetComponent<NavMeshAgent>();
            m_rigidbody = GetComponent<Rigidbody>();
            m_navMesh.stoppingDistance = m_attackLength;
        }

        virtual protected void Update()
        {
            if (!GameManager.Instance.IsPlay || m_isDeath)
                return;
            if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
            {
                m_navMesh.isStopped = false;
                // プレイヤーを追いかける
                m_navMesh.SetDestination(m_npcManager.Player.position);

                if (m_npcManager.GetPLDistance(transform) <= m_attackLength)
                {
                    // 攻撃する
                    m_animator.SetTrigger("Attack");
                }
                m_animator.SetFloat("Speed", m_navMesh.velocity.magnitude / m_navMesh.speed);
            }
            else
            {
                m_navMesh.isStopped = true;
            }
        }

        public void PopAttackCollision()
        {
            GameDebug.Log("アタック");
            var obj = Instantiate(m_attackPrefab, m_attackPos);
        }

        override protected void Death()
        {
            m_isDeath = true;
            m_navMesh.isStopped = true;
            m_animator.SetTrigger("Death");
            Destroy(gameObject, 3f);
            GetComponent<Collider>().enabled = false;
        }

        override protected void Damage(Vector3 hitPos)
        {
            m_animator.SetTrigger("Damage");
            var vec = transform.position - hitPos;
            vec.y = 0;
            m_rigidbody.AddForce(vec.normalized * 10f, ForceMode.Force);
        }
    }
}

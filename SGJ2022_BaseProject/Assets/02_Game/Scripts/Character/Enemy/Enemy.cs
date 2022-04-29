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

        virtual protected void Start()
        {
            m_npcManager = NpcManager.Instance;
            m_navMesh = GetComponent<NavMeshAgent>();
            m_navMesh.stoppingDistance = m_attackLength;
        }

        virtual protected void Update()
        {
            if (!GameManager.Instance.IsPlay)
                return;
            // プレイヤーを追いかける
            m_navMesh.SetDestination(m_npcManager.Player.position);

            if (m_npcManager.GetPLDistance(transform) <= m_attackLength)
            {
                // 攻撃する
                m_animator.SetTrigger("Attack");
            }
            m_animator.SetFloat("Speed", m_navMesh.velocity.magnitude / m_navMesh.speed);
        }
    }
}

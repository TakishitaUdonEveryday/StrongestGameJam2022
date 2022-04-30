using UnityEngine;
using System.Collections;
using UnityEngine.AI;

namespace SGJ
{
    public class Npc : CharacterBase
    {
        protected NpcManager m_npcManager = null;
        protected NavMeshAgent m_navMesh = null;

        [SerializeField]
        protected Animator m_animator = null;

        virtual protected void Start()
        {
            m_npcManager = NpcManager.Instance;
            m_navMesh = GetComponent<NavMeshAgent>();

            transform.Rotate(Vector3.up * Random.Range(-180, 180));
            m_rand = Random.Range(8f, 15f);
            m_isMove = Random.Range(0, 2) == 0;
            m_target = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        }

        private float m_rand = 10f;
        private float m_time = 0f;
        private bool m_isMove = false;
        private Vector3 m_target = Vector3.zero;

        virtual protected void Update()
        {
            if (!GameManager.Instance.IsPlay || m_isDeath)
            {
                if (GameManager.Instance.IsGameClear)
                {
                    m_animator.SetBool("isClear", true);
                }
                return;
            }

            if(m_time / m_rand < 1f)
            {
                m_time += Time.deltaTime;
                if (m_isMove)
                {
                    m_navMesh.isStopped = false;
                    m_navMesh.SetDestination(transform.position + m_target);
                }
                else
                {
                    m_navMesh.isStopped = true;
                }
                m_animator.SetFloat("Speed", m_navMesh.velocity.magnitude / m_navMesh.speed);
            }
            else
            {
                m_time = 0;
                m_rand = Random.Range(8f, 15f);
                m_isMove = Random.Range(0, 2) == 0;
                m_target = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            }
            
        }
    }
}

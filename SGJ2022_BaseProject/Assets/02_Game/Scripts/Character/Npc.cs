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

        protected Rigidbody m_rigidbody = null;

        virtual protected void Start()
        {
            m_npcManager = NpcManager.Instance;
            m_navMesh = GetComponent<NavMeshAgent>();
            m_rigidbody = GetComponent<Rigidbody>();

            transform.Rotate(Vector3.up * Random.Range(-180, 180));

            m_randTime = Random.Range(8f, 15f);
            m_target = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            m_isMove = Random.Range(0, 2) == 0;
        }

        private float m_randTime = 10f;
        private float m_time = 0f;

        private Vector3 m_target = Vector3.zero;

        private bool m_isMove = false;

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

            if(m_time / m_randTime < 1f)
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
                m_time = 0f;
                m_randTime = Random.Range(8f, 15f);
                m_target = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
                m_isMove = Random.Range(0, 2) == 0;
            }
        }

    }
}

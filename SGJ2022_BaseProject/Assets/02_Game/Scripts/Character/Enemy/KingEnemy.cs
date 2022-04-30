using UnityEngine;
using System.Collections;
using UnityEngine.AI;

namespace SGJ
{
    public class KingEnemy : Enemy
    {
        [SerializeField]
        private float m_coolTime = 5f;

        [SerializeField]
        private GameObject m_zombiePrefab = null;

        [Label("感染数"), SerializeField]
        private int m_infected = 3;

        [Label("感染範囲"), SerializeField]
        private float m_rad = 5f;

        private float m_time = 0;

        override protected void Start()
        {
            base.Start();
            // 一番近い一般人を感染させる
            Infected();
        }

        override protected void Update()
        {
            if (!GameManager.Instance.IsPlay || m_isDeath)
                return;
            if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
            {
                m_navMesh.isStopped = false;
                if (NpcManager.Instance.GetPLDistance(transform) < m_attackLength)
                {
                    // 攻撃する
                    m_animator.SetTrigger("Attack");
                }
                else
                {
                    // プレイヤーを追いかける
                    m_navMesh.SetDestination(m_npcManager.Player.position);
                }
                m_animator.SetFloat("Speed", m_navMesh.velocity.magnitude / m_navMesh.speed);

                if (m_time / m_coolTime < 1f)
                {
                    m_time += Time.deltaTime;
                }
                else
                {
                    // 一番近い一般人を感染させる
                    Infected();
                    m_time = 0f;
                }
            }
            else
            {
                m_navMesh.isStopped = true;
            }
        }

        /// <summary>
        /// 一番近い一般人を感染させる
        /// </summary>
        private void Infected()
        {
            for (int i = 0; i < m_infected; ++i)
            {
                var npc = m_npcManager.GetNearPeople(transform, m_rad);
                if (npc)
                {
                    m_npcManager.RemovePeople(npc);
                    var zom = Instantiate(m_zombiePrefab);
                    zom.transform.position = npc.transform.position;
                    zom.transform.rotation = npc.transform.rotation;
                    m_npcManager.AddEnemy(zom);
                    Destroy(npc);
                }
            }
        }

        override protected void Death()
        {
            m_isDeath = true;
            m_navMesh.isStopped = true;
            m_animator.SetTrigger("Death");

            Destroy(gameObject, 3f);
            GetComponent<Collider>().enabled = false;

            GameManager.Instance.GameClear();
            m_npcManager.Clear();
        }
    }
}

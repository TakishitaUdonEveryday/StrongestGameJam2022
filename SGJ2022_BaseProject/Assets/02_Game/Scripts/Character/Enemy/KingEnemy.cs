using UnityEngine;
using System.Collections;

namespace SGJ
{
    public class KingEnemy : MonoBehaviour
    {
        private NpcManager m_npcManager = null;

        [SerializeField]
        private float m_coolTime = 5f;

        [SerializeField]
        private GameObject m_zombiePrefab = null;

        [Label("感染数"), SerializeField]
        private int m_infected = 3;

        [Label("感染範囲"), SerializeField]
        private float m_rad = 5f;

        private float m_time = 0;


        private void Start()
        {
            m_npcManager = NpcManager.Instance;
        }

        private void Update()
        {
            if(m_time / m_coolTime < 1f)
            {
                m_time += Time.deltaTime;
            }
            else
            {
                // 一番近い一般人を感染させる
                for(int i = 0; i < m_infected; ++i)
                {
                    var npc = m_npcManager.GetNearPeople(transform, m_rad);
                    if (npc)
                    {
                        m_npcManager.RemovePeople(npc);
                        var zom = Instantiate(m_zombiePrefab);
                        zom.transform.position = npc.transform.position;
                        zom.transform.rotation = npc.transform.rotation;
                        Destroy(npc);
                    }
                }
                m_time = 0f;
            }
        }
    }
}

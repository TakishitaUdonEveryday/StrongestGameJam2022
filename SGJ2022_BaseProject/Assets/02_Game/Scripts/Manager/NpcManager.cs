using UnityEngine;
using System.Collections;

namespace SGJ
{
    public class NpcManager : SingletonMonoBehaviour<NpcManager>
    {
        [SerializeField]
        private GameObject m_peopleObj = null;
        [SerializeField]
        private GameObject m_kingEnemy = null;
        [SerializeField]
        private GameObject m_enemy = null;

        [SerializeField]
        private int m_createCount = 20;

        [SerializeField]
        private float m_rad = 20f;

        private void Start()
        {
            Create();
        }

        private void Create()
        {
            int count = 0;
            while(count < m_createCount)
            {
                var obj = Instantiate(m_peopleObj, transform);
                var pos = new Vector3(Random.Range(-m_rad, m_rad), obj.transform.position.y, Random.Range(-m_rad, m_rad));
                obj.transform.position = pos;
                ++count;
            }

            // キングをつくる
            var king = Instantiate(m_kingEnemy, transform);
            var kpos = new Vector3(Random.Range(-m_rad / 2, m_rad / 2), king.transform.position.y, Random.Range(-m_rad / 2, m_rad / 2));
            king.transform.position = kpos;
        }
    }
}

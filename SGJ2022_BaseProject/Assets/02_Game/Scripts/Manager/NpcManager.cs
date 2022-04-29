using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

        private List<GameObject> m_peoples = new List<GameObject>();

        private Transform m_player = null;

        public Transform Player { get => m_player; }

        private void Start()
        {
            Create();
        }

        public void SetPlayer(Transform pl)
        {
            m_player = pl;
        }

        private void Create()
        {
            int count = 0;
            while(count < m_createCount)
            {
                var obj = Instantiate(m_peopleObj, transform);
                var pos = new Vector3(Random.Range(-m_rad, m_rad), obj.transform.position.y, Random.Range(-m_rad, m_rad));
                obj.transform.position = pos;
                m_peoples.Add(obj);
                ++count;
            }

            // キングをつくる
            var king = Instantiate(m_kingEnemy, transform);
            var kpos = new Vector3(Random.Range(-m_rad / 2, m_rad / 2), king.transform.position.y, Random.Range(-m_rad / 2, m_rad / 2));
            king.transform.position = kpos;
        }

        /// <summary>
        /// 指定距離内で一番近い
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public GameObject GetNearPeople(Transform trans, float rad)
        {
            var min = 999f;
            GameObject ans = null;
            foreach (var p in m_peoples)
            {
                var dis = Mathf.Abs(Vector3.Distance(trans.position, p.transform.position));
                if (dis < min && dis < rad)
                {
                    ans = p;
                    min = dis;
                }
            }

            return ans;
        }

        public void RemovePeople(GameObject obj)
        {
            m_peoples.Remove(obj);
        }

        public float GetPLDistance(Transform trans)
        {
            return Mathf.Abs(Vector3.Distance(trans.position, m_player.transform.position));
        }
    }
}

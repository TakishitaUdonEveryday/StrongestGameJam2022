using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGJ
{
    public class HeartUI : SingletonMonoBehaviour<HeartUI>
    {
        [SerializeField]
        private GameObject m_prefab;

        private List<GameObject> m_list = new List<GameObject>();

        public void SetUI(int count)
        {
            for(int i = transform.childCount; i < count; ++i)
            {
                m_list.Add(Instantiate(m_prefab, transform));
            }
            while (m_list.Count > count)
            {
                Destroy(m_list[m_list.Count - 1]);
                m_list.RemoveAt(m_list.Count - 1);
            }
        }
    }
}
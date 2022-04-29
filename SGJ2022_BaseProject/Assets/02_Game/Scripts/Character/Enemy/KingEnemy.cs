using UnityEngine;
using System.Collections;

namespace SGJ
{
    public class KingEnemy : MonoBehaviour
    {
        private NpcManager m_npcManager = null;

        private void Start()
        {
            m_npcManager = NpcManager.Instance;
        }

        private void Update()
        {
            
        }
    }
}

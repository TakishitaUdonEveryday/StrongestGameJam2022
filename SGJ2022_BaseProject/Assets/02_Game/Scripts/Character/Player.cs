using UnityEngine;
using System.Collections;

namespace SGJ
{
    // プレイヤー固有の関数（アイテム影響）などを記述
    public class Player : CharacterBase
    {
        [SerializeField]
        private Animator m_animator = null;

        [SerializeField]
        private GameObject m_attackPrefab = null;

        [SerializeField]
        private Transform m_attackPos = null;

        private void Start()
        {
            NpcManager.Instance.SetPlayer(transform);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_animator.SetTrigger("Attack");
            }
        }

        public void PopAttackCollision()
        {
            var obj = Instantiate(m_attackPrefab, m_attackPos);
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGJ
{
    public class GirlController : MonoBehaviour
    {
        [SerializeField]
        private Animator m_animator = null;
        [SerializeField]
        private Transform m_playerTransform = null;
        [SerializeField]
        private GameObject m_pickupHeart = null;
        [SerializeField]
        private GameObject m_brokenHeart = null;

        private Rigidbody m_rigidbody = null;

        private bool m_isEnd = false;

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            // プレイ終了
            if (!GameManager.Instance.IsPlay)
            {
                // プレイヤーの方を向く
                if (GameManager.Instance.IsGameClear || GameManager.Instance.IsGameOver)
                {
                    LookToPlayer();
                }

                if (m_isEnd == false)
                {
                    // クリアした？
                    if (GameManager.Instance.IsGameClear)
                    {
                        // パーティクルセット
                        GameObject pickup = Instantiate(m_pickupHeart) as GameObject;
                        pickup.transform.position += transform.position;
                        pickup.transform.parent = transform;

                        m_isEnd = true;
                    }
                    // ゲームオーバー？
                    if (GameManager.Instance.IsGameOver)
                    {
                        // パーティクルセット
                        Vector3 pos = new Vector3(0, 1.3f, 0);
                        GameObject broken = Instantiate(m_brokenHeart) as GameObject;
                        broken.transform.position += transform.position;
                        broken.transform.parent = transform;

                        m_isEnd = true;
                    }
                }
            }
        }

        private void LookToPlayer()
        {
            const int ROT_SPEED = 240; // 1秒間の回転量
            var vec = m_playerTransform.position - transform.position;
            var rot = Quaternion.LookRotation(vec, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, ROT_SPEED * Time.deltaTime);
        }
    }
}
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

        private Rigidbody m_rigidbody = null;

        private bool m_isEnd = false;

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            // �v���C�I��
            if (!GameManager.Instance.IsPlay)
            {
                // �v���C���[�̕�������
                if (GameManager.Instance.IsGameClear || GameManager.Instance.IsGameOver)
                {
                    LookToPlayer();
                }

                if (m_isEnd == false)
                {
                    // �N���A�����H
                    if (GameManager.Instance.IsGameClear)
                    {
                        // �A�j���[�V�����Z�b�g

                        m_isEnd = true;
                    }
                    // �Q�[���I�[�o�[�H
                    if (GameManager.Instance.IsGameOver)
                    {
                        // �A�j���[�V�����Z�b�g

                        m_isEnd = true;
                    }
                }
            }
        }

        private void LookToPlayer()
        {
            const int ROT_SPEED = 240; // 1�b�Ԃ̉�]��
            var vec = m_playerTransform.position - transform.position;
            var rot = Quaternion.LookRotation(vec, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, ROT_SPEED * Time.deltaTime);
        }
    }
}
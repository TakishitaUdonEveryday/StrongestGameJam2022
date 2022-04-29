using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SGJ
{

    public class EnemyController : MonoBehaviour
    {
        [SerializeField, Label("����p�x")] private float m_viewAngleRange = 45.0f;
        [SerializeField, Label("���싗��")] private float m_viewLength = 5.0f;
        [SerializeField, Label("�ړ����x")] private float m_moveSpeed = 1.2f;

		[SerializeField] private List<Transform> m_targetsList = new List<Transform>();

        private PlayerController m_foundPlayer = null;

        private Rigidbody m_rigidbody = null;


        // Start is called before the first frame update
        void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();

            StartCoroutine(CoObservePlayer());

			// �o�^ 
			GameManager.Instance.RegistZombie(this);
        }

        // Update is called once per frame
        void Update()
        {
        }


		private void FixedUpdate()
		{
			// �v���C���[�����F���Ă�����v���C���[�����Ɉړ����� 
            if (m_foundPlayer!=null)
			{
				if (GameManager.Instance.IsPlay)
				{
					// �̂̌������v���C���[�����ɂ���
					Vector3 dir = m_foundPlayer.transform.position - transform.position;
					dir.y = 0.0f;
					Quaternion lookRot = Quaternion.LookRotation(dir);
					transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 0.25f);

					// ���ʂɈړ����� 
					Vector3 move = transform.forward * (m_moveSpeed * Time.fixedDeltaTime);
					m_rigidbody.MovePosition(transform.position + move);
				}
			}
		}





		/// <summary>
		/// ��莞�ԊԊu���ƂɃv���C���[���Ď�����
		/// </summary>
		/// <returns></returns>
		private IEnumerator CoObservePlayer()
		{
            while (true)
			{
                // �Ď�����
                ObservePlayer();

                // �����_�����ԊԊu�Ń`�F�b�N
                yield return new WaitForSeconds(Random.Range(0.0f, 0.5f));
			}
		}


        /// <summary>
        /// �v���C���[���Ď����� 
        /// </summary>
        private void ObservePlayer()
		{
            const int VIEW_DIV_NUM = 20;
            const float EYE_HEIGHT = 1.0f;

			Vector3 eyeLocalPos = Vector3.up * EYE_HEIGHT;
		//	Debug.Log("pos = " + transform.position);
			Vector3 eyeWorldPos = transform.TransformPoint(eyeLocalPos);
			GameObject foundObj = null;
			int layerMask = ~(1 << CommonDefines.LAYER_ZOMBIE);

			for (int i=0; i < VIEW_DIV_NUM; ++i)
			{
                RaycastHit hit = new RaycastHit();
                float angle = Mathf.Lerp(m_viewAngleRange * (-0.5f),
                        m_viewAngleRange * (0.5f), (float)i / (float)(VIEW_DIV_NUM - 1)) * Mathf.PI/180.0f;
                Vector3 viewLocalDir = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
                if (Physics.Raycast(new Ray
                    {
                        origin = eyeWorldPos,
                        direction = transform.TransformDirection(viewLocalDir)
                    }, out hit, m_viewLength, layerMask))
				{
                    if (hit.collider.gameObject.layer == CommonDefines.LAYER_PLAYER)
					{
                        foundObj = hit.collider.gameObject;
                        break;
                    }
				}
			}

            // ������ꂽ�H
            if (foundObj!=null)
			{
                m_foundPlayer = foundObj.GetComponentInParent<PlayerController>();
            //    Debug.Log("FOUND");
            } else
			{
                m_foundPlayer = null;
            //    Debug.Log("NONE");
			}

        }


		/// <summary>
		/// �^�[�Q�b�g���J�������猩���Ă��邩���肷�� 
		/// </summary>
		/// <param name="camera"></param>
		/// <returns></returns>
		public float CalcViewRate(Camera camera)
		{
			int inViewCount = 0;
			Vector3 cameraPos = camera.transform.position;
			foreach (var target in m_targetsList)
			{
				Vector3		viewp = camera.WorldToViewportPoint(target.position);
				if ( 0.0f < viewp.z && 0.0f < viewp.x && viewp.x < 1.0f && 0.0f < viewp.y && viewp.y < 1.0f )
				{
					// ��Q������ 
					RaycastHit hitResult = new RaycastHit();
					if (Physics.Raycast(new Ray(cameraPos, target.position - cameraPos),
						out hitResult, 100.0f, ~(1 << CommonDefines.LAYER_PLAYER)))
					{
						var hitEnemy = hitResult.transform.GetComponentInParent<EnemyController>();
						if ( hitEnemy == this )
						{
							inViewCount++;
						}
					}
				}
			}
			if ( inViewCount <= 0 )
			{
				return 0.0f;
			}
			else
			{
				// �J�����Ɛ��΂��Ă���قǍ����_ 
				float dot = Vector3.Dot(camera.transform.forward, transform.forward);

				return (float)inViewCount / (float)m_targetsList.Count * Mathf.Lerp(0.5f, 1.0f, dot * (-0.5f) + 0.5f);
			}
		}


	}

}




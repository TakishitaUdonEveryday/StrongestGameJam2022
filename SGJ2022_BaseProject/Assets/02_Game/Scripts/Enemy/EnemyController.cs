using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SGJ
{

    public class EnemyController : MonoBehaviour
    {
        [SerializeField, Label("視野角度")] private float m_viewAngleRange = 45.0f;
        [SerializeField, Label("視野距離")] private float m_viewLength = 5.0f;
        [SerializeField, Label("移動速度")] private float m_moveSpeed = 1.2f;

		[SerializeField] private List<Transform> m_targetsList = new List<Transform>();

        private PlayerController m_foundPlayer = null;
		private Vector3 m_targetPosition = Vector3.zero;

        private Rigidbody m_rigidbody = null;

		private Transform m_bodyTr = null;
		private Quaternion m_startRot = Quaternion.identity;

		private float m_swingAngle = 0;


        // Start is called before the first frame update
        void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
			m_bodyTr = transform.GetChild(0);
			m_startRot = m_bodyTr.localRotation;

			StartCoroutine(CoObservePlayer());

			// 登録 
			GameManager.Instance.RegistZombie(this);
        }

		const float FOUND_SWING_ANGLE = (Mathf.PI * 2.0f * 2.5f);
		const float NOT_FOUND_SWING_ANGLE = (Mathf.PI * 2.0f);

        // Update is called once per frame
        void Update()
        {
			if (m_foundPlayer !=null)
			{
				m_swingAngle += FOUND_SWING_ANGLE * Time.deltaTime;
			} else
			{
				m_swingAngle += NOT_FOUND_SWING_ANGLE * Time.deltaTime;
			}

			m_bodyTr.localRotation = Quaternion.AngleAxis(2.0f * Mathf.Sin(m_swingAngle), Vector3.forward) * m_startRot;
		}


		private void FixedUpdate()
		{
			// プレイヤーを視認していたらプレイヤー方向に移動する 
            if (m_foundPlayer!=null)
			{
				if (GameManager.Instance.IsPlay)
				{
					// 体の向きをプレイヤー方向にする
				//	Vector3 dir = m_foundPlayer.transform.position - transform.position;
					Vector3 dir = m_targetPosition - transform.position;
					dir.y = 0.0f;
					Quaternion lookRot = Quaternion.LookRotation(dir);
					transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 0.25f);

					// 正面に移動する 
					Vector3 move = transform.forward * (m_moveSpeed * Time.fixedDeltaTime);
					m_rigidbody.MovePosition(transform.position + move);
				}
			}
		}





		/// <summary>
		/// 一定時間間隔ごとにプレイヤーを監視する
		/// </summary>
		/// <returns></returns>
		private IEnumerator CoObservePlayer()
		{
            while (true)
			{
                // 監視処理
                ObservePlayer();

                // ランダム時間間隔でチェック
                yield return new WaitForSeconds(Random.Range(0.0f, 0.5f));
			}
		}


        /// <summary>
        /// プレイヤーを監視する 
        /// </summary>
        private void ObservePlayer()
		{
        //    const int VIEW_DIV_NUM = 20;
            const float EYE_HEIGHT = 1.0f;

			Vector3 eyeLocalPos = Vector3.up * EYE_HEIGHT;
		//	Debug.Log("pos = " + transform.position);
			Vector3 eyeWorldPos = transform.TransformPoint(eyeLocalPos);
			eyeWorldPos.y = EYE_HEIGHT;		// 高身長でもプレイヤーを見つけるパッチ
			GameObject foundObj = null;
			int layerMask = ~(1 << CommonDefines.LAYER_ZOMBIE);

			// プレイヤー中央 
			var playerCont = GameManager.Instance.GetPlayer();
			Vector3 playerCenter = playerCont.transform.position + Vector3.up;

			// プレイヤー方向 
			Vector3 toPlayer = playerCenter - eyeWorldPos;
			Vector3 toPlayerXZ = new Vector3(toPlayer.x, toPlayer.y*0.1f, toPlayer.z);

			// 視野内か？ 
			float dot = Vector3.Dot(transform.forward, toPlayerXZ.normalized);
			if (Mathf.Cos(m_viewAngleRange*Mathf.PI/180.0f) < dot)
			{
				// 視線は通るか？ 
				RaycastHit hit = new RaycastHit();
				if (Physics.Raycast(new Ray
				{
					origin = eyeWorldPos,
					direction = toPlayerXZ
				}, out hit, m_viewLength, layerMask))
				{
					if (hit.collider.gameObject.layer == CommonDefines.LAYER_PLAYER)
					{
						foundObj = hit.collider.gameObject;
					}
				}
			}


			/*	@memo 自分の正面に放射状に視線を飛ばすと、距離が離れると隙間が大きくなってかいくぐれてしまう 
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
			*/

            // 見つけられた？
            if (foundObj!=null)
			{
                m_foundPlayer = foundObj.GetComponentInParent<PlayerController>();
				m_targetPosition = m_foundPlayer.transform.position;
				//    Debug.Log("FOUND");
			} else
			{
                m_foundPlayer = null;
            //    Debug.Log("NONE");
			}

        }


		/// <summary>
		/// ターゲットがカメラから見えているか判定する 
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
					// 障害物判定 
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
				// カメラと正対しているほど高得点 
				float dot = Vector3.Dot(camera.transform.forward, transform.forward);

				return (float)inViewCount / (float)m_targetsList.Count * Mathf.Lerp(0.5f, 1.0f, dot * (-0.5f) + 0.5f);
			}
		}


	}

}




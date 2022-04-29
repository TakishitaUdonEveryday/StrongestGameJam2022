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


        private PlayerController m_foundPlayer = null;

        private Rigidbody m_rigidbody = null;


        // Start is called before the first frame update
        void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();

            StartCoroutine(CoObservePlayer());
        }

        // Update is called once per frame
        void Update()
        {
        }


		private void FixedUpdate()
		{
			// プレイヤーを視認していたらプレイヤー方向に移動する 
            if (m_foundPlayer!=null)
			{
				if (GameManager.Instance.IsPlay)
				{
					// 体の向きをプレイヤー方向にする
					Vector3 dir = m_foundPlayer.transform.position - transform.position;
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

            // 見つけられた？
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

    }

}




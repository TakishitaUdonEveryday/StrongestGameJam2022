using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SGJ
{

    public class EnemyController : MonoBehaviour
    {
        [SerializeField, Label("視野角度")] private float m_viewAngleRange = 45.0f;
        [SerializeField, Label("視野距離")] private float m_viewLength = 5.0f;


        private PlayerController m_foundPlayer = null;


        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(CoObservePlayer());
        }

        // Update is called once per frame
        void Update()
        {
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
            const int VIEW_DIV_NUM = 8;
            const float EYE_HEIGHT = 1.0f;

            Vector3 eyeLocalPos = Vector3.up * EYE_HEIGHT;
            Debug.Log("pos = " + transform.position);
            Vector3 eyeWorldPos = transform.TransformPoint(eyeLocalPos);
            GameObject foundObj = null;
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
                    }, out hit, m_viewLength))
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
                Debug.Log("FOUND");
            } else
			{
                m_foundPlayer = null;
                Debug.Log("NONE");
			}

        }

    }

}




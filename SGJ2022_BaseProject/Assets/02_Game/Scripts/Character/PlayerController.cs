using UnityEngine;
using System.Collections;

namespace SGJ
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Label("コントローラー"), SerializeField]
        private FloatingJoystick m_joystick = null;

        [Label("移動スピード"), SerializeField]
        private float m_speed = 1f;

        [Label("自動で走る"), SerializeField]
        private bool m_isAutoRun = false;

        [SerializeField]
        private Animator m_animator = null;

        private Rigidbody m_rigidbody = null;
        private Camera m_camera = null;

        private bool m_isEnd = false;

		private bool m_isTakingPictures = false;


        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            m_camera = Camera.main;
        }


		public void Update()
		{
			if (GameManager.Instance.IsPlay)
			{
				if (CameraTexture.Instance != null)
				{
					switch (CameraTexture.Instance.GetStatus())
					{
						case CameraTexture.Status.Outside:
							if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
							{
								// 写真撮影開始 
								CameraTexture.Instance.ActionEnter();
								m_isTakingPictures = true;
							}
							break;
						case CameraTexture.Status.Hold:
							if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
							{
								// シャッター 
								CameraTexture.Instance.ActionShutter();
							}
							else if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
							{
								// 撮影辞める 
								CameraTexture.Instance.ActionLeave();
							}
							break;
						case CameraTexture.Status.Leave:
							m_isTakingPictures = false;
							break;
					}
				}
			}
			else
			{
				if (CameraTexture.Instance.GetStatus() == CameraTexture.Status.Hold)
				{
					// 撮影辞める 
					CameraTexture.Instance.ActionLeave();
				}
			}
		}


		private void FixedUpdate()
        {
            // プレイ中
            if (GameManager.Instance.IsPlay)
            {
                // カメラの方向から、X-Z平面の単位ベクトルを取得
                Vector3 cameraForward = Vector3.Scale(m_camera.transform.forward, new Vector3(1, 0, 1)).normalized;

				// 方向キーの入力値とカメラの向きから、移動方向を決定
				Vector3 moveForward = Vector3.zero;
				if (!m_isTakingPictures)
				{
					moveForward = cameraForward * m_joystick.Vertical + m_camera.transform.right * m_joystick.Horizontal;
				}
	

                if (m_isAutoRun)
                {
                    moveForward += Vector3.forward;
                }

                // キャラクターの向きを進行方向に
                if (moveForward != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(moveForward);
                    m_rigidbody.MovePosition(m_rigidbody.position + moveForward * m_speed * Time.deltaTime);
                }
                else
                {
					// 物理挙動Off
					Vector3 velo = m_rigidbody.velocity;
					velo.x = velo.z = 0.0f;
                    m_rigidbody.velocity = velo;
                }
                m_animator.SetFloat("Speed", moveForward.magnitude);
            }
            else
            {
                if (m_isEnd == false)
                {
                    // クリアした？
                    if (GameManager.Instance.IsGameClear)
                    {
                        // カメラの方を向く
                        var vec = m_camera.transform.position - transform.position;
                        vec.y = 0;
                        transform.forward = vec.normalized;

                        // アニメーションセット
                        m_animator.SetBool("isClear", true);
                        m_isEnd = true;
                    }
					// ゲームオーバー 
					else if (GameManager.Instance.IsGameOver)
					{
						// アニメーションセット
						m_animator.SetBool("isGameOver", true);
						m_isEnd = true;
					}
				}
            }
        }




		///////////////////////////////////////////////////////////////////////////////////////////



		public void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.layer == CommonDefines.LAYER_ZOMBIE)
			{
				GameManager.Instance.GameOver();
			}
		}

	}

}

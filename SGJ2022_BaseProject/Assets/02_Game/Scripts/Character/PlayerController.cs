using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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


		[SerializeField] private Transform m_cameraMachineTr = null;	// カメラ機のTransform 

        private Rigidbody m_rigidbody = null;
        private Camera m_camera = null;

        private bool m_isEnd = false;

		private bool m_isTakingPictures = false;

		[SerializeField] private float m_cameraMoveAngleSpd = 90.0f;
		[SerializeField] private RenderTexture m_photoRenderTexture;

		private List<Texture> m_photoTextures = new List<Texture>();

		private int m_countOfFilms = GameManager.DEFAULT_FILMS_NUM;     // フィルムの数 


        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            m_camera = Camera.main;

			// カメラの動作を無効化 
			m_cameraMachineTr.gameObject.SetActive(false);
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
								// カメラ機の操作開始 
								StartCoroutine(CoControlCameraMachine());
							}
							break;
						case CameraTexture.Status.Hold:
							if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
							{
								if (0 < m_countOfFilms)
								{
									// シャッター 
									CameraTexture.Instance.ActionShutter();

									// フィルムの枚数を減らす 
									m_countOfFilms--;
									GameManager.Instance.SetFilmsCount(m_countOfFilms);

									// 写真をコピー 
									var newTexture = new Texture2D(m_photoRenderTexture.width, m_photoRenderTexture.height, TextureFormat.RGBA32, false);
									Graphics.CopyTexture(m_photoRenderTexture, newTexture);
									m_photoTextures.Add(newTexture);

									// プレイ中アルバムに新しい写真を設定 
									GameManager.Instance.SetNewTextureToPlayingAlbum(newTexture);
								}
							}
							else if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
							{
								// 撮影辞める 
								CameraTexture.Instance.ActionLeave();
							}
							break;
						case CameraTexture.Status.Leave:
							m_isTakingPictures = false;
							StopAllCoroutines();
							// カメラの動作を無効化 
							m_cameraMachineTr.gameObject.SetActive(false);
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


		/// <summary>
		/// カメラ機の操作 
		/// </summary>
		/// <returns></returns>
		private IEnumerator CoControlCameraMachine()
		{
			m_cameraMachineTr.localRotation = Quaternion.identity;
			Vector3 rotAngle = Vector3.zero;

			Vector3 mousePos = Vector3.zero;
			bool isDown = false;
			if (Input.GetMouseButton(0))
			{
				isDown = true;
				mousePos = Input.mousePosition;
			}

			// カメラの動作を有効化 
			m_cameraMachineTr.gameObject.SetActive(true);

			while (CameraTexture.Instance.GetStatus() != CameraTexture.Status.Outside)
			{
				if (Input.GetMouseButtonDown(0))
				{
					isDown = true;
					mousePos = Input.mousePosition;
				} else if (!Input.GetMouseButton(0))
				{
					isDown = false;
				}

				if (isDown)
				{
					//rotAngle.y = Mathf.Clamp(rotAngle.y - m_joystick.Horizontal * (m_cameraMoveAngleSpd * Time.deltaTime), -25.0f, 25.0f);
					//rotAngle.x = Mathf.Clamp(rotAngle.x + m_joystick.Vertical * (m_cameraMoveAngleSpd * Time.deltaTime), -120.0f, 120.0f);

					rotAngle.y = Mathf.Clamp(rotAngle.y - (Input.mousePosition.x - mousePos.x) / (float)Screen.width * m_cameraMoveAngleSpd, -120.0f, 120.0f);
					rotAngle.x = Mathf.Clamp(rotAngle.x + (Input.mousePosition.y - mousePos.y) / (float)Screen.height * m_cameraMoveAngleSpd, -25.0f, 25.0f);
					mousePos = Input.mousePosition;

					m_cameraMachineTr.localRotation = Quaternion.Euler(rotAngle);
				}

				yield return null;
			}

			// カメラの動作を無効化 
			m_cameraMachineTr.gameObject.SetActive(false);

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

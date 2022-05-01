using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SGJ
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        private bool m_isStart = false;
        private bool m_isGameClear = false;
        private bool m_isGameOver = false;

		public const int DEFAULT_FILMS_NUM = 10;

		[SerializeField] private ClearStaging m_textClear;
		[SerializeField] private ClearStaging m_textOver;
		[SerializeField] private TextMeshProUGUI m_filmsCountText = null;
		[SerializeField] private PlayingAlbumUI m_playingAlbum = null;
		[SerializeField] private ResultPhotoAlbumUI m_resultPhotoAlbum = null;
		[SerializeField] private PlayerController m_playerCont = null;

		public const int DEFAULT_PHOTO_SCORE = 100;

		public class EnemyInfo
		{
			public EnemyController m_enemy = null;
			public int m_point = DEFAULT_PHOTO_SCORE;
		};
		public List<EnemyInfo> m_enemyInfoList = new List<EnemyInfo>();


		/// <summary>
		/// �v���C���[���擾 
		/// </summary>
		/// <returns></returns>
		public PlayerController	GetPlayer()
		{
			return m_playerCont;
		}



		/// <summary>
		/// �Q�[���v���C���t���O
		/// </summary>
		public bool IsPlay
        {
            get
            {
                // �X�^�[�g���Ă��āA�N���A��Q�[���I�[�o�[����Ȃ��Ȃ�true
                if (m_isStart && m_isGameClear == false && m_isGameOver == false)
                {
                    return true;
                }
                return false;
            }
        }


		public void Start()
		{
			m_resultPhotoAlbum.gameObject.SetActive(false);
		}


		public bool IsGameClear { get => m_isGameClear; }
        public bool IsGameOver { get => m_isGameOver; }

        public void GameStart()
        {
            if (m_isStart)
                return;
            GameDebug.Log("�Q�[���X�^�[�g");
            if(GoalObj.Instance == null)
            {
                GameDebug.LogError("�S�[��������܂���");
            }
            m_isStart = true;
        }

        public void GameClear()
        {
            if (m_isGameClear || m_isGameOver)
                return;
            GameDebug.Log("�Q�[���N���A");
            m_isGameClear = true;

			// �v���C���A���o�����\�� 
			m_playingAlbum.gameObject.SetActive(false);

			// ���U���g�\�� 
			var photoList = m_playerCont.GetPhotoDataList();
			m_resultPhotoAlbum.StartDisplay(photoList);

			// ���v�_ 
			int totalScore = 0;
			foreach (var photo in photoList)
			{
				totalScore += photo.m_score;
			}
			m_textClear.SetTotalScore(totalScore);

			// �L�^�X�V 
			var data = GameDataManager.Instance.GameData;
			int lastScore = SaveData.GetRecord(data.StageNum);
			if (lastScore < totalScore)
			{
				SaveData.SetRecord(data.StageNum, totalScore);
				m_textClear.SetNew(true);
			} else
			{
				m_textClear.SetNew(false);
			}

			// ���U���g�\�� 
			StartCoroutine(CoDelayResult((float)photoList.Count*0.2f + 0.25f));
		}

		private IEnumerator CoDelayResult(float delayTime)
		{
			yield return new WaitForSeconds(delayTime);

			// �Q�[���N���A�\�� 
			m_textClear.SetActive(true, 2.0f);
		}



		public void GameOver()
        {
            if (m_isGameClear || m_isGameOver)
                return;
            GameDebug.Log("�Q�[���I�[�o�[");
            m_isGameOver = true;

			// �Q�[���I�[�o�[�\�� 
			m_textOver.SetActive(true, 2.0f);

		}



		/// <summary>
		/// �t�B�����̐���UI�e�L�X�g���X�V 
		/// </summary>
		/// <param name="count"></param>
		public void SetFilmsCount(int count)
		{
			m_filmsCountText.text = count.ToString();
		}


		/// <summary>
		/// �v���C���A���o���ɐV�����ʐ^��o�^ 
		/// </summary>
		/// <param name="newTexture"></param>
		public void SetNewTextureToPlayingAlbum(Texture newTexture)
		{
			m_playingAlbum.SetNewPhotoTexture(newTexture);
		}



		/// <summary>
		/// �G��o�^ 
		/// </summary>
		/// <param name="enemy"></param>
		public void RegistZombie(EnemyController enemy)
		{
			m_enemyInfoList.Add(new EnemyInfo
			{
				m_enemy = enemy
			});
		}



		/// <summary>
		/// �B�e�ɂ��X�R�A�v�Z 
		/// </summary>
		/// <param name="photoCamera"></param>
		/// <returns></returns>
		public int	CalcurateScoreByShooting(Camera photoCamera, out int newCount)
		{
			const float MAX_PHOTO_DISTANCE = 100.0f;
			const float MAX_PHOTO_DISTANCE_SQ = MAX_PHOTO_DISTANCE * MAX_PHOTO_DISTANCE;

			int score = 0;
			newCount = 0;

			Transform cameraTr = photoCamera.transform;

			foreach (var enemy in m_enemyInfoList)
			{
				// �������� 
				Vector3 dir = enemy.m_enemy.transform.position - cameraTr.position;
				if (MAX_PHOTO_DISTANCE_SQ < dir.sqrMagnitude) continue;

				// ��������O������ 
				if (Vector3.Dot(dir, cameraTr.forward) < 0.0f)
				{
					continue;
				}

				// �ڍה��� 
				float viewRate = enemy.m_enemy.CalcViewRate(photoCamera);
				if ( 0.0f < viewRate)
				{
					if (DEFAULT_PHOTO_SCORE <= enemy.m_point)
					{
						newCount++;
					}
					int thisScore = (int)Mathf.Min((float)(DEFAULT_PHOTO_SCORE + 2) * viewRate, enemy.m_point);
					enemy.m_point -= thisScore;
					score += thisScore;
				}
			}

			return score;
		}


	}
}


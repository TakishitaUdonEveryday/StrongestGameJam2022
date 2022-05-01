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
		/// プレイヤーを取得 
		/// </summary>
		/// <returns></returns>
		public PlayerController	GetPlayer()
		{
			return m_playerCont;
		}



		/// <summary>
		/// ゲームプレイ中フラグ
		/// </summary>
		public bool IsPlay
        {
            get
            {
                // スタートしていて、クリアやゲームオーバーじゃないならtrue
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
            GameDebug.Log("ゲームスタート");
            if(GoalObj.Instance == null)
            {
                GameDebug.LogError("ゴールがありません");
            }
            m_isStart = true;
        }

        public void GameClear()
        {
            if (m_isGameClear || m_isGameOver)
                return;
            GameDebug.Log("ゲームクリア");
            m_isGameClear = true;

			// プレイ中アルバムを非表示 
			m_playingAlbum.gameObject.SetActive(false);

			// リザルト表示 
			var photoList = m_playerCont.GetPhotoDataList();
			m_resultPhotoAlbum.StartDisplay(photoList);

			// 合計点 
			int totalScore = 0;
			foreach (var photo in photoList)
			{
				totalScore += photo.m_score;
			}
			m_textClear.SetTotalScore(totalScore);

			// 記録更新 
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

			// リザルト表示 
			StartCoroutine(CoDelayResult((float)photoList.Count*0.2f + 0.25f));
		}

		private IEnumerator CoDelayResult(float delayTime)
		{
			yield return new WaitForSeconds(delayTime);

			// ゲームクリア表示 
			m_textClear.SetActive(true, 2.0f);
		}



		public void GameOver()
        {
            if (m_isGameClear || m_isGameOver)
                return;
            GameDebug.Log("ゲームオーバー");
            m_isGameOver = true;

			// ゲームオーバー表示 
			m_textOver.SetActive(true, 2.0f);

		}



		/// <summary>
		/// フィルムの数のUIテキストを更新 
		/// </summary>
		/// <param name="count"></param>
		public void SetFilmsCount(int count)
		{
			m_filmsCountText.text = count.ToString();
		}


		/// <summary>
		/// プレイ中アルバムに新しい写真を登録 
		/// </summary>
		/// <param name="newTexture"></param>
		public void SetNewTextureToPlayingAlbum(Texture newTexture)
		{
			m_playingAlbum.SetNewPhotoTexture(newTexture);
		}



		/// <summary>
		/// 敵を登録 
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
		/// 撮影によるスコア計算 
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
				// 距離判定 
				Vector3 dir = enemy.m_enemy.transform.position - cameraTr.position;
				if (MAX_PHOTO_DISTANCE_SQ < dir.sqrMagnitude) continue;

				// ざっくり前方判定 
				if (Vector3.Dot(dir, cameraTr.forward) < 0.0f)
				{
					continue;
				}

				// 詳細判定 
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


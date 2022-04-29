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

			// ゲームクリア表示 
			m_textClear.SetActive(true);
        }

        public void GameOver()
        {
            if (m_isGameClear || m_isGameOver)
                return;
            GameDebug.Log("ゲームオーバー");
            m_isGameOver = true;

			// ゲームオーバー表示 
			m_textOver.SetActive(true);

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


	}
}


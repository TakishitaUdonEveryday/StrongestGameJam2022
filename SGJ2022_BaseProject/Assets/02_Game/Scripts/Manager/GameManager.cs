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

			// �Q�[���N���A�\�� 
			m_textClear.SetActive(true);
        }

        public void GameOver()
        {
            if (m_isGameClear || m_isGameOver)
                return;
            GameDebug.Log("�Q�[���I�[�o�[");
            m_isGameOver = true;

			// �Q�[���I�[�o�[�\�� 
			m_textOver.SetActive(true);

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


	}
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SGJ
{
    public class ClearStaging : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup m_canvasGroup = null;

		[SerializeField] private TextMeshProUGUI m_totalScoreText = null;

		[SerializeField] private TextMeshProUGUI m_textNew = null;


        private bool m_isTap = false;

		private float m_delayTime = 0.0f;
		private float m_startTime = 0.0f;

        public void SetActive(bool active, float delayTime)
        {
            if (active)
            {
                m_canvasGroup.alpha = 1f;
                m_canvasGroup.blocksRaycasts = true;

				m_delayTime = delayTime;
				m_startTime = Time.time;
            }
            else
            {
                m_canvasGroup.alpha = 0f;
                m_canvasGroup.blocksRaycasts = false;
            }
        }

		public void SetTotalScore(int score)
		{
			m_totalScoreText.text = "Total " + score.ToString();
		}

		public void SetNew(bool isNew)
		{
			m_textNew.enabled = isNew;
		}


        public void OnClick()
        {
			if (Time.time - m_startTime < m_delayTime) return;

            if (m_isTap)
                return;
            SceneLoadManager.Instance.Load(SceneType.Title);
            m_isTap = true;
        }
    }
}

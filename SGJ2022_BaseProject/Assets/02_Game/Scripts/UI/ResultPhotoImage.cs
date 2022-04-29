using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace SGJ
{

	public class ResultPhotoImage : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI m_scoreText = null;
		[SerializeField] private TextMeshProUGUI m_newInfoText = null;

		// Start is called before the first frame update
		void Start()
		{

		}

		public void Setup(Texture photoTexture, int score, int newCount)
		{
			RawImage photo = GetComponent<RawImage>();
			photo.texture = photoTexture;

			m_scoreText.text = score.ToString();

			if (newCount <= 0)
			{
				m_newInfoText.enabled = false;
			} else
			{
				m_newInfoText.text = "V”­Œ©x" + newCount.ToString();
			}
		}

	}
}


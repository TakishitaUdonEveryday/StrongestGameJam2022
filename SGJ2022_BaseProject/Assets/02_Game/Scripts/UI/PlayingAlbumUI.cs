using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SGJ
{

	public class PlayingAlbumUI : MonoBehaviour
	{
		const float TEXTURE_ONE_SIZE = 160.0f + 10.0f;

		[SerializeField] private RectTransform m_contentTr = null;
		[SerializeField] private List<RawImage> m_photoImages;

		private float m_nextPhotoPosX = TEXTURE_ONE_SIZE * 3.0f;

		// Start is called before the first frame update
		void Start()
		{

		}

		/// <summary>
		/// 新しい写真を設定 
		/// </summary>
		/// <param name="newTexture"></param>
		public void SetNewPhotoTexture(Texture newTexture)
		{
			StartCoroutine(CoPhotoRoll(newTexture));
		}

		private IEnumerator CoPhotoRoll(Texture newTexture)
		{
			const float ANIM_TIME = 0.2f;

			// テクスチャ割り当て 
			m_photoImages[m_photoImages.Count - 1].texture = newTexture;

			// 移動 
			float time = 0.0f;
			Vector2 contentPos = m_contentTr.anchoredPosition;
			while (time < ANIM_TIME)
			{
				time += Time.deltaTime;
				m_contentTr.anchoredPosition = new Vector2(contentPos.x - TEXTURE_ONE_SIZE * Mathf.Clamp01(time / ANIM_TIME), contentPos.y);
				yield return null;
			}

			// 写真next
			RawImage lastImage = m_photoImages[0];
			m_photoImages.RemoveAt(0);
			RectTransform rectTr = lastImage.GetComponent<RectTransform>();
			rectTr.anchoredPosition = new Vector2(m_nextPhotoPosX, rectTr.anchoredPosition.y);
			m_nextPhotoPosX += TEXTURE_ONE_SIZE;

			// 末尾に追加 
			m_photoImages.Add(lastImage);
		}
	}

}


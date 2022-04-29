using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SGJ
{

	public class ResultPhotoAlbumUI : MonoBehaviour
	{
		[SerializeField] private ResultPhotoImage m_resultPhotoPrefab = null;

		// Start is called before the first frame update
		void Start()
		{

		}

		public void StartDisplay(List<PlayerController.PhotoData> photoDataList)
		{
			gameObject.SetActive(true);
			StartCoroutine(CoDisplay(photoDataList));
		}

		private IEnumerator CoDisplay(List<PlayerController.PhotoData> photoDataList)
		{
			const float DELAY = 0.2f;

			foreach (var photo in photoDataList)
			{
				var photoImage = Instantiate(m_resultPhotoPrefab, transform);
				photoImage.Setup(photo.m_texture, photo.m_score, photo.m_newCount);
				yield return new WaitForSeconds(DELAY);
			}
		}

	}

}


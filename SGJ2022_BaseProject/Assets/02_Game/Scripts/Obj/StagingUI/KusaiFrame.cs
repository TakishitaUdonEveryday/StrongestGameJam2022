using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SGJ
{
    public class KusaiFrame : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] m_kusaiTextures;

        private Image m_image;

        private void Start()
        {
            m_image = GetComponent<Image>();
        }

        private void Update()
        {
            if (PointManager.Instance.KusaiLevel3 <= PointManager.Instance.KusaiPoint)
            {
                m_image.sprite = m_kusaiTextures[2];
            }
            else if (PointManager.Instance.KusaiLevel2 <= PointManager.Instance.KusaiPoint)
            {
                m_image.sprite = m_kusaiTextures[1];
            }
            else if (PointManager.Instance.KusaiLevel1 <= PointManager.Instance.KusaiPoint)
            {
                m_image.sprite = m_kusaiTextures[0];
                m_image.color = Color.white;
            }
        }
    }
}
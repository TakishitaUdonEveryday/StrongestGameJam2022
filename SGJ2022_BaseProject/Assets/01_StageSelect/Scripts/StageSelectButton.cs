using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SGJ
{
    public class StageSelectButton : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_text = null;

		[SerializeField] private TextMeshProUGUI m_recordText = null;

        private int m_num = 0;

        public void Set(string levelName, int num)
        {
            m_text.text = levelName;
            m_num = num;

			// ãLò^Çê›íË 
			int record = SaveData.GetRecord(num);
			if (record <= 0)
			{
				m_recordText.text = "í≤ç∏ãLò^Ç»Çµ";
			} else
			{
				m_recordText.text = record.ToString() + "pts";
			}
        }



        public void OnClick()
        {
            GameDataManager.Instance.GameData.StageNum = m_num;
            SceneLoadManager.Instance.Load(SceneType.Main);
        }
    }
}

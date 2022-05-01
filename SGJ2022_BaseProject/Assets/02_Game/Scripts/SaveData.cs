using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SGJ
{

	public class SaveData
	{
		const string KEY = "STAGE";

		static public int	GetRecord(int stageNo)
		{
			return PlayerPrefs.GetInt(KEY + stageNo.ToString(), 0);
		}

		static public void	SetRecord(int stageNo, int newRecord)
		{
			string key = KEY + stageNo.ToString();
			int oldRecord = PlayerPrefs.GetInt(key, 0);
			if (oldRecord < newRecord)
			{
				PlayerPrefs.SetInt(key, newRecord);
				PlayerPrefs.Save();
			}
		}
	}

}


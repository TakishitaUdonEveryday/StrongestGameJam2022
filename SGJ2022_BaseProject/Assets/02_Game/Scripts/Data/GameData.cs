using UnityEngine;
using UnityEditor;

namespace SGJ
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "GameData", menuName = "Create/GameData")]
    public class GameData : ScriptableObject
    {
        [Label("ステージナンバー"), SerializeField]
        private int m_stageNum = 0;

        [Label("最大ステージ数"), SerializeField]
        private int m_maxStage = 1;

        public int StageNum { get => m_stageNum; set => m_stageNum = value; }
     //   public int MaxStage { get => m_maxStage; }
        public int MaxStage { get { return m_sceneName.Length; } }

        [SerializeField] private string[] m_stageName;

        [SerializeField] private string[] m_sceneName;
        
        public string GetSceneName()
		{
            return m_sceneName[StageNum % m_sceneName.Length];
        }

        public string GetStageName(int i)
		{
            return m_stageName[i % m_stageName.Length];

        }

    }
}
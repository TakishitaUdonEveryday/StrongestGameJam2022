using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGJ
{
    public class PointManager : SingletonMonoBehaviour<PointManager>
    {
        [SerializeField]
        private int m_gameOverPoint = 5;

        private int m_kusaiPoint = 0;

        public int GameOverPoint => m_gameOverPoint;

        public int KusaiPoint
        {
            get { return m_kusaiPoint; }
            set { m_kusaiPoint += value; }
        }
    }
}
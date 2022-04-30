using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGJ
{
    public class PointManager : SingletonMonoBehaviour<PointManager>
    {
        [SerializeField]
        private int m_kusaiLevel1 = 3;
        [SerializeField]
        private int m_kusaiLevel2 = 7;
        [SerializeField]
        private int m_kusaiLevel3 = 10;

        private int m_kusaiPoint = 0;

        public int KusaiLevel1 => m_kusaiLevel1;
        public int KusaiLevel2 => m_kusaiLevel2;
        public int KusaiLevel3 => m_kusaiLevel3;

        public int KusaiPoint
        {
            get { return m_kusaiPoint; }
            set { m_kusaiPoint += value; }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGJ
{
    public class KusaiItem : ItemBase
    {
        [SerializeField]
        private int m_kusaiPoint = 1;

        protected override void Hit()
        {
            PointManager.Instance.KusaiPoint = m_kusaiPoint;
            base.Hit();
        }
    }
}
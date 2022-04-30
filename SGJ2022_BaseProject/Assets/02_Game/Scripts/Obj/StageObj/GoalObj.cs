using UnityEngine;
using System.Collections;

namespace SGJ
{
    public class GoalObj : SingletonMonoBehaviour<GoalObj>
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (PointManager.Instance.KusaiPoint < PointManager.Instance.KusaiLevel3)
                {
                    GameManager.Instance.GameClear();
                }
                else
                {
                    GameManager.Instance.GameOver();
                }
            }
        }
    }
}

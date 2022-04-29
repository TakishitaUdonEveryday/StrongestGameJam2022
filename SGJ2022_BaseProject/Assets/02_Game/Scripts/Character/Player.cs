using UnityEngine;
using System.Collections;

namespace SGJ
{
    // プレイヤー固有の関数（アイテム影響）などを記述
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_kusaiParticlePrefab = null;

        /// <summary>
        /// プレイヤーにくさいパーティクルを生成
        /// </summary>
        public void SetKusaiParticle()
        {
            Vector3 pos = new Vector3(0, 1.3f, 0);
            GameObject kusai = Instantiate(m_kusaiParticlePrefab, transform.position + pos, Quaternion.identity) as GameObject;
            kusai.transform.parent = transform;
        }
    }
}

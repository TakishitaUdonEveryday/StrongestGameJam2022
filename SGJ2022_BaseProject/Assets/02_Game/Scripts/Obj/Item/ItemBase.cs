using UnityEngine;
using System.Collections;

namespace SGJ
{
    public class ItemBase : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.gameObject.GetComponent<Player>().SetKusaiParticle();
                Hit();
            }
        }

        virtual protected void Hit()
        {
            GameDebug.Log("アイテムゲット！");
            Destroy(gameObject);
        }
    }
}

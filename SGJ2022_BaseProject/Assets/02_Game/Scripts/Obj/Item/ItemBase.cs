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
                Hit(other.gameObject);
            }
        }

        virtual protected void Hit(GameObject obj)
        {
            GameDebug.Log("アイテムゲット！");
            Destroy(gameObject);
        }
    }
}

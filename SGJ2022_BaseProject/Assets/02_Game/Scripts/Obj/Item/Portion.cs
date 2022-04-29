using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SGJ;

public class Portion : ItemBase
{
    /// <summary>
    /// �񕜗�
    /// </summary>
    [SerializeField]
    private int m_Heal = 1;


    protected override void Hit(GameObject obj)
    {
        base.Hit(obj);

        var player = obj.GetComponent<Player>();
        if (player != null)
        {
            // HP��
            player.HP = player.HP + m_Heal;
        }

    }
}

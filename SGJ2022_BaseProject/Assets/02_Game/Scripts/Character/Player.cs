﻿using UnityEngine;
using System.Collections;

namespace SGJ
{
    // プレイヤー固有の関数（アイテム影響）などを記述
    public class Player : CharacterBase
    {
        private void Start()
        {
            NpcManager.Instance.SetPlayer(transform);
        }




    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// オブジェクトの種類の種類
public enum ObjectType
{
    None,
    Player,
    Enemy,
}


public class CharacterBase : MonoBehaviour
{
    /// <summary>
    /// HP
    /// </summary>
    [SerializeField]
    protected int m_Hp = 10;

    public int HP
    {
        get { return m_Hp; }
        set { m_Hp = value; }
    }

    /// <summary>
    /// オブジェクトの種類(コリジョン生成用)
    /// </summary>
    protected ObjectType m_ObjectType = ObjectType.None;


    /// <summary>
    /// 攻撃用コリジョンが当たった場合に呼び出される
    /// </summary>
    public virtual void HitAttackCollision()
    {

    }

    
}

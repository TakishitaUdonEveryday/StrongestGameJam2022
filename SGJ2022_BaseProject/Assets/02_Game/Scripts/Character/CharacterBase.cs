using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// オブジェクトの種類の種類
[System.Serializable]
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

    public bool IsDeath { get => m_isDeath; }

    protected bool m_isDeath = false;

    /// <summary>
    /// オブジェクトの種類(コリジョン生成用)
    /// </summary>
    [SerializeField]
    protected ObjectType m_ObjectType = ObjectType.None;


    /// <summary>
    /// 攻撃用コリジョンが当たった場合に呼び出される
    /// </summary>
    public virtual void HitAttackCollision(GameObject other)
    {
        if (m_Hp <= 0)
            return;

        GameDebug.Log(m_ObjectType + "に当たった！");
        --m_Hp;
        if(m_Hp <= 0)
        {
            Death();
        }
        else
        {
            Damage(other.transform.position);
        }
    }

    virtual protected void Death()
    {
        GameDebug.Log(gameObject.name + "が死んだ");
    }

    virtual protected void Damage(Vector3 hitPos)
    {
    }
}

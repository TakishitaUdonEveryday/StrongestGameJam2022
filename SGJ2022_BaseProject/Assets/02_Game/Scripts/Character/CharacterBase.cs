using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �I�u�W�F�N�g�̎�ނ̎��
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
    /// �I�u�W�F�N�g�̎��(�R���W���������p)
    /// </summary>
    [SerializeField]
    protected ObjectType m_ObjectType = ObjectType.None;


    /// <summary>
    /// �U���p�R���W���������������ꍇ�ɌĂяo�����
    /// </summary>
    public virtual void HitAttackCollision(GameObject other)
    {
        if (m_Hp <= 0)
            return;

        GameDebug.Log(m_ObjectType + "�ɓ��������I");
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
        GameDebug.Log(gameObject.name + "������");
    }

    virtual protected void Damage(Vector3 hitPos)
    {
    }
}

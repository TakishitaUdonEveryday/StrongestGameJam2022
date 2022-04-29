using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �I�u�W�F�N�g�̎�ނ̎��
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
    /// �I�u�W�F�N�g�̎��(�R���W���������p)
    /// </summary>
    protected ObjectType m_ObjectType = ObjectType.None;


    /// <summary>
    /// �U���p�R���W���������������ꍇ�ɌĂяo�����
    /// </summary>
    public virtual void HitAttackCollision()
    {

    }

    
}

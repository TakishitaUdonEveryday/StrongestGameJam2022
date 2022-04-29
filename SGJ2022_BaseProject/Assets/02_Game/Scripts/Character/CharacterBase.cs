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

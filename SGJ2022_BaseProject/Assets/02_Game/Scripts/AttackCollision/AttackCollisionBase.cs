using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollisionBase : MonoBehaviour
{
    private static readonly float DEF_ACTIVE_TIME = 5.0f;

    [SerializeField]
    [Tooltip("�R���W������������")]
    protected float m_collisionActiveTime = DEF_ACTIVE_TIME;

    /// <summary>
    /// �����e�̎��
    /// </summary>
    protected ObjectType m_createType = ObjectType.None;

    /// <summary>
    /// �������t���O
    /// </summary>
    private bool m_isInit = false;

    /// <summary>
    /// �o�ߎ��ԃJ�E���g
    /// </summary>
    private float m_timeCnt = 0.0f;

    /// <summary>
    /// �^�C���J�E���g�R���[�`��
    /// </summary>
    private Coroutine m_timeCntCo = null;

    protected virtual void Awake()
    {
        // ���������u�Ԃ̓R���W�����𖳌��ɂ��Ă���
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// �R���W�����������`�J�n����
    /// ���Ԏw�� 0�ȉ��̏ꍇ�͉i���I�ɃR���W�������o���悤�ɂ���
    /// </summary>
    public void StartCollisionData(ObjectType type, Transform parentTrans, bool isParent, float time = 5.0f)
    {
        this.gameObject.SetActive(true);

        if (isParent)
        {
            this.gameObject.transform.parent = parentTrans;
            this.gameObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            this.gameObject.transform.position = parentTrans.position;
        }
        // �������ԏ㏑��
        m_collisionActiveTime = time;

        m_timeCnt = 0.0f;
        m_isInit = true;

        // ���Ԃ�0���傫���l���w�肳��Ă���Ƃ��̕b�������R���W������L���ɂ���
        if (0 < m_collisionActiveTime)
        {
            // �R���[�`�����J�n���Ă��Ȃ��ꍇ�͊J�n
            if (m_timeCntCo == null)
            {
                m_timeCntCo = StartCoroutine(CoTimeCnt());
            }
        }
        else
        {
            // �i���I�ɗL���ɂ���
            // �R���[�`�����J�n���Ă���ꍇ�͒�~
            if (m_timeCntCo == null)
            {
                StopCoroutine(m_timeCntCo);
            }
        }
        
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        Debug.Log("AttackCollisionBase��Update���Ă΂�Ă��܂��B");
    }

    private IEnumerator CoTimeCnt()
    {
        // �o�ߎ��Ԃ��J�E���g���Ďw��̕b�����z�����ꍇ�ɍ폜����
        while (m_timeCnt < m_collisionActiveTime)
        {
            m_timeCnt += Time.deltaTime;
            yield return null;
        }
        // �ݒ肳�ꂽ���Ԃ��o�߂����̂ō폜
        Destroy(this.gameObject);
    }


    /// <summary>
    /// �����蔻��
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!m_isInit)
        {
            return;
        }

        CharacterBase hitObj = other.gameObject.GetComponent<CharacterBase>();

        // ���������I�u�W�F�N�g�̎�ނ��e�̎�ނƈႤ�ꍇ�Ƀq�b�g�������s��
        if (hitObj)
        {
            // ���������Ƃ��̎����Ăяo��(�����͊e���ōs��)
            hitObj.HitAttackCollision();

            // ���i�q�b�g�����Ȃ����߂ɃI�u�W�F�N�g�폜
            Destroy(this.gameObject);
        }
    }
}

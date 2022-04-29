using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollisionBase : MonoBehaviour
{
    private static readonly float DEF_ACTIVE_TIME = 5.0f;

    [SerializeField]
    [Tooltip("コリジョン生存時間")]
    protected float m_collisionActiveTime = DEF_ACTIVE_TIME;

    /// <summary>
    /// 生成親の種類
    /// </summary>
    protected ObjectType m_createType = ObjectType.None;

    /// <summary>
    /// 初期化フラグ
    /// </summary>
    private bool m_isInit = false;

    /// <summary>
    /// 経過時間カウント
    /// </summary>
    private float m_timeCnt = 0.0f;

    /// <summary>
    /// タイムカウントコルーチン
    /// </summary>
    private Coroutine m_timeCntCo = null;

    protected virtual void Awake()
    {
        // 生成した瞬間はコリジョンを無効にしておく
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// コリジョン初期化～開始処理
    /// 時間指定 0以下の場合は永続的にコリジョンを出すようにする
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
        // 生存時間上書き
        m_collisionActiveTime = time;

        m_timeCnt = 0.0f;
        m_isInit = true;

        // 時間が0より大きい値が指定されているとその秒数だけコリジョンを有効にする
        if (0 < m_collisionActiveTime)
        {
            // コルーチンを開始していない場合は開始
            if (m_timeCntCo == null)
            {
                m_timeCntCo = StartCoroutine(CoTimeCnt());
            }
        }
        else
        {
            // 永続的に有効にする
            // コルーチンを開始している場合は停止
            if (m_timeCntCo == null)
            {
                StopCoroutine(m_timeCntCo);
            }
        }
        
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        Debug.Log("AttackCollisionBaseのUpdateが呼ばれています。");
    }

    private IEnumerator CoTimeCnt()
    {
        // 経過時間をカウントして指定の秒数を越した場合に削除する
        while (m_timeCnt < m_collisionActiveTime)
        {
            m_timeCnt += Time.deltaTime;
            yield return null;
        }
        // 設定された時間を経過したので削除
        Destroy(this.gameObject);
    }


    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!m_isInit)
        {
            return;
        }

        CharacterBase hitObj = other.gameObject.GetComponent<CharacterBase>();

        // 当たったオブジェクトの種類が親の種類と違う場合にヒット処理を行う
        if (hitObj)
        {
            // 当たったときの実装呼び出し(実装は各自で行う)
            hitObj.HitAttackCollision();

            // 多段ヒットさせないためにオブジェクト削除
            Destroy(this.gameObject);
        }
    }
}

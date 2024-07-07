using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstructEvent : MonoBehaviour
{
    [SerializeField]
    protected bool timeStopFlag = false; //時間を止めるか
    [SerializeField]
    protected bool moveWithoutTimeScale = false; //時間停止中も動かすか

    protected bool activeFlag = false; //イベントが発生しているか
    protected float time = 0.0f; //経過時間
    protected bool delFlag = false; //消去するか

    protected Player player; //自機のオブジェクト
    protected EventManager eveManager = null; //イベント関連のオブジェクト
    protected SoundManager soundManager = null; //BGM関連のオブジェクト
    protected SoundEffectManager soundEffectManager = null; //効果音関連のオブジェクト

    public bool ActiveFlag { set { activeFlag = value; } } //イベントが発生しているか
    public bool TimeStopFlag { get { return timeStopFlag; } } //時間を止めるか
    public bool DelFlag { get { return delFlag; } } //消去するか

    // Start is called before the first frame update
    protected void Start()
    {

    }

    // Update is called once per frame
    protected void Update()
    {
        if (!activeFlag) return; //起動していないなら飛ばす
        SubUpdate(); //継承側の更新
        //時間停止中も動くなら
        if (moveWithoutTimeScale)
        {
            time += Time.fixedDeltaTime; //時間を増加
        }
        //動かないなら
        else
        {
            time += Time.deltaTime; //時間を増加
        }
    }

    //継承側の更新
    protected virtual void SubUpdate() { }

    //継承側の更新
    protected virtual void SubFixUpdate() { }

    //イベントの起動
    public virtual void Activate() { }

    //最初の設定
    public void FirstSetting(EventManager em, Player p, SoundManager sm, SoundEffectManager sem)
    {
        eveManager = em;
        player = p;
        soundManager = sm;
        soundEffectManager = sem;
    }
}

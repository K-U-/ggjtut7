using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public abstract class UIAction : MonoBehaviour
{

    # region

    protected enum UIActionState{
        STATE_STOP = 0,
        STATE_MOVE,
        STATE_ROTE,
        STATE_FADEOUT,
        STATE_FADEIN,
        STATE_JUMP,
        STATE_END
    }

    [SerializeField]
    protected UIActionState mUIState;

    [SerializeField]
    protected Vector3 mMovePosition;

    [SerializeField]
    protected Vector3 mRotation;

    [SerializeField]
    protected Vector3 mScale;

    [SerializeField][Range(0.0f,1.0f)]
    protected float mAlpha;

    [SerializeField]
    [Range(0.0f, 100.0f)]
    protected float mTime;

    [SerializeField]
    [Range(0.0f, 100.0f)]
    protected float mDelay;

    [SerializeField]
    private LeanTweenType mType;
    [SerializeField]
    private iTween.EaseType mIEase;

    protected System.Action<GameObject,GameObject> mFinishAction;

    [SerializeField]
    protected CanvasGroup mGroup;

    protected delegate void SeqEnce();

    private List<SeqEnce> mSeq = new List<SeqEnce>();

    private int mSeqIndex = 0;

    //private bool mIsSeq = false;

    /// <summary>
    /// 状態のチェック
    /// </summary>
    /// <returns></returns>
    private UIActionState CheckUIState()
    {
        return mUIState;
    }

    /// <summary>
    /// シーケンス処理の追加
    /// </summary>
    /// <param name="function"></param>
    protected void AddSeq(SeqEnce function)
    {
        mSeq.Add(function);
    }

    /// <summary>
    /// シーケンス実行
    /// </summary>
    private IEnumerator NextSeq()
    {
        if (mSeq.Count == 0) yield break;
        mSeq[mSeqIndex]();
        mSeqIndex++;

    }

    /// <summary>
    /// シーケンス
    /// </summary>
    /// <returns></returns>
    private IEnumerator Seq()
    {
        //foreach (var i in mSeq)
        for (int i = 0; i < mSeq.Count; i++ )
        {
            var seq = StartCoroutine(NextSeq());
            yield return seq;
            while (CheckUIState() != UIActionState.STATE_STOP)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    /// <summary>
    /// 終了時の処理
    /// </summary>
    public virtual void Action() {
        UnityEngine.Debug.Log("アクション実行 : "+"overrideしてもしなくても良いよ。");
    }

    /// <summary>
    /// シーケンス実行
    /// </summary>
    protected void CallSeq()
    {
        StartCoroutine(Seq());
    }

    private LTDescr SetUpLtdescr(LTDescr d,System.Action action = null)
    {
        d.setDelay(mDelay);
        d.setEase(mType);
        if (action != null) d.setOnComplete(() => { mUIState = UIActionState.STATE_STOP; action(); });
        return d;
    }

    /// <summary>
    /// UIの移動
    /// </summary>
    protected void Move()
    {
        Debug.Log(mUIState);
        int id = LeanTween.moveLocal(gameObject,mMovePosition,mTime).id;
        LTDescr move = LeanTween.description(id);
        SetUpLtdescr(move);
        move.setOnComplete(() => { mUIState = UIActionState.STATE_STOP; });
    }

    /// <summary>
    /// 行動後に関数実行
    /// </summary>
    /// <param name="action"></param>
    protected void Move(System.Action action)
    {
        mUIState = UIActionState.STATE_MOVE;
        int id = LeanTween.moveLocal(gameObject, mMovePosition, mTime).id;
        LTDescr move = LeanTween.description(id);
        //SetUpLtdescr(move, action);
        move.setOnComplete(() => { mUIState = UIActionState.STATE_STOP; action(); });
    }

    protected void AlphaMove(System.Action action)
    {
        mUIState = UIActionState.STATE_MOVE;
        int id = LeanTween.moveLocal(gameObject, mMovePosition, mTime).id;
        LTDescr move = LeanTween.description(id);
        Alpha();
        move.setOnComplete(() => { mUIState = UIActionState.STATE_STOP; action(); });
    }

    protected void AlphaMove(System.Action action,GameObject target)
    {
        mUIState = UIActionState.STATE_MOVE;
        int id = LeanTween.moveLocal(target, mMovePosition, mTime).id;
        LTDescr move = LeanTween.description(id);
        Alpha();
        move.setOnComplete(() => { mUIState = UIActionState.STATE_STOP; action(); Destroy(target); });
    }

    protected void Alpha()
    {
        mUIState = UIActionState.STATE_FADEOUT;
        Debug.Log(mUIState);
        iTween.ValueTo(this.gameObject, iTween.Hash("from", 1.0f, "to", mAlpha, "time", mTime, "delay", mDelay, "easetype", mIEase, "onupdate", "CanvasAlphaUpdate","oncomplete","Reset"));
    }

    protected void Reset()
    {
        mGroup.alpha = 1;
    }

    protected void Scale()
    {
        mUIState = UIActionState.STATE_ROTE;
        int id = LeanTween.scale(gameObject, mScale, mTime).id;
        LTDescr scale = LeanTween.description(id);
        scale.setOnComplete(() => { mUIState = UIActionState.STATE_STOP; });
    }

    protected void Scale(System.Action action)
    {
        mUIState = UIActionState.STATE_ROTE;
        int id = LeanTween.scale(gameObject, mScale, mTime).id;
        LTDescr scale = LeanTween.description(id);
        scale.setOnComplete(() => { mUIState = UIActionState.STATE_STOP; action(); });
    }

    /// <summary>
    /// キャンバス内の透明度
    /// </summary>
    /// <param name="value"></param>
    private void CanvasAlphaUpdate(float value)
    {
        mGroup.alpha = value;
    }

    [ContextMenu("AttachCanvasGroup")]
    private void AttachCanvasGroup()
    {
        //Canvasを持ってるオブジェクトにGroupを付加する。
    }
    # endregion
}

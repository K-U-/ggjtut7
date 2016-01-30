using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StrikeInputUtility : MonoBehaviour {

    [SerializeField]
    private float mSpeed = 3;
    [SerializeField]
    private float mTime;

    [SerializeField]
    private Text mUIText;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        InputStrike();
	}

    /// <summary>
    /// 擦りの更新関数
    /// </summary>
    private void InputStrike()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            float d = Vector2.Distance(touchDeltaPosition, Input.GetTouch(0).position);
            mTime += Input.GetTouch(0).deltaTime * mSpeed;
        }
        UpdateTargetMagicUpdate();
    }

    /// <summary>
    /// 1以上かの検知
    /// </summary>
    /// <returns></returns>
    private bool IsMax()
    {
        if (1 <= mTime) return true ;
        return false;
    }

    /// <summary>
    /// 擦り値
    /// </summary>
    /// <returns></returns>
    public float GetStrickValue()
    {
        return mTime;
    }

    private void UpdateTargetMagicUpdate()
    {
        mUIText.text = mTime.ToString("F2");
    }
    private void Reset()
    {
        mTime = 0.0f;
    }

}

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

	private Vector2 startPosistion;
	private Vector2 endPosition;
	private bool onMouse = false;

	float distance1, distance2;
	public bool isScratch = false;

	float startTime, time;

	StageController stageController;

	Vector2[] mahoujinsPositions;

	MahojinController mahojinController;

	// Use this for initialization
	void Start () {
		time = 0;
		stageController = GameObject.Find ("Stage").GetComponent<StageController> ();
		mahoujinsPositions = new Vector2[stageController.mahojinsPos.Length];
		for (int i = 0; i < stageController.mahojinsPos.Length; i++) {
			mahoujinsPositions [i] = stageController.mahojinsPos[i];
		}
	}

	void Update() {
		InputStrike ();

		Vector2 myPosition = new Vector2 (this.transform.position.x, this.transform.position.z);
		for (int i = 0; i < mahoujinsPositions.Length; i++) {
			if (mahojinController == null) {
				if (Vector2.Distance (myPosition, mahoujinsPositions [i]) < 1) {
					mahojinController = GameObject.Find ("mahojin" + i).GetComponent<MahojinController> ();
				} else {
					mahojinController = null;
				}
			} else  {
				mahojinController.AddGauge((int)time);
			}
		}
	}

    /// <summary>
    /// 擦りの更新関数
    /// </summary>
    private void InputStrike()
    {
		#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0)) {
			startPosistion = Input.mousePosition;
			onMouse = true;
			startTime = Time.timeSinceLevelLoad;
			time = 0;
		}
		if (Input.GetMouseButtonUp(0)) {
			// endPosition = Input.mousePosition;
			onMouse = false;
			isScratch = false;
		}

		if (onMouse) {
			distance1 = Vector2.Distance(startPosistion, Input.mousePosition);
			Invoke("GetMousePoint", 0.1f);
			distance2 = Vector2.Distance(Input.mousePosition, endPosition);
			if (distance1 != distance2) {
				time = Time.timeSinceLevelLoad - startTime;
				isScratch = true;
			}
		} else {
			time = 0;
			isScratch = false;
		}

		Debug.Log(time);
		// Debug.Log(Vector2.Distance(startPosistion, endPosition));

		#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            float d = Vector2.Distance(touchDeltaPosition, Input.GetTouch(0).position);
            mTime += Input.GetTouch(0).deltaTime * mSpeed;
        }
        UpdateTargetMagicUpdate();
		#endif
    }

	void GetMousePoint() {
		endPosition= Input.mousePosition;
	}

	void OnMouseDrag() {
		Debug.Log ("OnmouseDrag");
	}

    /// <summary>
    /// 1以上かの検知
    /// </summary>
    /// <returns></returns>
    private bool IsMax()
    {
        // if (1 <= mTime) return true ;
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
        // mUIText.text = mTime.ToString("F2");
    }
    private void Reset()
    {
        mTime = 0.0f;
    }

}

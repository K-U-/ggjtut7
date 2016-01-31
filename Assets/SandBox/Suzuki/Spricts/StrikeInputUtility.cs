using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class StrikeInputUtility : MonoBehaviour {

    [SerializeField]
    private float mSpeed = 3;
    [SerializeField]
    private float mTime;

	private Vector2 startPosistion;
	private Vector2 endPosition;
	private bool onMouse = false;

	float distance1, distance2;
	public bool isScratch = false;

	float startTime, time;

	StageController stageController;

	Vector2[] mahoujinsPositions;

	MahojinController[] mahojinControllers;

	bool isHuman;
	bool akumaMode;

	// Use this for initialization
	void Start () {
		time = 0;
		stageController = GameObject.Find ("Stage").GetComponent<StageController> ();
		mahoujinsPositions = new Vector2[stageController.mahojinsPos.Length];
		for (int i = 0; i < stageController.mahojinsPos.Length; i++) {
			mahoujinsPositions [i] = stageController.mahojinsPos[i];
		}
		mahojinControllers = new MahojinController[mahoujinsPositions.Length];
		for (int i = 0; i < mahojinControllers.Length; i++) {
			mahojinControllers [i] = GameObject.Find ("mahojin" + i).GetComponent<MahojinController>();
		}

		isHuman = GameManager.GetInstance ().myInfo.isHuman;
		akumaMode = false;
	}

	void Update() {
		InputStrike ();

		Vector2 myPosition = new Vector2 (this.transform.position.x, this.transform.position.z);
		MahojinController mahojinController = null;
		for (int i = 0; i < mahoujinsPositions.Length; i++) {
			if (mahojinController == null) {
				if (Vector2.Distance (myPosition, mahoujinsPositions [i]) < 1) {
					mahojinController = mahojinControllers[i];
				} else {
					mahojinController = null;
				}
			} else  {
				if (akumaMode && (isHuman == false)) {
					mahojinController.subGauge ((int)time * 2);
				} else {
					mahojinController.AddGauge((int)time * 2);
				}
			}
		}
	}



	void OnGUI() {
		if(GameManager.GetInstance().myInfo.id.ToString() == gameObject.name)
		GUI.TextArea (new Rect (0, 0, 100, 30), akumaMode.ToString());
	}

	public void ChangeMode() {
		if (akumaMode == true) {
			akumaMode = false;
		} else {
			akumaMode = true;
		}
	}

    /// <summary>
    /// 擦りの更新関数
    /// </summary>
    private void InputStrike()
    {
		#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE
		if (Input.GetMouseButtonDown(0)) {
			startPosistion = Input.mousePosition;
			onMouse = true;
			startTime = Time.timeSinceLevelLoad;
			time = 0;
		}
		if (Input.GetMouseButtonUp(0)) {
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

		#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            float d = Vector2.Distance(touchDeltaPosition, Input.GetTouch(0).position);
            mTime += Input.GetTouch(0).deltaTime * mSpeed;
        }
		#endif
    }

	void GetMousePoint() {
		endPosition= Input.mousePosition;
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

    private void Reset()
    {
        mTime = 0.0f;
    }

}

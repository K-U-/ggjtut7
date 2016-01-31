using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
public class DrowRanking : MonoBehaviour {

    [SerializeField]
    private List<Text> mValueList = new List<Text>();
    [SerializeField]
    private List<Text> mNameList = new List<Text>();

    private List<PlayerReadyStatus> mPlayerInfo = new List<PlayerReadyStatus>();

    [SerializeField]
    private bool mIsRound;

    private bool mWait = true;
    private int mIndex;

    /// <summary>
    /// 勢力図
    /// </summary>
    [SerializeField]
    private GameObject mPowerStructure;

    [SerializeField]
    private GameObject mPopUpUI;

    float waitTime = 2.0f;

	// Use this for initialization
	void Start () {
        if (mIsRound) Shake();

        mPlayerInfo = new List<PlayerReadyStatus>(GameManager.GetInstance().ReadyStatusList.readyStatusList);
        //mPlayerInfo[0].info.point;

        //昇順に並び替え
        mPlayerInfo.Sort(delegate(PlayerReadyStatus mca1, PlayerReadyStatus mca2) { return mca1.info.point - mca2.info.point; });

        SearchRankingValue();
        mIndex = mValueList.Count - 1;
        StartCoroutine(DrowSeqence());
	}

    private void Update()
    {
        if (GameManager.GetInstance().myInfo.isHost &&
        GameManager.GetInstance().ReadyStatusList.readyStatusList.Count (_ => !_.info.isSpector) >= 4)
        {
            //リザルト終了ダイアログを出す。
            mPopUpUI.SetActive(true);
        }
        else
        {
            //リザルト終了ダイアログを出す。
            mPopUpUI.SetActive(false);
        }
    }

    private void Shake()
    {
        {
            mPowerStructure.SetActive(true);
            iTween.ShakePosition(mPowerStructure.transform.GetChild(0).gameObject,
                iTween.Hash("amount", new Vector3(100, 0, 0), "delay", 1.0f, "time", waitTime, "oncomplete", "OutPutPowerStructure"));
            //iTween.ShakePosition(mPowerStructure.transform.GetChild(0).gameObject,
            //    new Vector3(100.0f,0.0f,0.0f),waitTime);
        }
    }

    /// <summary>
    /// リストの設定
    /// </summary>
    private void SearchRankingValue()
    {
        foreach (Transform i in transform)
        {
            mValueList.Add(i.FindChild("Ranking_Value").GetComponent<Text>());
            mNameList.Add(i.FindChild("Ranking_Name").GetComponent<Text>());
        }
    }

    /// <summary>
    /// どんどんリザルトを確定表示させていく。
    /// </summary>
    /// <returns></returns>
    private IEnumerator DrowSeqence()
    {
        StartCoroutine(Wait());
        //while(mIndex > 0)
        {
            while (mIndex > 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    int rand = Random.Range(0, 20);
                    mValueList[mIndex].text = rand.ToString();
                }
                yield return new WaitForEndOfFrame();
            }
        }

        yield return new WaitForSeconds(waitTime);

        //最後のプレイヤーの役職を判定してIconを変える
        if (!mIsRound)
        {
            //どっちサイドの勝利かを確認する必要がるあ。
            Debug.Log("どっちサイドの勝利かを確認する必要がるあ");
            GameObject.Find("Result_Winner_Icon_A").GetComponent<EasyTween>().OpenCloseObjectAnimation();
            GameObject.Find("Result_Winner_Icon_D").GetComponent<EasyTween>().OpenCloseObjectAnimation();
        }

    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        for(int i = 0; i < mValueList.Count;i++)
        {
            //TODO ゲームマネージャーから取得する。
            mValueList[mIndex].text = mPlayerInfo[mIndex].info.point.ToString();
            mNameList[mIndex].text = mPlayerInfo[mIndex].info.name;
            UnityEngine.Debug.Log(mPlayerInfo[mIndex].info.point.ToString());
            mIndex--;
            yield return new WaitForSeconds(waitTime);

        }

        //リザルト終了ダイアログを出す。
        mPopUpUI.GetComponent<EasyTween>().OpenCloseObjectAnimation();
        
    }

}

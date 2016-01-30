using UnityEngine;
using System.Collections;

public class ArrowEvent : MonoBehaviour {

    MoveCommand sampleCommand;
    KillCommand killcmd;
    private string PlayerID;
    private bool killflag;
    private bool[] death;
    public float watetime;

    void Awake()
    {
        sampleCommand = new MoveCommand();
        killcmd = new KillCommand();
        PlayerID = GameManager.GetInstance().myInfo.id.ToString();
        killflag = false;
        death = new bool[8];
        for (int i = 0; i < 8; i++) {
            death[i] = false;
        }
    }

    
    void move(string id, float x, float z) {
        if (death[int.Parse(id)] == false)
        {
            sampleCommand.target = id;
            sampleCommand.offsetX = x;
            sampleCommand.offsetZ = z;
            PhotonRPCModel model = new PhotonRPCModel();
            model.senderId = PlayerID;
            model.command = PhotonRPCCommand.Move;
            model.message = JsonUtility.ToJson(sampleCommand);
            PhotonRPCHandler.GetInstance().PostRPC(model);
        }
    }

    #region 矢印キーの取得
    public void MoveUp()
    {
        move(PlayerID, 0f, 1f);
    }

    public void MoveDown()
    {
        move(PlayerID, 0f, -1f);
    }

    public void MoveRight()
    {
        move(PlayerID, 1f, 0f);
    }

    public void MoveLeft()
    {
        move(PlayerID, -1f, 0f);
    }
    #endregion

    public void Update() {

        Debug.Log(killflag);

        if (killflag == true) {
            Kill();
        }
    }

    #region Killに関する処理
    void Kill() {
        if (TouchPlatform() == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                raycast();
            }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    raycast();
                }
            }
        }
    }


    bool TouchPlatform()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return true;
        }
        return false;
    }

    void raycast() {
        RaycastHit hit;

        // 床との当たり判定のみを取りたいのでマスクするレイヤーを設定する
        int layerNo = LayerMask.NameToLayer("Player");
        int layerMask = 1 << layerNo;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float fDistance = 100.0f;

        //レイを投射してオブジェクトを検出
        if (Physics.Raycast(ray, out hit, fDistance, layerMask))
        {
            if (PlayerID != hit.collider.gameObject.name)
            {
                killcmd.target = hit.collider.gameObject.name;
                PhotonRPCModel model = new PhotonRPCModel();
                model.senderId = PlayerID;
                model.command = PhotonRPCCommand.Kill;
                model.message = JsonUtility.ToJson(killcmd);
                PhotonRPCHandler.GetInstance().PostRPC(model);

                StartCoroutine(respawn(hit.collider.gameObject));
                killflag = false;
            }
        }
    }

    public void ChangeMode() {
        if (killflag == false) {
            killflag = true;
        } else if (killflag == true) {
            killflag = false;
        }
    }

    IEnumerator respawn(GameObject hit) {
        death[int.Parse(hit.name)] = true;
        yield return new WaitForSeconds(watetime);
        death[int.Parse(hit.name)] = false;
    }
    #endregion
}

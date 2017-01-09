using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleWindow : BaseWindowWrapper<BattleWindow> {
    private const float areaSpeedY = 15;
    private const int height = 1100;
    private const int gridAreaNum = 3;
    private const int returnNum = 99;
    private PlayerInfoProxy playerInfoProxy;
    private MakeInfoProxy makeInfoProxy;
    private PlayerCtrl playerCtrl;
    private BattleProgress battleProgress;
    private Text txtBattleInfo;
    public RoomProxy[] arrGridArea;
    private GameObject downArea;
    private LimitProxy limitProxy;
    private int lastIndex = -1;
    private float curAreaY;
    private float targetAreaY;

    protected override void InitCtrl() {
        playerInfoProxy = gameObject.GetChildControl<Transform>("CvsLeftUI").gameObject.AddMissingComponent<PlayerInfoProxy>();
        makeInfoProxy = gameObject.GetChildControl<Transform>("CvsRightUI/imgMakeTitle/Make").gameObject.AddMissingComponent<MakeInfoProxy>();
        battleProgress = gameObject.GetChildControl<Transform>("CvsRightUI/imgProgressBg").gameObject.AddMissingComponent<BattleProgress>();
        txtBattleInfo = gameObject.GetChildControl<Text>("CvsRightUI/imgInfo/txtInfo");

        playerCtrl = gameObject.GetChildControl<Transform>("DownArea/Player").gameObject.AddMissingComponent<PlayerCtrl>();
        arrGridArea = new RoomProxy[gridAreaNum];
        for (int index = 0; index < gridAreaNum; index++ ) {
            GameObject gridArea = gameObject.GetChildControl<Transform>("DownArea/CvsGridArea" + (index+1)).gameObject;
            gridArea.transform.localPosition = new Vector3(0, -height * index, 0);
            RoomProxy roomProxy = gridArea.AddMissingComponent<RoomProxy>();
            roomProxy.Init();
            arrGridArea[index] = roomProxy;
        }
        downArea = gameObject.GetChildControl<Transform>("DownArea").gameObject;
        limitProxy = gameObject.GetChildControl<Transform>("DownArea/Limit").gameObject.AddMissingComponent<LimitProxy>();
    }

    protected override void OnPreOpen() {
    }

    protected override void OnOpen() {
    }

    protected override void InitMsg() {
        Send.RegisterMsg(SendType.PlayerYMove, OnPlayerYMove);
    }

    protected override void ClearMsg() {
        Send.UnregisterMsg(SendType.PlayerYMove, OnPlayerYMove);
    }

    void FixedUpdate() {
        AreaMoveUpdate();
        playerCtrl.FixedUpdateHandle();
    }

    void Update() {
        playerCtrl.UpdateHandle();
        //limitProxy.UpdateHandle();
    }

    private void AreaMoveUpdate() {
        curAreaY = Mathf.SmoothStep(curAreaY, targetAreaY, Time.fixedDeltaTime * areaSpeedY);
        downArea.transform.localPosition = new Vector3(0, curAreaY, 0);

        int index = (int)curAreaY / height % gridAreaNum;
        if (index != lastIndex) {
            lastIndex = index;
            switch (index) {
                case 0:
                    arrGridArea[1].transform.localPosition = arrGridArea[0].transform.localPosition + new Vector3(0, -height, 0);
                    RoomCreatMgr.Instance.CreatNewRoom(arrGridArea[1]);
                    break;
                case 1:
                    arrGridArea[2].transform.localPosition = arrGridArea[1].transform.localPosition + new Vector3(0, -height, 0);
                    RoomCreatMgr.Instance.CreatNewRoom(arrGridArea[2]);
                    break;
                case 2:
                    arrGridArea[0].transform.localPosition = arrGridArea[2].transform.localPosition + new Vector3(0, -height, 0);
                    RoomCreatMgr.Instance.CreatNewRoom(arrGridArea[0]);
                    break;
            }
        }
        if (curAreaY > returnNum * height) {
            float returnPosY = returnNum * height;
            targetAreaY = targetAreaY - returnPosY;
            curAreaY = curAreaY - returnPosY;
            downArea.transform.localPosition = new Vector3(0, downArea.transform.localPosition.y - returnPosY, 0);
            arrGridArea[0].transform.localPosition = new Vector3(0, arrGridArea[0].transform.localPosition.y + returnPosY, 0);
            arrGridArea[1].transform.localPosition = new Vector3(0, arrGridArea[1].transform.localPosition.y + returnPosY, 0);
            arrGridArea[2].transform.localPosition = new Vector3(0, arrGridArea[2].transform.localPosition.y + returnPosY, 0);
            playerCtrl.transform.localPosition = new Vector3(0, playerCtrl.transform.localPosition.y + returnPosY, 0);
            limitProxy.transform.localPosition = new Vector3(0, limitProxy.transform.localPosition.y + returnPosY, 0);
        }
    }

    private void OnPlayerYMove(object[] objs) {
        float deltaY = (float)objs[0];
        if (deltaY >= height * (returnNum - 1) ) {
            deltaY = deltaY - height * returnNum;
        }
        targetAreaY = targetAreaY - deltaY;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleWindow : BaseWindowWrapper<BattleWindow> {
    private const float areaSpeedY = 15;
    private const int height = 1080;
    private const int gridAreaNum = 3;
    private PlayerInfoProxy playerInfoProxy;
    private MakeInfoProxy makeInfoProxy;
    private PlayerCtrl playerCtrl;
    private BattleProgress battleProgress;
    private Text txtBattleInfo;
    private GameObject[] arrGridArea;
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
        arrGridArea = new GameObject[gridAreaNum];
        for (int index = 0; index < gridAreaNum; index++ ) {
            GameObject gridArea = gameObject.GetChildControl<Transform>("DownArea/CvsGridArea" + (index+1)).gameObject;
            arrGridArea[index] = gridArea;
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
        limitProxy.UpdateHandle();
    }

    private void AreaMoveUpdate() {
        curAreaY = Mathf.SmoothStep(curAreaY, targetAreaY, Time.fixedDeltaTime * areaSpeedY);
        downArea.transform.localPosition = new Vector3(0, curAreaY, 0);

        int index = (int)curAreaY / height % gridAreaNum;
        if (index != lastIndex) {
            lastIndex = index;
            switch (index) {
                case 0:
                    arrGridArea[1].transform.localPosition = arrGridArea[0].transform.localPosition + new Vector3(0, -1080, 0);
                    break;
                case 1:
                    arrGridArea[2].transform.localPosition = arrGridArea[1].transform.localPosition + new Vector3(0, -1080, 0);
                    break;
                case 2:
                    arrGridArea[0].transform.localPosition = arrGridArea[2].transform.localPosition + new Vector3(0, -1080, 0);
                    break;
            }
        }
    }

    private void OnPlayerYMove(object[] objs) {
        float deltaY = (float)objs[0];
        targetAreaY = targetAreaY - deltaY;
    }
}

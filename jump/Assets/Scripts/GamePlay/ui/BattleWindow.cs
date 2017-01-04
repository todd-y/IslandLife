using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleWindow : BaseWindowWrapper<BattleWindow> {
    private PlayerInfoProxy playerInfoProxy;
    private MakeInfoProxy makeInfoProxy;
    private PlayerCtrl playerCtrl;
    private BattleGridCtrl battleGridCtrl;
    private BattleProgress battleProgress;
    private Text txtBattleInfo;

    protected override void InitCtrl() {
        playerInfoProxy = gameObject.GetChildControl<Transform>("CvsLeftUI").gameObject.AddMissingComponent<PlayerInfoProxy>();
        makeInfoProxy = gameObject.GetChildControl<Transform>("CvsRightUI/imgMakeTitle/Make").gameObject.AddMissingComponent<MakeInfoProxy>();
        playerCtrl = gameObject.GetChildControl<Transform>("Player").gameObject.AddMissingComponent<PlayerCtrl>();
        battleGridCtrl = gameObject.GetChildControl<Transform>("CvsGrid").gameObject.AddMissingComponent<BattleGridCtrl>();
        battleGridCtrl.gridPrefab = gameObject.GetChildControl<Transform>("CvsGrid/Grid").gameObject;
        battleGridCtrl.gridPrefab.AddMissingComponent<BattleGrid>();
        battleGridCtrl.CreatGrid();

        battleProgress = gameObject.GetChildControl<Transform>("CvsRightUI/imgProgressBg").gameObject.AddMissingComponent<BattleProgress>();
        txtBattleInfo = gameObject.GetChildControl<Text>("CvsRightUI/imgInfo/txtInfo");
    }

    protected override void OnPreOpen() {
    }

    protected override void OnOpen() {
    }

    protected override void InitMsg() {
        Send.RegisterMsg(SendType.PlayerHit, OnPlayerHit);
    }

    protected override void ClearMsg() {
        Send.UnregisterMsg(SendType.PlayerHit, OnPlayerHit);
    }

    private void OnPlayerHit(object[] objs) {
        int xIndex = (int)objs[0];
        battleGridCtrl.RemoveGrid(xIndex, 0);
    }
}

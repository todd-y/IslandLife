using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleWindow : BaseWindowWrapper<BattleWindow> {

    private PlayerCtrl playerCtrl;
    private BattleGridCtrl battleGridCtrl;

    protected override void InitCtrl() {
        playerCtrl = gameObject.GetChildControl<Transform>("Player").gameObject.AddMissingComponent<PlayerCtrl>();
        battleGridCtrl = gameObject.GetChildControl<Transform>("CvsGrid").gameObject.AddMissingComponent<BattleGridCtrl>();
        battleGridCtrl.gridPrefab = gameObject.GetChildControl<Transform>("CvsGrid/Grid").gameObject;
        battleGridCtrl.gridPrefab.AddMissingComponent<BattleGrid>();
        battleGridCtrl.CreatGrid();
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
        battleGridCtrl.RemoveLine();
    }
}

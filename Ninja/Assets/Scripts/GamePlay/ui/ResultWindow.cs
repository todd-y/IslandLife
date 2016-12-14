using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultWindow : BaseWindowWrapper<ResultWindow> {
    public Button btnRestart;

    protected override void InitCtrl() {
    }

    protected override void OnPreOpen() {
    }

    protected override void OnOpen() {
    }

    protected override void InitMsg() {
        btnRestart.onClick.AddListener(OnRestartClick);
    }

    protected override void ClearMsg() {
        btnRestart.onClick.RemoveListener(OnRestartClick);
    }

    private void OnRestartClick() {
        BattleMgr.Instance.StartBattle();
    }
}

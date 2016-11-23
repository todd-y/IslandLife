using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingWindow : BaseWindowWrapper<LoadingWindow> {
    public Button btnStartGame;

    protected override void InitMsg() {
        btnStartGame.onClick.AddListener(StartGameClick);
    }

    protected override void ClearMsg() {
        btnStartGame.onClick.RemoveAllListeners();
    }

    private void StartGameClick() {
        BattleMgr.Instance.CreateBattle();
        WindowMgr.Instance.CloseWindow<LoadingWindow>();
    }
}

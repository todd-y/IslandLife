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
        LocalAssetMgr.Instance.Load_Scene("Battle");
        WindowMgr.Instance.CloseWindow<LoadingWindow>();
    }
}
